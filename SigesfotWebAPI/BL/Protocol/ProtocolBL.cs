using BE.Message;
using BE.Organization;
using BE.Protocol;
using BE.ProtocolComponent;
using BE.Service;
using DAL.Organizarion;
using DAL.Protocol;
using DAL.SystemUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static BE.Common.Enumeratores;

namespace BL.Protocol
{
    public class ProtocolBL
    {
        public ProtocolCustom GetDataProtocol(string protocolId)
        {
            ProtocolDal protocolDal = new ProtocolDal();
            return protocolDal.GetDatosProtocolo(protocolId);
        }


        public static BoardProtocolComponent GetProtocolComponentByProtocolId(string protocolId) {
            BoardProtocolComponent data = new BoardProtocolComponent();
            data.ListProtocolComponents = new ProtocolComponentDal().GetProtocolComponents(protocolId);
            data.ListUsers = SystemUserDal.GetSystemUserExternal(protocolId);
            return data;
        }

        public static int GetSecuentialForOrderService(int nodeId, int tableId) {
            return DAL.Common.Utils.GetNextSecuentialNoSave(nodeId, tableId);
        }

        public string ReturnOrDuplicateProtocol(ServiceCustom data, int nodeId, int userId, List<ProtocolComponentCustom> ListProtocolComponent)
        {
            try
            {
                if (ListProtocolComponent == null) return null;

                var id = data.DataProtocol.EmpresaEmpleadora.Split('|');
                var id1 = data.DataProtocol.EmpresaCliente.Split('|');
                var id2 = data.DataProtocol.EmpresaTrabajo.Split('|');
                var masterServiceTypeId = data.DataProtocol.i_MasterServiceTypeId;
                var masterServiceId = data.DataProtocol.i_MasterServiceId;
                var groupOccupationName = data.DataProtocol.Geso;
                var esoTypeId = data.DataProtocol.i_EsoTypeId;
                bool ExisteProtocolo = ProtocolDal.ExisteProtocoloPropuestoSegunLaEmpresa(id[0], masterServiceTypeId, masterServiceId, groupOccupationName, esoTypeId);
                if (!ExisteProtocolo)
                {
                    ProtocolBE _ProtocolBE = new ProtocolBE();
                    var sufProtocol = data.DataProtocol.EmpresaEmpleadoraName.Split('/');
                    _ProtocolBE.v_Name = data.DataProtocol.ProtocolName + " " + sufProtocol[0].ToString();
                    _ProtocolBE.v_EmployerOrganizationId = id[0];
                    _ProtocolBE.v_EmployerLocationId = id[1];
                    _ProtocolBE.i_EsoTypeId = data.DataProtocol.i_EsoTypeId;
                    //obtener GESO
                    var gesoId = new OrganizationDal().GetGroupOcupation(id[1], groupOccupationName);
                    _ProtocolBE.v_GroupOccupationId = gesoId;
                    _ProtocolBE.v_CustomerOrganizationId = id1[0];
                    _ProtocolBE.v_CustomerLocationId = id1[1];
                    _ProtocolBE.v_WorkingOrganizationId = id2[0];
                    _ProtocolBE.v_WorkingLocationId = data.DataProtocol.EmpresaEmpleadora != "-1" ? id2[1] : "-1";
                    _ProtocolBE.i_MasterServiceId = masterServiceId;
                    _ProtocolBE.v_CostCenter = string.Empty;
                    _ProtocolBE.i_MasterServiceTypeId = masterServiceTypeId;
                    _ProtocolBE.i_HasVigency = 1;
                    _ProtocolBE.i_ValidInDays = null;
                    _ProtocolBE.i_IsActive = 1;
                    _ProtocolBE.v_NombreVendedor = string.Empty;

                    List<ProtocolComponentDto> ListProtocolComponentDto = new List<ProtocolComponentDto>();
                    foreach (var objProtCom in ListProtocolComponent)
                    {
                        ProtocolComponentDto _ProtocolComponentDto = new ProtocolComponentDto();
                        _ProtocolComponentDto.v_ComponentId = objProtCom.ComponentId;
                        _ProtocolComponentDto.r_Price = objProtCom.Price;
                        _ProtocolComponentDto.i_OperatorId = objProtCom.OperatorId;
                        _ProtocolComponentDto.i_Age = objProtCom.Age;
                        _ProtocolComponentDto.i_GenderId = objProtCom.GenderId;
                        _ProtocolComponentDto.i_IsAdditional = objProtCom.IsAdditional;
                        _ProtocolComponentDto.i_IsConditionalId = objProtCom.IsConditionalId;
                        _ProtocolComponentDto.i_GrupoEtarioId = objProtCom.GrupoEtarioId;
                        _ProtocolComponentDto.i_IsConditionalIMC = objProtCom.IsConditionalIMC;
                        _ProtocolComponentDto.r_Imc = objProtCom.Imc;
                        ListProtocolComponentDto.Add(_ProtocolComponentDto);
                    }
                    string protocolId = ProtocolDal.AddProtocol(_ProtocolBE, ListProtocolComponentDto, nodeId, userId);
                    if (protocolId == null) return null;

                    var ListUser = ProtocolDal.GetSystemUserSigesoft();
                    var extUserWithCustomer = ListUser.FindAll(p => p.v_SystemUserByOrganizationId == id1[0]).ToList();
                    var extUserWithEmployer = ListUser.FindAll(p => p.v_SystemUserByOrganizationId == id[0]).ToList();
                    var extUserWithWorking = ListUser.FindAll(p => p.v_SystemUserByOrganizationId == id2[0]).ToList();

                    foreach (var extUs in extUserWithCustomer)
                    {
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId);
                        var list = new List<ProtocolSystemUserBE>();
                        foreach (var perm in ListUserExter)
                        {
                            var oProtocolSystemUserBEo = new ProtocolSystemUserBE();
                            oProtocolSystemUserBEo.i_SystemUserId = extUs.i_SystemUserId;
                            oProtocolSystemUserBEo.v_ProtocolId = protocolId;
                            oProtocolSystemUserBEo.i_ApplicationHierarchyId = perm.i_ApplicationHierarchyId;
                            list.Add(oProtocolSystemUserBEo);
                        }
                        bool resultUs = ProtocolDal.AddProtocolSystemUser(list, userId, nodeId);
                        if (!resultUs) return null;
                    }
                    foreach (var extUs in extUserWithEmployer)
                    {
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId);
                        var list = new List<ProtocolSystemUserBE>();
                        foreach (var perm in ListUserExter)
                        {
                            var oProtocolSystemUserBEo = new ProtocolSystemUserBE();
                            oProtocolSystemUserBEo.i_SystemUserId = extUs.i_SystemUserId;
                            oProtocolSystemUserBEo.v_ProtocolId = protocolId;
                            oProtocolSystemUserBEo.i_ApplicationHierarchyId = perm.i_ApplicationHierarchyId;
                            list.Add(oProtocolSystemUserBEo);
                        }
                        bool resultUs = ProtocolDal.AddProtocolSystemUser(list, userId, nodeId);
                        if (!resultUs) return null;
                    }
                    foreach (var extUs in extUserWithWorking)
                    {
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId);
                        var list = new List<ProtocolSystemUserBE>();
                        foreach (var perm in ListUserExter)
                        {
                            var oProtocolSystemUserBEo = new ProtocolSystemUserBE();
                            oProtocolSystemUserBEo.i_SystemUserId = extUs.i_SystemUserId;
                            oProtocolSystemUserBEo.v_ProtocolId = protocolId;
                            oProtocolSystemUserBEo.i_ApplicationHierarchyId = perm.i_ApplicationHierarchyId;
                            list.Add(oProtocolSystemUserBEo);
                        }
                        bool resultUs = ProtocolDal.AddProtocolSystemUser(list, userId, nodeId);
                        if (!resultUs) return null;
                    }
                    return protocolId;
                }
                else
                {
                    return data.ProtocolId;
                }
                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static BoardProtocol GetAllProtocol(BoardProtocol data)
        {
            return ProtocolDal.GetAllProtocols(data);
        }

        public static MessageCustom SaveProtocols(ProtocolList data, int userId, int nodeId)
        {
            MessageCustom msg = new MessageCustom();

            try
            {
                using (var ts = new TransactionScope())
                {

                    ProtocolBE protocolEntity = new ProtocolBE();
                    protocolEntity.v_ProtocolId = data.v_ProtocolId;
                    protocolEntity.v_Name = data.v_Name;
                    protocolEntity.v_GroupOccupationId = data.v_Geso;
                    protocolEntity.v_CustomerOrganizationId = data.v_CustomerOrganizationId.Split('|')[0];
                    protocolEntity.v_CustomerLocationId = data.v_CustomerOrganizationId.Split('|')[1];
                    protocolEntity.i_MasterServiceTypeId = data.i_ServiceTypeId;
                    protocolEntity.v_EmployerOrganizationId = data.v_IntermediaryOrganization.Split('|')[0];
                    protocolEntity.v_EmployerLocationId = data.v_IntermediaryOrganization.Split('|')[1];
                    protocolEntity.i_MasterServiceId = data.i_MasterServiceId;
                    protocolEntity.v_WorkingOrganizationId = data.v_WorkingOrganizationId.Split('|')[0];
                    protocolEntity.v_WorkingLocationId = data.v_WorkingOrganizationId.Split('|')[1];
                    protocolEntity.v_CostCenter = data.v_CostCenter;
                    protocolEntity.i_EsoTypeId = data.i_EsoTypeId;
                    protocolEntity.i_IsActive = data.i_IsActive;
                    protocolEntity.i_HasVigency = (int)SiNo.No;
                    protocolEntity.r_PriceFactor = data.r_PriceFactor == null ? 0 : data.r_PriceFactor;
                    protocolEntity.r_MedicineDiscount = data.r_MedicineDiscount == null ? 0 : data.r_MedicineDiscount;
                    protocolEntity.r_HospitalBedPrice = data.r_HospitalBedPrice == null ? 0 : data.r_HospitalBedPrice;
                    protocolEntity.r_ClinicDiscount = data.r_ClinicDiscount == null ? 0 : data.r_ClinicDiscount;
                    protocolEntity.r_DiscountExam = data.r_DiscountExam == null ? 0 : data.r_DiscountExam;

                    List<ProtocolComponentDto> ListProtocolComponentCreate = new List<ProtocolComponentDto>();
                    List<ProtocolComponentDto> ListProtocolComponentUpdate = new List<ProtocolComponentDto>();

                    if (data.ListComponents != null)
                    {
                        foreach (var obj in data.ListComponents)
                        {
                            ProtocolComponentDto objProtComp = new ProtocolComponentDto();
                            objProtComp.v_ProtocolComponentId = obj.v_ProtocolComponentId;
                            objProtComp.v_ComponentId = obj.v_ComponentId;
                            objProtComp.r_Price = obj.r_Price;
                            objProtComp.i_OperatorId = obj.i_OperadorId;
                            objProtComp.i_Age = obj.i_Edad;
                            objProtComp.i_GenderId = obj.i_GenderId;
                            objProtComp.i_GrupoEtarioId = obj.i_GrupoEtario;
                            objProtComp.i_IsConditionalId = obj.i_IsConditional;
                            objProtComp.i_IsConditionalIMC = obj.i_IsIMC;
                            objProtComp.r_Imc = obj.r_ValueIMC;
                            objProtComp.i_IsAdditional = obj.i_IsAditional;
                            objProtComp.v_ProtocolComponentId = obj.v_ProtocolComponentId;
                            if (obj.RecordStatus == (int)RecordStatus.Agregado && obj.RecordType == (int)RecordType.Temporal || obj.v_ProtocolComponentId == null)
                            {
                                ListProtocolComponentCreate.Add(objProtComp);
                            }
                            else if (obj.RecordStatus == (int)RecordStatus.Editado && obj.RecordType == (int)RecordType.NoTemporal || obj.v_ProtocolComponentId != null)
                            {
                                ListProtocolComponentUpdate.Add(objProtComp);
                            }


                        }
                    }

                    
                    string protocolId = null;
                    if (data.v_ProtocolId == null)
                    {
                        protocolId = ProtocolDal.AddProtocol(protocolEntity, ListProtocolComponentCreate, nodeId, userId);
                        bool resultProtComp = ProtocolComponentDal.UpdateProtocolComponent(ListProtocolComponentUpdate, userId);
                        if (!resultProtComp) return null;
                    }
                    else
                    {
                        protocolId = ProtocolDal.UpdateProtocol(protocolEntity, ListProtocolComponentCreate, ListProtocolComponentUpdate, userId, nodeId);
                    }
                    if (protocolId == null)
                    {
                        msg.Error = true;
                        msg.Status = (int)HttpStatusCode.Conflict;
                        msg.Message = "Sucedió un error al crear el protocolo, por favor vuelva intentar.";
                        throw new Exception("");
                    }
                    else
                    {
                        msg.Error = false;
                        msg.Id = protocolId;
                        msg.Status = (int)HttpStatusCode.Created;
                        msg.Message = "El protocolo se creó correctamente.";
                    }
                    ts.Complete();
                }
                return msg;
            }
            catch (Exception ex)
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.Conflict;
                msg.Message = "Sucedió un error al crear el protocolo, por favor vuelva intentar.";
                return msg;
            }  
            
        }

        public static MessageCustom VerifyExistsProtocol(string protocolName)
        {
            return ProtocolDal.VerifyExistsProtocol(protocolName);
        }

        public static MessageCustom DeletedProtocolComponent(string protocolComponentId, int userId)
        {
            MessageCustom msg = new MessageCustom();
            bool result = ProtocolComponentDal.DeletedProtocolComponent(protocolComponentId, userId);
            if (result)
            {
                msg.Error = false;
                msg.Status = (int)HttpStatusCode.OK;
                msg.Message = "Se eliminó correctamente.";
            }
            else
            {
                msg.Error = true;
                msg.Status = (int)HttpStatusCode.OK;
                msg.Message = "Sucedió un error, por favor vuelva intentar.";
            }
            return msg;
        }

        public static List<OrganizationCustom> GetEmpresaByProtocoloId(string pstrProtocolId)
        {
            return ProtocolDal.GetEmpresaByProtocoloId(pstrProtocolId);
        }

        public static ProtocolList GetProtocolById(string pstrProtocolId)
        {
            return ProtocolDal.GetProtocolById(pstrProtocolId);
        }
    }
}
