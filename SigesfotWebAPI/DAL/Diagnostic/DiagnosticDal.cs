using System;
using System.Collections.Generic;
using System.Linq;
using BE.Common;
using BE.Diagnostic;
using BE.Service;
using static BE.Common.Enumeratores;

namespace DAL.Diagnostic
{
    public class DiagnosticDal
    {
        public delegate void DiagnosticHandler(List<DiagnosticRepositoryDto> diagnosticRepositoryDtos, int nodeId, int systemUserId);

        private static readonly DatabaseContext Ctx = new DatabaseContext();
    
        public List<DiagnosticCustom> GetDiagnosticsByServiceId(string serviceId)
        {
            var query = (from  a in Ctx.DiagnosticRepository
                join b in Ctx.Diseases on a.v_DiseasesId equals b.v_DiseasesId
                join c in Ctx.SystemParameter on new {a = a.i_AutoManualId.Value, b = 136}
                    equals new {a = c.i_ParameterId, b = c.i_GroupId}
                join d in Ctx.SystemParameter on new {a = a.i_PreQualificationId.Value, b = 137}
                    equals new {a = d.i_ParameterId, b = d.i_GroupId}
                join e in Ctx.SystemParameter on new {a = a.i_FinalQualificationId.Value, b = 138}
                    equals new {a = e.i_ParameterId, b = e.i_GroupId}
                join f in Ctx.SystemParameter on new {a = a.i_DiagnosticTypeId.Value, b = 139}
                    equals new {a = f.i_ParameterId, b = f.i_GroupId}
                where a.v_ServiceId == serviceId
                      && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                select new DiagnosticCustom
                {
                    DiagnosticRepositoryId = a.v_DiagnosticRepositoryId,
                    DiseaseId = a.v_DiseasesId,
                    DiseaseName = b.v_Name,
                    AutoManual = c.v_Value1,
                    PreQualificationId = a.i_PreQualificationId,
                    FinalQualificationId = a.i_FinalQualificationId,
                    DiagnosticTypeId = a.i_DiagnosticTypeId,
                    PreQualificationName = d.v_Value1,
                    FinalQualificationName = e.v_Value1,
                    DiagnosticTypeName = f.v_Value1,
                    Cie10 = b.v_CIE10Id,    
                    ComponentId = a.v_ComponentId,
                    RecordStatus = (int) RecordStatus.Grabado,
                    RecordType = (int) RecordType.NoTemporal,
                    Recommendations = (from RecommendationDto s1A in Ctx.Recommendation
                        join s1B in Ctx.MasterRecommendationRestricction on s1A.v_MasterRecommendationId equals s1B
                            .v_MasterRecommendationRestricctionId
                        where s1A.v_ServiceId == serviceId && s1A.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId && s1A.i_IsDeleted == (int)Enumeratores.SiNo.No
                        select new Recommendation
                        {
                            DiseaseId = a.v_DiseasesId,
                            DiseaseName = b.v_Name,
                            RecommendationId = s1A.v_RecommendationId,
                            RecommendationName = s1B.v_Name,
                            RecordStatus = (int)RecordStatus.Grabado,
                            RecordType = (int)RecordType.NoTemporal,
                        }).ToList(),
                    Restrictions = (from RestrictionDto s2A in Ctx.Restriction
                        join s2B in Ctx.MasterRecommendationRestricction on s2A.v_MasterRestrictionId equals s2B
                            .v_MasterRecommendationRestricctionId
                        where s2A.v_ServiceId == serviceId && s2A.v_DiagnosticRepositoryId == a.v_DiagnosticRepositoryId && s2B.i_IsDeleted == (int)Enumeratores.SiNo.No
                        select new Restriction
                        {
                            DiseaseId = a.v_DiseasesId,
                            DiseaseName = b.v_Name,
                            RestrictionId = s2A.v_RestrictionId,
                            RestrictionName = s2B.v_Name,
                            RecordStatus = (int)RecordStatus.Grabado,
                            RecordType = (int)RecordType.NoTemporal,
                        }).ToList()
                }).ToList();

            return query;
        }

        //public void SaveDiagnostics(List<DiagnosticRepositoryDto> diagnostics, int nodeId, int systemUserId, DiagnosticHandler diagnosticHandler)
        //{
        //    diagnosticHandler(diagnostics, nodeId, systemUserId);
        //}

        public void SaveDiagnostics(List<DiagnosticRepositoryDto> addDiagnostics, List<DiagnosticRepositoryDto> editDiagnostics, List<DiagnosticRepositoryDto> removeDiagnostics, int nodeId, int systemUserId)
        {
            using (var ctx = new DatabaseContext())
            {
                if (addDiagnostics.Count > 0) AddTemporaryDiagnostics(addDiagnostics, nodeId, systemUserId);
                if (editDiagnostics.Count > 0) EditNonTemporalDiagnostics(editDiagnostics, nodeId, systemUserId);
                if (removeDiagnostics.Count > 0) AddTemporaryDiagnostics(removeDiagnostics, nodeId, systemUserId);
            }
        }

        public void AddTemporaryDiagnostics(List<DiagnosticRepositoryDto> diagnostics, int nodeId, int systemUserId)
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {
                    foreach (var dto in diagnostics)
                    {
                        var id = new Common.Utils().GetPrimaryKey(nodeId, 29, "DR");

                        dto.v_DiagnosticRepositoryId = id;
                        dto.i_IsDeleted = (int)SiNo.No;
                        dto.d_InsertDate = DateTime.UtcNow;
                        dto.i_InsertUserId = systemUserId;

                        ctx.DiagnosticRepository.Add(dto);
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }

        public void EditNonTemporalDiagnostics(List<DiagnosticRepositoryDto> diagnostics, int nodeId, int systemUserId)
        {
            foreach (var dto in diagnostics)
            {
                var objEntitySource = (from a in Ctx.DiagnosticRepository
                    where a.v_DiagnosticRepositoryId == dto.v_DiagnosticRepositoryId
                    select a).FirstOrDefault();

                if (objEntitySource == null) continue;
                objEntitySource.i_AutoManualId = dto.i_AutoManualId;
                objEntitySource.i_PreQualificationId = dto.i_PreQualificationId;
                objEntitySource.v_ComponentId = dto.v_ComponentId;
                objEntitySource.i_FinalQualificationId = dto.i_FinalQualificationId;

                objEntitySource.i_DiagnosticTypeId = dto.i_DiagnosticTypeId;
                objEntitySource.i_IsSentToAntecedent = dto.i_IsSentToAntecedent;
                objEntitySource.d_ExpirationDateDiagnostic = dto.d_ExpirationDateDiagnostic;

                objEntitySource.i_DiagnosticSourceId = dto.i_DiagnosticSourceId;
                objEntitySource.i_ShapeAccidentId = dto.i_ShapeAccidentId;
                objEntitySource.i_BodyPartId = dto.i_BodyPartId;
                objEntitySource.i_ClassificationOfWorkAccidentId = dto.i_ClassificationOfWorkAccidentId;

                objEntitySource.i_RiskFactorId = dto.i_RiskFactorId;
                objEntitySource.i_ClassificationOfWorkdiseaseId = dto.i_ClassificationOfWorkdiseaseId;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = systemUserId;
            }
            Ctx.SaveChanges();
        }

        public void RemoveNonTemporalDiagnostics(List<DiagnosticRepositoryDto> diagnostics, int nodeId, int systemUserId)
        {
            foreach (var dto in diagnostics)
            {
                var objEntitySource = (from a in Ctx.DiagnosticRepository
                    where a.v_DiagnosticRepositoryId == dto.v_DiagnosticRepositoryId
                    select a).FirstOrDefault();

                if (objEntitySource == null) continue;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = systemUserId;
                objEntitySource.i_IsDeleted = (int)SiNo.Si;
            }
            Ctx.SaveChanges();
        }

        public List<string> Searchmasterrecommendationrestricction(string name, int typeId)
        {
            var query = (from a in Ctx.MasterRecommendationRestricction
                where a.v_Name.Contains(name) && a.i_IsDeleted == (int)SiNo.No && a.i_TypifyingId == typeId
                         select new
                {
                    value = a.v_Name + "|" + a.v_MasterRecommendationRestricctionId
                }).ToList();

            return query.Select(p => p.value).ToList();

        }

        public void AddDiagnosticRepository(List<DiagnosticCustom> pobjDiagnosticRepository, int nodeId, int systemUserId)
        {
            try
            {
                string NewId0 = "(No generado)";
                string componentId = null;

                if (pobjDiagnosticRepository != null)
                {
                    foreach (var dr in pobjDiagnosticRepository)
                    {

                        #region DiagnosticRepository -> ADD / UPDATE / DELETE

                        // ADD
                        if (dr.RecordType == (int)RecordType.Temporal && (dr.RecordStatus == (int)RecordStatus.Agregado || dr.RecordStatus == (int)RecordStatus.Editado))
                        {
                            DiagnosticRepositoryDto objEntity = new DiagnosticRepositoryDto();

                            // En caso de haber mas de un ComponentID quiere decir que lo datos provienen de un examen agrupador con una categoria (LAB,PSICOLOGIA) 
                            // entonces cojo el ID del hijo mayor (osea el primer ID)[0]
                            // Buscar un palote
                            if (dr.ComponentId != null)
                            {
                                if (dr.ComponentId.Contains('|'))
                                    componentId = (dr.ComponentId.Split('|'))[0];
                                else
                                    componentId = dr.ComponentId;
                            }

                            objEntity.v_DiagnosticRepositoryId = dr.DiagnosticRepositoryId;
                            objEntity.v_ServiceId = dr.ServiceId;
                            objEntity.v_ComponentId = componentId;
                            objEntity.v_DiseasesId = dr.DiseaseId;
                            // ID del Control que generó el DX automático [v_ComponentFieldsId]
                            objEntity.v_ComponentFieldId = dr.ComponentFieldsId;
                            objEntity.i_AutoManualId = (int)AutoManual.Manual;// dr.AutoManualId;
                            objEntity.i_PreQualificationId = (int)PreQualification.Aceptado;// dr.PreQualificationId;
                                                                                            // Total Diagnósticos
                            objEntity.i_FinalQualificationId = (int)FinalQualification.Definitivo;// dr.FinalQualificationId;
                            objEntity.i_DiagnosticTypeId = (int)TipoDx.Enfermedad_Comun;// dr.DiagnosticTypeId;
                            objEntity.i_IsSentToAntecedent = dr.IsSentToAntecedent;
                            objEntity.d_ExpirationDateDiagnostic = dr.ExpirationDateDiagnostic;

                            objEntity.d_InsertDate = DateTime.Now;
                            objEntity.i_InsertUserId = systemUserId;
                            objEntity.i_IsDeleted = 0;

                            // Accidente laboral
                            objEntity.i_DiagnosticSourceId = dr.DiagnosticSourceId;
                            objEntity.i_ShapeAccidentId = dr.ShapeAccidentId;

                            objEntity.i_BodyPartId = dr.BodyPartId;
                            objEntity.i_ClassificationOfWorkAccidentId = dr.ClassificationOfWorkAccidentId;

                            // Enfermedad laboral
                            objEntity.i_RiskFactorId = dr.RiskFactorId;
                            objEntity.i_ClassificationOfWorkdiseaseId = dr.ClassificationOfWorkdiseaseId;

                            // Autogeneramos el Pk de la tabla                      
                            NewId0 = new Common.Utils().GetPrimaryKey(nodeId, 29, "DR");
                            // Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 29), "DR");
                            objEntity.v_DiagnosticRepositoryId = NewId0;

                            Ctx.DiagnosticRepository.Add(objEntity);

                        } // UPDATE
                        else if (dr.RecordType == (int)RecordType.NoTemporal && dr.RecordStatus == (int)RecordStatus.Editado)
                        {
                            // Obtener la entidad fuente
                            var objEntitySource = (from a in Ctx.DiagnosticRepository
                                                   where a.v_DiagnosticRepositoryId == dr.DiagnosticRepositoryId
                                                   select a).FirstOrDefault();

                            // Crear la entidad con los datos actualizados   
                            objEntitySource.i_AutoManualId = (int)AutoManual.Manual;//dr.AutoManualId;
                            objEntitySource.i_PreQualificationId = (int)PreQualification.Aceptado;//dr.PreQualificationId;
                            objEntitySource.v_ComponentId = dr.ComponentId.Split('|')[0];
                            // ID del Control que generó el DX automático [v_ComponentFieldsId]
                            //objEntitySource.v_ComponentFieldsId = dr.v_ComponentFieldsId;
                            // Total Diagnósticos
                            if (objEntitySource.i_FinalQualificationId == null)
                                objEntitySource.i_FinalQualificationId = dr.FinalQualificationId;


                            
                            objEntitySource.i_DiagnosticTypeId = (int)TipoDx.Enfermedad_Comun;//dr.DiagnosticTypeId;
                            objEntitySource.i_IsSentToAntecedent = dr.IsSentToAntecedent;
                            objEntitySource.d_ExpirationDateDiagnostic = dr.ExpirationDateDiagnostic;

                            // Accidente laboral
                            objEntitySource.i_DiagnosticSourceId = dr.DiagnosticSourceId;
                            objEntitySource.i_ShapeAccidentId = dr.ShapeAccidentId;
                            objEntitySource.i_BodyPartId = dr.BodyPartId;
                            objEntitySource.i_ClassificationOfWorkAccidentId = dr.ClassificationOfWorkAccidentId;

                            // Enfermedad laboral
                            objEntitySource.i_RiskFactorId = dr.RiskFactorId;
                            objEntitySource.i_ClassificationOfWorkdiseaseId = dr.ClassificationOfWorkdiseaseId;

                            objEntitySource.d_UpdateDate = DateTime.Now;
                            objEntitySource.i_UpdateUserId = systemUserId;

                        } // DELETE
                        else if (dr.RecordType == (int)RecordType.NoTemporal && dr.RecordStatus == (int)RecordStatus.Eliminado)
                        {
                            // Obtener la entidad fuente
                            var objEntitySource = (from a in Ctx.DiagnosticRepository
                                                   where a.v_DiagnosticRepositoryId == dr.DiagnosticRepositoryId
                                                   select a).FirstOrDefault();

                            // Crear la entidad con los datos actualizados                                                           
                            objEntitySource.d_UpdateDate = DateTime.Now;
                            objEntitySource.i_UpdateUserId = systemUserId;
                            objEntitySource.i_IsDeleted = 1;

                        }

                        #endregion

                        #region Restricciones -> ADD / DELETE

                        if (dr.Restrictions != null)
                        {
                            // Operaciones básicas [Add,Update,Delete] restricciones 
                            foreach (var r in dr.Restrictions)
                            {
                                if (r.RecordType == (int)RecordType.Temporal && r.RecordStatus == (int)RecordStatus.Agregado)
                                {
                                    RestrictionDto objRestriction = new RestrictionDto();

                                    var NewId1 = new Common.Utils().GetPrimaryKey(nodeId, 30, "RD");
                                    //Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 30), "RD");
                                    objRestriction.v_ServiceId = dr.ServiceId;
                                    objRestriction.v_ComponentId = dr.ComponentId.Split('|')[0];
                                    //objRestriction.v_RestrictionByDiagnosticId = NewId1;
                                    objRestriction.v_RestrictionId = NewId1;
                                    objRestriction.v_DiagnosticRepositoryId = NewId0 == "(No generado)" ? dr.DiagnosticRepositoryId : NewId0;

                                    objRestriction.v_MasterRestrictionId = r.MasterRestrictionId.Length > 16 ? null : r.MasterRestrictionId;
                                    objRestriction.d_InsertDate = DateTime.Now;
                                    objRestriction.i_InsertUserId = systemUserId;
                                    objRestriction.i_IsDeleted = 0;

                                    Ctx.Restriction.Add(objRestriction);

                                }
                                else if (r.RecordType == (int)RecordType.NoTemporal && r.RecordStatus == (int)RecordStatus.Eliminado)
                                {
                                    // Obtener la entidad fuente v_RestrictionByDiagnosticId
                                    var objEntitySource = (from a in Ctx.Restriction
                                                           where a.v_RestrictionId == r.RestrictionId
                                                           select a).FirstOrDefault();

                                    // Crear la entidad con los datos actualizados                                                           
                                    objEntitySource.d_UpdateDate = DateTime.Now;
                                    objEntitySource.i_UpdateUserId = systemUserId;
                                    objEntitySource.i_IsDeleted = 1;

                                }
                                //dbContext.SaveChanges();
                            }
                        }

                        #endregion

                        #region Recomendaciones -> ADD / DELETE

                        if (dr.Recommendations != null)
                        {
                            // Grabar recomendaciones 
                            foreach (var r in dr.Recommendations)
                            {
                                if (r.RecordType == (int)RecordType.Temporal && r.RecordStatus == (int)RecordStatus.Agregado)
                                {
                                    RecommendationDto objRecommendation = new RecommendationDto();

                                    var NewId1 = new Common.Utils().GetPrimaryKey(nodeId, 32, "RR");
                                    //Common.Utils.GetNewId(intNodeId, Utils.GetNextSecuentialId(intNodeId, 32), "RR");
                                    objRecommendation.v_ServiceId = dr.ServiceId;
                                    objRecommendation.v_ComponentId = dr.ComponentId.Split('|')[0];
                                    objRecommendation.v_RecommendationId = NewId1;
                                    objRecommendation.v_DiagnosticRepositoryId = NewId0 == "(No generado)" ? dr.DiagnosticRepositoryId : NewId0;

                                    //objRecommendation.v_MasterRecommendationId = r.v_RecommendationId.Length > 16 ? null : r.v_MasterRecommendationId;
                                    objRecommendation.v_MasterRecommendationId = r.MasterRecommendationId;
                                    objRecommendation.d_InsertDate = DateTime.Now;
                                    objRecommendation.i_InsertUserId = systemUserId;
                                    objRecommendation.i_IsDeleted = 0;

                                    Ctx.Recommendation.Add(objRecommendation);

                                }
                                else if (r.RecordType == (int)RecordType.NoTemporal && r.RecordStatus == (int)RecordStatus.Eliminado)
                                {
                                    // Obtener la entidad fuente
                                    var objEntitySource = (from a in Ctx.Recommendation
                                                           where a.v_RecommendationId == r.RecommendationId
                                                           select a).FirstOrDefault();

                                    // Crear la entidad con los datos actualizados                                                           
                                    objEntitySource.d_UpdateDate = DateTime.Now;
                                    objEntitySource.i_UpdateUserId = systemUserId;
                                    objEntitySource.i_IsDeleted = 1;

                                }
                                //dbContext.SaveChanges();
                            }
                        }

                        #endregion

                    }

                    // Guardar los cambios
                    Ctx.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                
            }
            
        }

    }
}
