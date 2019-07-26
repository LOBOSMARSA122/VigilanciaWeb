using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using BE.Categoria;
using BE.Common;
using BE.Component;
using BE.Message;
using BE.Organization;
using BE.Pacient;
using BE.Protocol;
using BE.ProtocolComponent;
using BE.RoleNodeComponentProfile;
using BE.Service;
using BE.Sigesoft;
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

            if (data.FechaCalendario == null)
            {
                data.ProtocolId = new ProtocolBL().ReturnOrDuplicateProtocol(data, nodeId, userId, ListProtocolComponent);
            }

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
            _CalendarDto.d_DateTimeCalendar = data.FechaCalendario == null ? DateTime.Now : data.FechaCalendario;
            _CalendarDto.d_CircuitStartDate = data.FechaCalendario == null ? DateTime.Now : data.FechaCalendario;
            _CalendarDto.d_EntryTimeCM = data.FechaCalendario == null ? DateTime.Now : data.FechaCalendario;
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

        public List<SaldoPaciente> GetListSaldosPaciente(string serviceId)
        {
            return new ServiceDal().GetListSaldosPaciente(serviceId);
        }

        public List<RoleNodeComponentProfileCustom> GetRoleNodeComponentProfileByRoleNodeId(int pintNodeId, int pintRoleId)
        {
            return new ServiceDal().GetRoleNodeComponentProfileByRoleNodeId(pintNodeId, pintRoleId);
        }

        public List<ServiceComponentList> GetServiceComponentsCulminados(string pstrServiceId)
        {
            return new ServiceDal().GetServiceComponentsCulminados(pstrServiceId);
        }

        public List<ServiceComponentList> GetServiceComponents(string pstrServiceId)
        {
            return new ServiceDal().GetServiceComponents(pstrServiceId);
        }

        public bool PermitirLlamar(string pstrServiceId, int pintPiso)
        {
            return new ServiceDal().PermitirLlamar(pstrServiceId, pintPiso);
        }

        public List<ServiceComponentList> GetServiceComponentByCategoryId(int pstrCategoryId, string pstrServiceId)
        {
            return new ServiceDal().GetServiceComponentByCategoryId(pstrCategoryId, pstrServiceId);
        }

        public bool UpdateAdditionalExam(List<ServiceComponentList> pobjDtoEntity, string serviceId, int isRequiredId, int userId)
        {
            return new ServiceDal().UpdateAdditionalExam(pobjDtoEntity, serviceId, isRequiredId, userId);
        }

        public bool AddAdditionalExam(List<AdditionalExamCustom> listAdditionalExam, int userId, int nodeId)
        {
            try
            {
                using (var ts = new TransactionScope())
                {
                    bool Result = new ServiceDal().AddAdditionalExam(listAdditionalExam, userId, nodeId);
                    if (!Result) throw new Exception("Error");
                    ts.Complete();
                }
                return true;     
            }
            catch (Exception ex)
            {
                return false;
            }

        }
        public UsuarioGrabo DevolverDatosUsuarioFirma(int systemuserId)
        {
            return new ServiceDal().DevolverDatosUsuarioFirma(systemuserId);
        }

        public OrganizationDto GetInfoMedicalCenter()
        {
            return new ServiceDal().GetInfoMedicalCenter();
        }

        public BoardServiceCustomList GetServicesPagedAndFiltered(BoardServiceCustomList data)
        {
            return new ServiceDal().GetServicesPagedAndFiltered(data);
        }

        public static List<Categoria> GetAllComponentsByService(string serviceId)
        {
            return ServiceComponentDal.GetAllComponentsByService(serviceId);
        }

        public static OrganizationCustom GetInfoMedicalCenterSede()
        {
            return ServiceDal.GetInfoMedicalCenterSede();
        }
    }
}
