using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Protocol;
using BE.ProtocolComponent;
using DAL.Common;
using static BE.Common.Enumeratores;

namespace DAL.Protocol
{
    public class ProtocolComponentDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public List<ProtocolComponentCustom> GetProtocolComponents(string protocolId)
        {
            var list = (from a in ctx.ProtocolComponent
                        join b in ctx.Component on a.v_ComponentId equals b.v_ComponentId
                        join sys in ctx.SystemParameter on new { a = b.i_CategoryId, b = 116 } equals new { a = sys.i_ParameterId, b = sys.i_GroupId } into sys_join
                        from sys in sys_join.DefaultIfEmpty()

                        join sys2 in ctx.SystemParameter on new { a = a.i_OperatorId.Value, b = 117 }
                                                               equals new { a = sys2.i_ParameterId, b = sys2.i_GroupId } into sys2_join
                        from sys2 in sys2_join.DefaultIfEmpty()

                        join sys3 in ctx.SystemParameter on new { a = a.i_GenderId.Value, b = 130 }  // Genero condicional
                                                           equals new { a = sys3.i_ParameterId, b = sys3.i_GroupId } into sys3_join
                        from sys3 in sys3_join.DefaultIfEmpty()

                        join sys4 in ctx.SystemParameter on new { a = b.i_ComponentTypeId.Value, b = 126 }  // Tipo componente
                                                                  equals new { a = sys4.i_ParameterId, b = sys4.i_GroupId } into sys4_join
                        from sys4 in sys4_join.DefaultIfEmpty()

                        where a.v_ProtocolId == protocolId && a.i_IsDeleted == (int)SiNo.No
                        select new ProtocolComponentCustom
                        {
                            ProtocolComponentId = a.v_ProtocolComponentId,
                            ComponentTypeId = b.i_ComponentTypeId,
                            UIIsVisibleId = b.i_UIIsVisibleId,
                            Operador = sys2.v_Value1,
                            i_OperatorId = sys2.i_ParameterId,
                            UIIndex = b.i_UIIndex,
                            IdUnidadProductiva = b.v_IdUnidadProductiva,
                            Price = a.r_Price,
                            OperatorId = a.i_OperatorId,
                            Age = a.i_Age,
                            GenderId = a.i_GenderId,
                            GrupoEtarioId = a.i_GrupoEtarioId,
                            IsConditionalId = a.i_IsConditionalId,
                            IsConditionalIMC = a.i_IsConditionalIMC,
                            r_Imc = a.r_Imc,
                            IsAdditional = a.i_IsAdditional,
                            ComponentId = b.v_ComponentId,
                            ComponentName = b.v_Name,
                            Porcentajes = sys.v_Field,
                            Genero = sys3.v_Value1,
                            CategoryName = sys.v_Value1,
                            v_IsCondicional = a.i_IsConditionalId == null ? "" : a.i_IsConditionalId == 1 ? "SI" : "NO",
                            ComponentTypeName = sys4.v_Value1
                        } ).ToList();
            
            return list;
        }

        public static bool AddProtocolComponent(List<ProtocolComponentDto> listProtComp, string protocolId, int userId, int nodeId)
        {
            try
            {
                DatabaseContext cnx = new DatabaseContext();
                foreach (var objProtComp in listProtComp)
                {
                    var newId = new Common.Utils().GetPrimaryKey(nodeId, 21, "PC");
                    objProtComp.v_ProtocolId = protocolId;
                    objProtComp.v_ProtocolComponentId = newId;
                    objProtComp.d_InsertDate = DateTime.Now;
                    objProtComp.i_IsDeleted = (int)SiNo.No;
                    objProtComp.i_InsertUserId = userId;

                    cnx.ProtocolComponent.Add(objProtComp);
                }
                return cnx.SaveChanges() > 0;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool UpdateProtocolComponent(List<ProtocolComponentDto> listProtComp, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                foreach (var obj in listProtComp)
                {
                    var objProtComp = ctx.ProtocolComponent.Where(x => x.v_ProtocolComponentId == obj.v_ProtocolComponentId).FirstOrDefault();

                    objProtComp.v_ComponentId = obj.v_ComponentId;
                    objProtComp.r_Price = obj.r_Price;
                    objProtComp.i_OperatorId = obj.i_OperatorId;
                    objProtComp.i_Age = obj.i_Age;
                    objProtComp.i_GenderId = obj.i_GenderId;
                    objProtComp.i_GrupoEtarioId = obj.i_GrupoEtarioId;
                    objProtComp.i_IsConditionalId = obj.i_IsConditionalId;
                    objProtComp.i_IsConditionalIMC = obj.i_IsConditionalIMC;
                    objProtComp.r_Imc = obj.r_Imc;
                    objProtComp.i_IsAdditional = obj.i_IsAdditional;

                    objProtComp.i_UpdateUserId = userId;
                    objProtComp.d_UpdateDate = DateTime.Now;

                    ctx.SaveChanges();
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static bool DeletedProtocolComponent(string protocolComponentId, int userId)
        {
            try
            {
                var objProtComp = ctx.ProtocolComponent.Where(x => x.v_ProtocolComponentId == protocolComponentId).FirstOrDefault();
                objProtComp.i_IsDeleted = (int)SiNo.Si;
                objProtComp.d_UpdateDate = DateTime.Now;
                objProtComp.i_UpdateUserId = userId;

                ctx.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
