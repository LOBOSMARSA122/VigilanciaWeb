using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Service;
using DAL.Common;
using static BE.Common.Enumeratores;

namespace DAL.Service
{
   public class ServiceComponentDal
    {
        private static DatabaseContext ctx = new DatabaseContext();

        public void AddServiceComponentInBlock(List<ServiceComponentDto> list, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                foreach (var item in list)
                {
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");

                    item.v_ServiceComponentId = serviceComponentId;
                    item.i_IsDeleted = (int)Enumeratores.SiNo.No;
                    item.d_InsertDate = DateTime.UtcNow;
                    item.i_InsertUserId = systemUserId;

                    oCtx.ServiceComponent.Add(item);
                }
                oCtx.SaveChanges();
            }
            
        }

        public List<ServiceComponentTemp> AddServiceComponentInBlockTemp(List<ServiceComponentDto> list, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var outList = new List<ServiceComponentTemp>();
                foreach (var item in list)
                {
                    var serviceComponentId = new Common.Utils().GetPrimaryKey(nodeId, 24, "SC");

                    item.v_ServiceComponentId = serviceComponentId;
                    item.i_IsDeleted = (int)Enumeratores.SiNo.No;
                    item.d_InsertDate = DateTime.UtcNow;
                    item.i_InsertUserId = systemUserId;

                    oCtx.ServiceComponent.Add(item);

                    var oClassCustom = new ServiceComponentTemp
                    {
                        v_ComponentId = item.v_ComponentId,
                        v_ServiceComponentId = serviceComponentId
                    };

                    outList.Add(oClassCustom);
                }
                oCtx.SaveChanges();

                return outList;
            }
            
        }

        public List<ServiceComponentDto> GetServiceComponentDtos(string serviceId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var list = (from A in oCtx.ServiceComponent where A.v_ServiceId == serviceId && A.i_IsDeleted == (int)SiNo.No select A).ToList();

                return list;
            }
        }

        public ServiceComponentDto UpdateServiceComponentId(ServiceComponentDto oDataServiceComponent, int nodeId, int systemUserId)
        {
            using (var oCtx = new DatabaseContext())
            {
                var obj = (from a in oCtx.ServiceComponent where a.v_ServiceComponentId == oDataServiceComponent.v_ServiceComponentId select a).FirstOrDefault();
                var componentId = obj.v_ComponentId;
                var serviceId = obj.v_ServiceId;
                var categoryId = (from a in oCtx.Component where a.v_ComponentId == componentId select a).FirstOrDefault().i_CategoryId;

                var components = (from a in oCtx.Component where a.i_CategoryId == categoryId select a.v_ComponentId).ToList();
                
                var oEntidadFuerte =
                    (from A in oCtx.ServiceComponent
                        where A.v_ServiceId == serviceId  && components.Contains(A.v_ComponentId)
                        //where A.v_ServiceComponentId == oDataServiceComponent.v_ServiceComponentId
                        select A).ToList();

                foreach (var item in oEntidadFuerte)
                {
                    item.v_Comment = oDataServiceComponent.v_Comment;
                    item.i_ServiceComponentStatusId = oDataServiceComponent.i_ServiceComponentStatusId;
                    //item.i_ExternalInternalId = oDataServiceComponent.i_ExternalInternalId;
                    item.i_IsApprovedId = oDataServiceComponent.i_IsApprovedId;
                    item.i_ApprovedUpdateUserId = systemUserId;
                    item.i_UpdateUserId = systemUserId;
                    item.d_UpdateDate = DateTime.Now;
                }
                
                oCtx.SaveChanges();

                return oEntidadFuerte[0];
            }
        }

        public List<ServiceComponentBe> ServiceComponentByServiceId(string serviceId)
        {
            using (var contex = new DatabaseContext())
            {
                var query = (from a in contex.ServiceComponent
                    where a.v_ServiceId == serviceId
                    select new ServiceComponentBe
                    {
                        ServiceId = a.v_ServiceId,
                        ServiceComponentId = a.v_ServiceComponentId,
                        ServiceComponentStatusId = a.i_ServiceComponentStatusId.Value
                    }).ToList();

                return query;
            }
        }
    }
}
