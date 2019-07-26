using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Embarazo
{
    public class EmbarazoCustom
    {
        public string EmbarazoId { get; set; }
        public string PersonId { get; set; }
        public string Anio { get; set; }
        public string Cpn { get; set; }
        public string Complicacion { get; set; }
        public string Parto { get; set; }
        public string PesoRn { get; set; }
        public string Puerpio { get; set; }
        public int? IsDeleted { get; set; }
        public int? InsertUserId { get; set; }
        public DateTime? InsertDate { get; set; }
        public int? UpdateUserId { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string ObservacionesGestacion { get; set; }
        public string ComentaryUpdate { get; set; }
    }
}
