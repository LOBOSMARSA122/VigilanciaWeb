using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class Constants
    {
        public const string OK_RESULT = "La operación se realizó correctamente.";
        public const string BAD_REQUEST = "Sucedió un error al procesar la información, por favor vuelva a intentar.";
        public const string RucWortec = "20505310072";
        public const string COLESTEROL_TOTAL_Colesterol_Total_Id = "N009-MF000001086";
        public const string PERFIL_LIPIDICO_Colesterol_Total_Id = "N009-MF000001904";

        public const string GLUCOSA_Glucosa_Id = "N009-MF000000261";

        public const string HEMOGLOBINA_Hemoglobina_Id = "N009-MF000000265";
        public const string HEMOGRAMA_Hemoglobina_Id = "N009-MF000001874";

        public const string FUNCIONES_VITALES_Presion_Sistolica_Id= "N002-MF000000001";
        public const string FUNCIONES_VITALES_Presion_Distolica_Id = "N002-MF000000002";

        public const string ANTROPOMETRIA_Peso_Id = "N002-MF000000008";
        public const string ANTROPOMETRIA_Imc_Id = "N002-MF000000009";

        public const string ESPIROMETRIA_Cvf_Id = "N002-MF000000286";

        public const string PROTOCOL_VIGILANCIA = "N007-PR000000690";

        public const string TITLE_DEFAULT = "Notificación Recibida";
        public const string BODY_ALERT_MEDICAL = "Usted, ha sido enviado al plan: ";

        public const string SERVICE_STARTED = "Servicio Iniciado";
        public const string SERVICE_PENDING_EXAMS = "Examenes Pendientes";
        public const string SERVICE_PENDING_STATUS = "Servicio Pendiente de Estado";
        public const string SERVICE_FINISHED = "Servicio Finalizado";
    }
}
