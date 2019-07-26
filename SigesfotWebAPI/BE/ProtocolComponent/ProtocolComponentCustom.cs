using BE.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.ProtocolComponent
{
    public class BoardProtocolComponent
    {
        public List<ProtocolComponentCustom> ListProtocolComponents { get; set; }
        public List<SystemUserCustom> ListUsers { get; set; }
    }


    public class ProtocolComponentCustom
    {
        public decimal? r_Imc { get; set; }
        public int? i_OperatorId { get; set; }
        public string ProtocolComponentId { get; set; }
        public string Genero { get; set; }
        public string CategoryName { get; set; }
        public string v_IsCondicional { get; set; }
        public string ComponentTypeName { get; set; }
        public string Operador { get; set; }
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
        public decimal? Imc { get; set; }
        public int? IsAdditional { get; set; }
        public int? ComponentTypeId { get; set; }
        public int? UIIsVisibleId { get; set; }
        public int? UIIndex { get; set; }
        public string IdUnidadProductiva { get; set; }
        public string Porcentajes { get; set; }
    }
}
