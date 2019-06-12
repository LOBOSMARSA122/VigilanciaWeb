using System;
using System.Collections.Generic;
using System.Linq;
using BE.Common;
using static BE.Common.Enumeratores;
using BE.Plan;

namespace DAL.Plan
{
    public class PlanDal
    {
        private static DatabaseContext _ctx = new DatabaseContext();

        public List<PlanVigilanciaCustom> Filter(out int totalRecords, BoardPlanVigilancia data)
        {
            int skip = (data.Index - 1) * data.Take;

            var list = (from a in _ctx.PlanVigilancia
                        where (data.Name == null || a.v_Name.Contains(data.Name)) && a.v_OrganizationId == data.OrganizationId && a.i_IsDeleted == (int)SiNo.No
                        select new PlanVigilanciaCustom
                        {
                            PlanVigilanciaId = a.v_PlanVigilanciaId,
                            Name = a.v_Name,
                            Description = a.v_Description,
                            PlanDiseases = (from subA in _ctx.PlanVigilanciaDiseases
                                            join subB in _ctx.Diseases on subA.v_DiseasesId equals subB.v_DiseasesId
                                            where subA.v_PlanVigilanciaId == a.v_PlanVigilanciaId && subA.i_IsDeleted == (int)SiNo.No
                                            select new PlanDiseasesCustom
                                            {
                                                PlanVigilanciaDiseasesId = subA.v_PlanVigilanciaDiseasesId,
                                                PlanVigilanciaId = a.v_PlanVigilanciaId,
                                                DiseasesId = subA.v_DiseasesId,
                                                DiseasesName = subB.v_Name,
                                                Cie10 = subB.v_CIE10Id,
                                                RecordStatus = (int)RecordStatus.Grabado,
                                                RecordType = (int)RecordType.NoTemporal,
                                            }).ToList()

                        }).ToList();


            totalRecords = list.Count;

            if (data.Take > 0)
                list = list.Skip(skip).Take(data.Take).ToList();

            return list;
        }

        public List<PlanDiseasesCustom> ListPlanVigilanciaDiseases(string planVigianciaId)
        {
            var list = (from a in _ctx.PlanVigilanciaDiseases
                where a.v_PlanVigilanciaId == planVigianciaId
                     && a.i_IsDeleted == (int)SiNo.No
                select new PlanDiseasesCustom
                {
                    PlanVigilanciaDiseasesId = a.v_PlanVigilanciaDiseasesId,
                    PlanVigilanciaId = a.v_PlanVigilanciaId,
                    DiseasesId = a.v_DiseasesId
                }).ToList();

            return list;
        }

        public PlanVigilanciaCustom GetId(string planVigianciaId)
        {
            
            var list = (from a in _ctx.PlanVigilancia
                where a.v_PlanVigilanciaId == planVigianciaId
                    select new PlanVigilanciaCustom
                    {
                    PlanVigilanciaId = a.v_PlanVigilanciaId,
                    Name = a.v_Name,
                    Description = a.v_Description,
                    PlanDiseases = (from subA in _ctx.PlanVigilanciaDiseases
                        join subB in _ctx.Diseases on subA.v_DiseasesId equals subB.v_DiseasesId
                        where subA.v_PlanVigilanciaId == a.v_PlanVigilanciaId
                        select new PlanDiseasesCustom
                        {
                            PlanVigilanciaDiseasesId = subA.v_PlanVigilanciaDiseasesId,
                            PlanVigilanciaId = a.v_PlanVigilanciaId,
                            DiseasesId = subA.v_DiseasesId,
                            DiseasesName = subB.v_Name,
                            Cie10 = subB.v_CIE10Id,
                            RecordStatus = (int)RecordStatus.Grabado,
                            RecordType = (int)RecordType.NoTemporal,
                        }).ToList()

                }).FirstOrDefault();
          
            return list;
        }
        
        public bool Save(PlanVigilanciaCustom oPlanVigilanciaCustom)
        {
            var planVigilanciaId = "";
            if (oPlanVigilanciaCustom.PlanVigilanciaId == null)
            {
                var oPlanVigilanciaDto = new PlanVigilanciaDto();
                oPlanVigilanciaDto.v_Name = oPlanVigilanciaCustom.Name;
                oPlanVigilanciaDto.v_Description = oPlanVigilanciaCustom.Description;
                oPlanVigilanciaDto.v_OrganizationId = oPlanVigilanciaCustom.OrganizationId;
                planVigilanciaId = AddPlanVigilancia(oPlanVigilanciaDto, oPlanVigilanciaCustom.NodeId, oPlanVigilanciaCustom.SystemUserId);
            }
            else
                EditPlanVigilancia(oPlanVigilanciaCustom);

            AddPlanVigilanciaDiseases(oPlanVigilanciaCustom.PlanDiseases, planVigilanciaId == "" ? oPlanVigilanciaCustom.PlanVigilanciaId: planVigilanciaId, oPlanVigilanciaCustom.NodeId, oPlanVigilanciaCustom.SystemUserId);

            return true;
        }

        public bool Remove(string planVigilanciaId, int systemUserId)
        {
            var objEntitySource = (from a in _ctx.PlanVigilancia
                where a.v_PlanVigilanciaId == planVigilanciaId
                select a).FirstOrDefault();

            if (objEntitySource != null)
            {
                objEntitySource.i_IsDeleted = (int)SiNo.Si;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = systemUserId;
            }

            var detail = (from a in _ctx.PlanVigilanciaDiseases
                where a.v_PlanVigilanciaId == planVigilanciaId
                select a).ToList();

            foreach (var item in detail)
            {
                item.i_IsDeleted = (int)SiNo.Si;
                item.d_UpdateDate = DateTime.Now;
                item.i_UpdateUserId = systemUserId;
            }
            _ctx.SaveChanges();
            return true;
        }

        public List<string> SearchDisease(string name)
        {
            var query = (from a in _ctx.Diseases
                where a.v_Name.Contains(name) && a.i_IsDeleted == (int) SiNo.No
                select new
                {
                    value = a.v_Name+ "|"+ a.v_DiseasesId + "|" + a.v_CIE10Id
                }).ToList();

            return query.Select(p => p.value).ToList();

        }

        public List<KeyValueDTO> ComboPlanesVigilancia(string organizationId)
        {
            using (DatabaseContext ctx = new DatabaseContext())
            {
                var list = (from a in ctx.PlanVigilancia
                    where a.i_IsDeleted == (int)SiNo.No
                          && a.v_OrganizationId == organizationId
                    select new KeyValueDTO
                    {
                        Id = a.v_PlanVigilanciaId,
                        Value = a.v_Name
                    }).ToList();

                return list;
            }

        }

        private string AddPlanVigilancia(PlanVigilanciaDto oPlanVigilanciaDto, int nodeId, int systemUserId)
        {
            var planVigilanciaId = new Common.Utils().GetPrimaryKey(nodeId, 220, "PV");

            oPlanVigilanciaDto.v_PlanVigilanciaId= planVigilanciaId;

            oPlanVigilanciaDto.i_IsDeleted = (int)SiNo.No;
            oPlanVigilanciaDto.d_InsertDate = DateTime.Now;
            oPlanVigilanciaDto.i_InsertUserId = systemUserId;

            _ctx.PlanVigilancia.Add(oPlanVigilanciaDto);
            _ctx.SaveChanges();
            return planVigilanciaId;
        }
        
        private void AddPlanVigilanciaDiseases(List<PlanDiseasesCustom> list, string planVigilanciaId,int nodeId, int systemUserId)
        {
            foreach (var item in list)
            {
                var oPlanVigilanciaDiseasesDto = new PlanVigilanciaDiseasesDto();

                if (item.RecordStatus == (int)RecordStatus.Agregado && item.RecordType == (int)RecordType.Temporal)
                {
                    var planVigilanciaDiseasesId = new Common.Utils().GetPrimaryKey(nodeId, 221, "VD");
                    oPlanVigilanciaDiseasesDto.v_PlanVigilanciaDiseasesId = planVigilanciaDiseasesId;
                    oPlanVigilanciaDiseasesDto.v_PlanVigilanciaId = planVigilanciaId;
                    oPlanVigilanciaDiseasesDto.v_DiseasesId = item.DiseasesId;
                    oPlanVigilanciaDiseasesDto.i_IsDeleted = (int)SiNo.No;
                    oPlanVigilanciaDiseasesDto.d_InsertDate = DateTime.Now;
                    oPlanVigilanciaDiseasesDto.i_InsertUserId = systemUserId;

                    _ctx.PlanVigilanciaDiseases.Add(oPlanVigilanciaDiseasesDto);

                }
                else if(item.RecordStatus == (int)RecordStatus.Editado && item.RecordType == (int)RecordType.NoTemporal)
                {
                    var objEntitySource = (from a in _ctx.PlanVigilanciaDiseases
                        where a.v_PlanVigilanciaDiseasesId == item.PlanVigilanciaDiseasesId
                        select a).FirstOrDefault();

                    if (objEntitySource != null)
                    {
                        objEntitySource.v_DiseasesId = item.DiseasesId;
                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = systemUserId;
                        //_ctx.PlanVigilanciaDiseases.Add(objEntitySource);
                    }
                }
                else if (item.RecordStatus == (int)RecordStatus.Eliminado && item.RecordType == (int)RecordType.NoTemporal)
                {
                    var objEntitySource = (from a in _ctx.PlanVigilanciaDiseases
                        where a.v_PlanVigilanciaDiseasesId == item.PlanVigilanciaDiseasesId
                        select a).FirstOrDefault();

                    if (objEntitySource != null)
                    {
                        objEntitySource.i_IsDeleted = (int)SiNo.Si;
                        objEntitySource.d_UpdateDate = DateTime.Now;
                        objEntitySource.i_UpdateUserId = systemUserId;
                        //_ctx.PlanVigilanciaDiseases.Add(objEntitySource);
                    }
                }
                _ctx.SaveChanges();
            }
        }

        private void EditPlanVigilancia(PlanVigilanciaCustom oPlanVigilanciaCustom)
        {

            var objEntitySource = (from a in _ctx.PlanVigilancia
                where a.v_PlanVigilanciaId == oPlanVigilanciaCustom.PlanVigilanciaId
                                   select a).FirstOrDefault();

            if (objEntitySource != null)
            {
                objEntitySource.v_Name = oPlanVigilanciaCustom.Name;
                objEntitySource.v_Description = oPlanVigilanciaCustom.Description;
                objEntitySource.d_UpdateDate = DateTime.Now;
                objEntitySource.i_UpdateUserId = oPlanVigilanciaCustom.SystemUserId;
                //_ctx.PlanVigilancia.Add(objEntitySource);
                _ctx.SaveChanges();
            }
        }

        public List<PlanBE> GetPlans(string protocolId, string UnidadProductiva)
        {
            try
            {
                var List = _ctx.Plan.Where(x => x.v_ProtocoloId == protocolId && x.v_IdUnidadProductiva == UnidadProductiva).ToList();
                return List;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
