using SigesoftWeb.Models.Protocol;
using SigesoftWeb.Models.ProtocolComponent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.ServiceOrder
{
    public class BoardServiceOrder
    {
        public string ProtocolId { get; set; }
        public string FechaEmision { get; set; }
        public ServiceOrderCustom EntityserviceOrder { get; set; }
        public List<ServiceOrderDetailCustom> ListEntityServiceOrder { get; set; }

    }

    public class ServiceOrderCustom
    {
        public string v_ServiceOrderId { get; set; }
        public string v_LocationId { get; set; }
        public string v_OrganizationId { get; set; }
        public string v_OrganizationName { get; set; }
        public string v_IdentificationNumber { get; set; }
        public string v_CustomServiceOrderId { get; set; }
        public string v_Description { get; set; }
        public string v_Comentary { get; set; }
        public int? i_NumberOfWorker { get; set; }
        public float? r_TotalCost { get; set; }
        public DateTime? d_DeliveryDate { get; set; }
        public int? i_ServiceOrderStatusId { get; set; }
        public int? i_LineaCreditoId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_MostrarPrecio { get; set; }
        public int? i_EsProtocoloEspecial { get; set; }
        public string v_ComentaryUpdate { get; set; }
    }

    public class ServiceOrderDetailCustom
    {
        public string v_ServiceOrderDetailId { get; set; }

        public string v_ServiceOrderId { get; set; }
        public string v_ProtocolId { get; set; }
        public string v_ProtocolTypeName { get; set; }
        public string v_GesoName { get; set; }
        public string v_GroupOccupationId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public float? r_ProtocolPrice { get; set; }
        public int? i_NumberOfWorkerProtocol { get; set; }
        public float? r_Total { get; set; }
        public string v_ComentaryUpdate { get; set; }
        public string v_ProtocolName { get; set; }
        public int? i_ProtocolType { get; set; }
        public List<ComponentCustom> ProtocolComponents { get; set; }
    }
}