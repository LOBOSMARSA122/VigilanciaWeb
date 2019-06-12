using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Message;
using BE.Protocol;
using BE.Service;
using BL.Protocol;
using DAL.Calendar;
using DAL.Hospitalizacion;
using DAL.Organizarion;
using DAL.Protocol;
using DAL.Service;
using static BE.Common.Enumeratores;

namespace BL.Service
{
    public class ServiceBl
    {
        public string DarDeBaja(string personId)
        {
            return new ServiceDal().DarDeBaja(personId);
        }

        public string CreateService(ServiceCustom data, int nodeId, int userId)
        {
            string serviceId = "";
            List<ProtocolComponentCustom> ListProtocolComponent = new ProtocolComponentDal().GetProtocolComponents(data.ProtocolId);

            data.ProtocolId = new ProtocolBL().ReturnOrDuplicateProtocol(data, nodeId, userId, ListProtocolComponent);

            serviceId = new ServiceDal().CreateService(data, nodeId, userId);
            if (serviceId == null) return null;
            
            

            data.ServiceId = serviceId;

            if (data.MasterServiceTypeId == (int)MasterServiceType.Empresarial)
            {
                bool result = new ServiceComponentDal().AddServiceComponent(ListProtocolComponent, data, nodeId, userId);
                if (!result) return null;
            }
            else
            {
                bool result = new ServiceComponentDal().AddServiceComponent(ListProtocolComponent, data, nodeId, userId);
                if (!result) return null;
            }

            CalendarDto _CalendarDto = new CalendarDto();
            _CalendarDto.v_PersonId = data.PersonId;
            _CalendarDto.v_ServiceId = data.ServiceId;
            _CalendarDto.v_PersonId = data.PersonId;
            _CalendarDto.d_DateTimeCalendar = DateTime.Now;
            _CalendarDto.d_CircuitStartDate = DateTime.Now;
            _CalendarDto.d_EntryTimeCM = DateTime.Now;
            _CalendarDto.i_ServiceTypeId = data.MasterServiceTypeId;
            _CalendarDto.i_CalendarStatusId = 1;
            _CalendarDto.i_ServiceId = data.MasterServiceId;
            _CalendarDto.v_ProtocolId = data.ProtocolId;
            _CalendarDto.i_NewContinuationId = 1;
            _CalendarDto.i_LineStatusId = 1;
            _CalendarDto.i_IsVipId = 0;

            bool calendarResult = new CalendarDal().AddCalendar(_CalendarDto, nodeId, userId);
            if (!calendarResult) return null;
            int tipoEmpresa = ProtocolDal.ObtenerTipoEmpresaByProtocol(data.ProtocolId);
            if ((data.MasterServiceId == 19 || data.MasterServiceId == 10 || data.MasterServiceId == 15 || data.MasterServiceId == 16 || data.MasterServiceId == 17 || data.MasterServiceId == 18 || data.MasterServiceId == 19) && tipoEmpresa == 4)
            {
                bool resultHospi = new HospitalizacionDal().AddHospitalizacion(data.PersonId, data.ServiceId, nodeId, userId);
                if (!resultHospi) return null;
            }

            return serviceId;
        }

        public bool UpdateServiceForProtocol(ServiceCustom data, int userId)
        {
            bool resultServiceComponent = new ServiceComponentDal().UpdateServiceComponent(data.ServiceId, userId);
            if (!resultServiceComponent) return false;

            return new ServiceDal().UpdateServiceForProtocolo(data, userId);
        }

        public bool RegistrarCarta(MultiDataModel data)
        {
            return new ServiceDal().RegistrarCarta(data);
        }

        public MessageCustom FusionarServicios(List<string> ServicesId, int nodeId, int userId) {
            return new ServiceDal().FusionarServicios(ServicesId, userId, nodeId);
        }
    }
}
