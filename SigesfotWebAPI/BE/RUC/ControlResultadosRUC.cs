using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.RUC
{
    public class RootObject
    {
        public bool success { get; set; }
        public string mensaje { get; set; }
        public ControlResultadosRUC result { get; set; }
    }
    public class ControlResultadosRUC
    {
        public string ruc { get; set; }
        public string razon_social { get; set; }
        public string ciiu { get; set; }
        public string fecha_actividad { get; set; }
        public string contribuyente_condicion { get; set; }
        public string contribuyente_tipo { get; set; }
        public string contribuyente_estado { get; set; }
        public string nombre_comercial { get; set; }
        public string fecha_inscripcion { get; set; }
        public string domicilio_fiscal { get; set; }
        public string sistema_emision { get; set; }
        public string sistema_contabilidad { get; set; }
        public string actividad_exterior { get; set; }
        public string emision_electronica { get; set; }
        public string fecha_inscripcion_ple { get; set; }
        public string Oficio { get; set; }
        public string fecha_baja { get; set; }
    }
}
