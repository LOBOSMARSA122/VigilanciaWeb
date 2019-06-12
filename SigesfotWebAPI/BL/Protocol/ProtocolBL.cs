using BE.Protocol;
using BE.Service;
using DAL.Organizarion;
using DAL.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Protocol
{
    public class ProtocolBL
    {
        public ProtocolCustom GetDataProtocol(string protocolId)
        {
            ProtocolDal protocolDal = new ProtocolDal();
            return protocolDal.GetDatosProtocolo(protocolId);
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
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId.Value);
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
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId.Value);
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
                        var ListUserExter = ProtocolDal.GetProtocolSystemUser(extUs.i_SystemUserId.Value);
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
    }
}
