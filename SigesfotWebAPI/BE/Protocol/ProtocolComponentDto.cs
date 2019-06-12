using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Protocol
{
    [Table("protocolcomponent")]
    public class ProtocolComponentDto
    {
        [Key]
        public string v_ProtocolComponentId { get; set; }

        public string v_ProtocolId { get; set; }
        public string v_ComponentId { get; set; }
        public float r_Price { get; set; }
        public int? i_OperatorId { get; set; }
        public int? i_Age { get; set; }
        public int? i_GenderId { get; set; }
        public int? i_GrupoEtarioId { get; set; }
        public int? i_IsConditionalId { get; set; }
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public int? i_IsConditionalIMC { get; set; }
        public float r_Imc { get; set; }
        public int? i_IsAdditional { get; set; }
    }

    public class ProtocolComponentCustom
    {
        public string ProtocolComponentId { get; set; }
        public string ComponentName { get; set; }
        public string ProtocolId { get; set; }
        public string ComponentId { get; set; }
        public float Price { get; set; }
        public int? OperatorId { get; set; }
        public int? Age { get; set; }
        public int? GenderId { get; set; }
        public int? GrupoEtarioId { get; set; }
        public int? IsConditionalId { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public int? IsConditionalIMC { get; set; }
        public float Imc { get; set; }
        public int? IsAdditional { get; set; }
        public int? ComponentTypeId { get; set; }
        public int? UIIsVisibleId { get; set; }
        public int? UIIndex { get; set; }
        public string IdUnidadProductiva { get; set; }
        public string Porcentajes { get; set; }
    }
}
