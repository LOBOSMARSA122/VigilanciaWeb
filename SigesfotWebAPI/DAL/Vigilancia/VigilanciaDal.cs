using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Vigilancia;

namespace DAL.Vigilancia
{
    public class VigilanciaDal
    {
        private static DatabaseContext _ctx = new DatabaseContext();

        public string AddVigilancia(VigilanciaDto oVigilanciaDto, int nodeId, int systemUserId)
        {
            using (var ctx = new DatabaseContext())
            {
                var vigilanciaId = new Common.Utils().GetPrimaryKey(nodeId, 230, "VV");

                oVigilanciaDto.v_VigilanciaId = vigilanciaId;

                oVigilanciaDto.i_IsDeleted = (int)Enumeratores.SiNo.No;
                oVigilanciaDto.d_InsertDate = DateTime.UtcNow;
                oVigilanciaDto.i_InsertUserId = systemUserId;

                ctx.Vigilancia.Add(oVigilanciaDto);
                ctx.SaveChanges();

                return vigilanciaId;
            }
        }

        public string AddVigilanciaService(VigilanciaServiceDto oVigilanciaServiceDto, int nodeId, int systemUserId)
        {
            using (var ctx = new DatabaseContext())
            {
                var vigilanciaServiceId = new Common.Utils().GetPrimaryKey(nodeId, 231, "VS");

                oVigilanciaServiceDto.v_VigilanciaServiceId = vigilanciaServiceId;
                oVigilanciaServiceDto.v_VigilanciaId = oVigilanciaServiceDto.v_VigilanciaId;
                oVigilanciaServiceDto.i_IsDeleted = (int)Enumeratores.SiNo.No;
                oVigilanciaServiceDto.d_InsertDate = DateTime.UtcNow;
                oVigilanciaServiceDto.i_InsertUserId = systemUserId;

                ctx.VigilanciaService.Add(oVigilanciaServiceDto);
                ctx.SaveChanges();

                return vigilanciaServiceId;
            }
        }

        public bool FinishVigilancia(string vigilanciaId, int systemUserId)
        {
            using (var ctx = new DatabaseContext())
            {
                var objEntity = (from a in ctx.Vigilancia
                                where a.v_VigilanciaId == vigilanciaId 
                                        && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                                select a).FirstOrDefault();

                if (objEntity == null) return false;

                objEntity.d_EndDate = DateTime.Now;
                objEntity.i_UpdateUserId = systemUserId;
                objEntity.d_UpdateDate = DateTime.Now;
                ctx.SaveChanges();  
                return true;
            }
        }

        public List<VigilanciaServiceCustom> VigilanciaServiceDtos(int doctoRespondibleId, string organizationId)
        {
            var list = (from a in _ctx.Vigilancia
                join b in _ctx.VigilanciaService on a.v_VigilanciaId equals b.v_VigilanciaId
                join c in _ctx.Person on a.v_PersonId equals  c.v_PersonId
                join e in _ctx.Calendar on b.v_ServiceId equals e.v_ServiceId
                join d in _ctx.PlanVigilancia on a.v_PlanVigilanciaId equals d.v_PlanVigilanciaId
                        where a.i_DoctorRespondibleId == doctoRespondibleId && d.v_OrganizationId == organizationId
                select new VigilanciaServiceCustom
                {
                    PersonId = a.v_PersonId,
                    Trabajador = c.v_FirstName + " " + c.v_FirstLastName + " " + c.v_SecondLastName,
                    DateSchedule = e.d_DateTimeCalendar.ToString(),
                    DoctoResponsibleId = a.i_DoctorRespondibleId,
                    ServiceId = b.v_ServiceId
                }).ToList();
                        
            return list;
        }

        public bool ControlInProgress(string personId)
        {
            var query = (from a in _ctx.Vigilancia where a.v_PersonId == personId && a.i_StateVigilanciaId == (int)Enumeratores.StateVigilancia.Iniciado select a).ToList();

            return query.Count > 0;
        }

        public bool VerifyPlanStarted(string personId, string planVigilanciaId)
        {
            using (var ctx = new DatabaseContext())
            {
                var query = (from a in ctx.Vigilancia
                    where a.v_PersonId == personId && a.v_PlanVigilanciaId == planVigilanciaId
                          && a.i_StateVigilanciaId == (int)Enumeratores.StateVigilancia.Iniciado
                    select a).FirstOrDefault();
                return query != null;
            }
        }
    }
}
