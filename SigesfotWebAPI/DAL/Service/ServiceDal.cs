using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using BE.Service;

namespace DAL.Service
{
    public class ServiceDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public string AddService(ServiceDto oServiceDto, int nodeId, int systemUserId)
        {
            var serviceId = new Common.Utils().GetPrimaryKey(nodeId, 23, "SR");
            
            oServiceDto.v_ServiceId = serviceId;
            oServiceDto.i_IsDeleted = (int) Enumeratores.SiNo.No;
            oServiceDto.d_InsertDate = DateTime.UtcNow;
            oServiceDto.i_InsertUserId = systemUserId;

            ctx.Service.Add(oServiceDto);
            ctx.SaveChanges();
            
            return oServiceDto.v_ServiceId;
        }

        public string DarDeBaja(string personId)
        {
            var services = (from a in ctx.Service where a.v_PersonId == personId && a.i_IsDeleted == 0 select a).ToList();
            
            foreach (var service in services)
            {
                service.i_StatusVigilanciaId = 2;
            }

            ctx.SaveChanges();
            return personId;
        }

        public Enumeratores.ServiceStatus GetServiceStatus(string serviceId)
        {
            using (var contex = new DatabaseContext())
            {
                var serviceStatusId = (from a in contex.Service where a.v_ServiceId == serviceId select a).FirstOrDefault().i_ServiceStatusId;

                if (serviceStatusId == (int)Enumeratores.ServiceStatus.Culminado)
                {
                    return Enumeratores.ServiceStatus.Culminado;
                }
                else if (serviceStatusId == (int)Enumeratores.ServiceStatus.Incompleto)
                {
                    return Enumeratores.ServiceStatus.Incompleto;
                }
                else if (serviceStatusId == (int)Enumeratores.ServiceStatus.Iniciado || serviceStatusId == (int)Enumeratores.ServiceStatus.PorIniciar)
                {
                    return Enumeratores.ServiceStatus.Iniciado;
                }
                else 
                {
                    return Enumeratores.ServiceStatus.Cancelado;
                }
            }
        }
    }
}
