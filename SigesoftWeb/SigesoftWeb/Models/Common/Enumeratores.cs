using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Common
{
    public class Enumeratores
    {
        public enum TypeSchedule
        {
            Agendado = 1,
            AgendadoIniciado = 2
        }
        public enum ServiceComponentStatus
        {
            PorIniciar = 1,
            Iniciado = 2,
            Evaluado = 3,
            Auditado = 4,
            NoRealizado = 5,
            PorAprobacion = 6,
            Especialista = 7
        }
        public enum SaveEso
        {
            Allowed = 1,
            Denied = 2
        }

        public enum MovementType
        {
            Ingreso = 1,
            Egreso = 2,
        }

        public enum TypeNotification
        {
            AlertaMedica = 1,
            CitaMedica = 2,
            CampaniaSalud = 2
        }

        public enum ProcessType
        {
            LOCAL = 1,
            REMOTO = 2
        }
    }
}