using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BE.Common;
using BE.Diagnostic;
using BE.Service;
using BE.Sigesoft;
using DAL.Diagnostic;
using static  BE.Common.Enumeratores;

namespace BL.Diagnostic
{
    public class DiagnosticBl
    {
        private readonly DiagnosticDal _oDiagnosticDal = new DiagnosticDal();

        public List<DiagnosticCustom> GetDiagnosticsByServiceId(string serviceId)
        {
            return _oDiagnosticDal.GetDiagnosticsByServiceId(serviceId);
        }

        //private DiagnosticDal.DiagnosticHandler _filHandler;
        public string SaveDiagnostics(List<DiagnosticCustom> diagnostics, int nodeId, int systemUserId)
        {
            //var addDiagnostic = FindDxAddTemp(diagnostics, nodeId, systemUserId);
            //var editDiagnostic = FindDxEditNonTemp(diagnostics, nodeId, systemUserId);
            //var removeDiagnostic = FindDxRemoveNonTemp(diagnostics, nodeId, systemUserId);

            //var addRecomendation = FindDxAddTemp(diagnostics, nodeId, systemUserId);
            //var editRecomendation = FindDxEditNonTemp(diagnostics, nodeId, systemUserId);
            //var removeRecomendation = FindDxRemoveNonTemp(diagnostics, nodeId, systemUserId);

            //var addRestriction = FindDxAddTemp(diagnostics, nodeId, systemUserId);
            //var editRestriction = FindDxEditNonTemp(diagnostics, nodeId, systemUserId);
            //var removeRestriction = FindDxRemoveNonTemp(diagnostics, nodeId, systemUserId);

            //new DiagnosticDal().SaveDiagnostics(addDiagnostic, editDiagnostic, removeDiagnostic, nodeId, systemUserId);

            //return "";
            #region Logic delegate
            //FindDxAddTemp(diagnostics, nodeId, systemUserId);
            //FindDxEditNonTemp(diagnostics, nodeId, systemUserId);
            //FindDxRemoveNonTemp(diagnostics, nodeId, systemUserId);
            #endregion

            new DiagnosticDal().AddDiagnosticRepository(diagnostics ,nodeId, systemUserId);

            return "";
        }

        public List<string> Searchmasterrecommendationrestricction(string name, int typeId)
        {
            return _oDiagnosticDal.Searchmasterrecommendationrestricction(name, typeId);
        }

        #region Private Methods

        private List<DiagnosticRepositoryDto> FindDxAddTemp(List<DiagnosticCustom> diagnostics, int nodeId, int systemUserId)
        {
            var list = new List<DiagnosticRepositoryDto>();
            var diagnosticslList = diagnostics.FindAll(p => p.RecordType == (int)RecordType.Temporal &&
                                                        (p.RecordStatus == (int)RecordStatus.Agregado ||
                                                        p.RecordStatus == (int)RecordStatus.Editado))
                                                        .ToList();

            if (diagnosticslList.Count == 0) return null;

            foreach (var dx in diagnosticslList)
            {
                var diagnosticRepositoryDto = new DiagnosticRepositoryDto
                {
                    v_ComponentId = dx.ComponentId.Contains('|') ? (dx.ComponentId.Split('|'))[0] : dx.ComponentId,
                    v_ServiceId = dx.ServiceId,
                    v_DiseasesId = dx.DiseaseId,
                    v_ComponentFieldId = dx.ComponentFieldsId,

                    //HardCode Temporal
                    i_AutoManualId = (int)AutoManual.Manual,// dx.AutoManualId,
                    i_PreQualificationId = (int)PreQualification.Aceptado,// dx.PreQualificationId,
                    i_FinalQualificationId = (int)FinalQualification.Definitivo,// dx.FinalQualificationId,
                    i_DiagnosticTypeId = (int)TipoDx.Enfermedad_Comun,//  dx.DiagnosticTypeId,
                    
                    i_IsSentToAntecedent = dx.IsSentToAntecedent,
                    d_ExpirationDateDiagnostic = dx.ExpirationDateDiagnostic,
                    i_DiagnosticSourceId = dx.DiagnosticSourceId,
                    i_ShapeAccidentId = dx.ShapeAccidentId,
                    i_BodyPartId = dx.BodyPartId,
                    i_ClassificationOfWorkAccidentId = dx.ClassificationOfWorkAccidentId,
                    i_RiskFactorId = dx.RiskFactorId,
                    i_ClassificationOfWorkdiseaseId = dx.ClassificationOfWorkdiseaseId
                };

                list.Add(diagnosticRepositoryDto);
            }
            //_filHandler += _oDiagnosticDal.AddTemporaryDiagnostics;
            //_oDiagnosticDal.SaveDiagnostics(list, nodeId, systemUserId, _filHandler);
            return list;
        }

        private List<DiagnosticRepositoryDto> FindDxEditNonTemp(List<DiagnosticCustom> diagnostics, int nodeId, int systemUserId)
        {
            var list = new List<DiagnosticRepositoryDto>();
            var diagnosticslList = diagnostics.FindAll(p => p.RecordType == (int)RecordType.Temporal &&
                                                            (p.RecordStatus == (int)RecordStatus.Agregado ||
                                                             p.RecordStatus == (int)RecordStatus.Editado))
                                                        .ToList();

            if (diagnosticslList.Count == 0) return null;

            foreach (var dx in diagnosticslList)
            {
                var diagnosticRepositoryDto = new DiagnosticRepositoryDto
                {
                    i_AutoManualId = dx.AutoManualId,
                    i_PreQualificationId = dx.PreQualificationId,
                    v_ComponentId = dx.ComponentId.Split('|')[0],
                    i_FinalQualificationId = dx.FinalQualificationId,
                    i_DiagnosticTypeId = dx.DiagnosticTypeId,
                    i_IsSentToAntecedent = dx.IsSentToAntecedent,
                    d_ExpirationDateDiagnostic = dx.ExpirationDateDiagnostic,
                    i_DiagnosticSourceId = dx.DiagnosticSourceId,
                    i_ShapeAccidentId = dx.ShapeAccidentId,
                    i_BodyPartId = dx.BodyPartId,
                    i_ClassificationOfWorkAccidentId = dx.ClassificationOfWorkAccidentId,
                    i_RiskFactorId = dx.RiskFactorId,
                    i_ClassificationOfWorkdiseaseId = dx.ClassificationOfWorkdiseaseId
                };

                list.Add(diagnosticRepositoryDto);

            }

            //_filHandler += _oDiagnosticDal.ModifyNonTemporalDiagnostics;
            //_oDiagnosticDal.SaveDiagnostics(list, nodeId, systemUserId, _filHandler);

            return list;
        }

        private List<DiagnosticRepositoryDto> FindDxRemoveNonTemp(List<DiagnosticCustom> diagnostics, int nodeId, int systemUserId)
        {
            var list = new List<DiagnosticRepositoryDto>();
            var diagnosticslList = diagnostics.FindAll(p => p.RecordType == (int)RecordType.NoTemporal &&
                                                            p.RecordStatus == (int)RecordStatus.Eliminado)
                                                        .ToList();

            if (diagnosticslList.Count == 0) return null;

            foreach (var dx in diagnosticslList)
            {
                var oDiagnosticRepositoryDto = new DiagnosticRepositoryDto();
                oDiagnosticRepositoryDto.v_DiagnosticRepositoryId = dx.DiagnosticRepositoryId;
                list.Add(oDiagnosticRepositoryDto);
            }
            //_filHandler += _oDiagnosticDal.RemoveNonTemporalDiagnostics;
            //_oDiagnosticDal.SaveDiagnostics(list, nodeId, systemUserId, _filHandler);

            return list;
        }


        private List<RecommendationDto> FindAddRecomendation(List<Recommendation> recomendations, string serviceId, string componentId, int nodeId, int systemUserId)
        {
            var list = new List<RecommendationDto>();
            var recommendations = recomendations.FindAll(p => p.RecordType == (int)RecordType.Temporal &&
                                                        (p.RecordStatus == (int)RecordStatus.Agregado ||
                                                        p.RecordStatus == (int)RecordStatus.Editado))
                                                        .ToList();

            
            return null;
        }

        #endregion

    }
}
