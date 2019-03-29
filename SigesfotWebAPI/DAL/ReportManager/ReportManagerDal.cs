using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.ReportManager;

namespace DAL.ReportManager
{
    public class ReportManagerDal
    {
        DatabaseContext _Ctx = new DatabaseContext();
       
        public List<OrdenReportesBe> GetOrdenReportes(string organizationId)
        {
            var query = from A in _Ctx.OrdenReporte
                        where A.v_OrganizationId == organizationId
                        select new OrdenReportesBe
                        {
                            OrdenReporteId = A.v_OrdenReporteId,
                            ComponenteId = A.v_ComponenteId,
                            NombreReporte = A.v_NombreReporte,
                            Orden = A.i_Orden.Value,
                            NombreCrystal = A.v_NombreCrystal,
                            NombreCrystalId = A.i_NombreCrystalId
                        };

            var objData = query.ToList();
            return objData;
        }
        
        public List<ComponentsByServiceBe> GetComponentsByServiceId(string pstrServiceId)
        {
            var isDeleted = 0;
            var components = (from aaa in _Ctx.ServiceComponent
                              join bbb in _Ctx.Component on aaa.v_ComponentId equals bbb.v_ComponentId
                           
                              join fff in _Ctx.SystemParameter on new { a = bbb.i_CategoryId, b = 116 } // CATEGORIA DEL EXAMEN
                                  equals new { a = fff.i_ParameterId, b = fff.i_GroupId } into J5_join
                              from fff in J5_join.DefaultIfEmpty()

                              where (aaa.v_ServiceId == pstrServiceId) &&
                                    (bbb.i_ComponentTypeId == (int?)Enumeratores.ComponentType.Examen) &&
                                    (aaa.i_IsDeleted == isDeleted) &&
                                    (aaa.i_IsRequiredId == (int?)Enumeratores.SiNo.Si)
                              orderby bbb.i_CategoryId, bbb.v_Name

                              select new ComponentsByServiceBe
                              {
                                  ComponentId = bbb.v_ComponentId,
                                  ComponentName = bbb.v_Name,
                                  ServiceComponentId = aaa.v_ServiceComponentId,
                                  CategoryId = bbb.i_CategoryId,
                                  CategoryName = fff.v_Value1

                              }).ToList();

            return components;
        }

        public string GetEmpresaId(string serviceId)
        {
            var query = (from A in _Ctx.Service
                        join B in _Ctx.Protocol on A.v_ProtocolId equals B.v_ProtocolId
                        where A.v_ServiceId == serviceId
                        select B).FirstOrDefault();

            return query.v_CustomerOrganizationId;
        }
    }
}
