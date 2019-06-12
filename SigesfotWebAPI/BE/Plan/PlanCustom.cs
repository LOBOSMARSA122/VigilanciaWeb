using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Plan
{
    public class PlanCustom
    {
        public int PlanId { get; set; }
        public string OrganizationSeguroId { get; set; }
        public string ProtocoloId { get; set; }
        public string IdUnidadProductiva { get; set; }
        public int EsDeducible { get; set; }
        public int EsCoaseguro { get; set; }
        public decimal Importe { get; set; }
    }
}
