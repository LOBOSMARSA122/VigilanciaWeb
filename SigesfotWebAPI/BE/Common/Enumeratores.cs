using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
   public class Enumeratores
    {
        public enum TypeForm
        {
            Windows = 1,
            Web = 2
        }

        public enum SiNo
        {
            No = 0,
            Si = 1
        }

        public enum Parameters
        {
            MartialStatus= 101,
            OrgType = 103,
            TypeMovement = 109,
            Gender = 100
        }

        public enum DataHierarchy
        {
            TypeDoc = 106,
            CategoryProd = 103,
            MeasurementUnit = 150,
            LevelOf = 108
        }

        public enum MovementType
        {
            Ingreso = 1,
            Egreso = 2,
        }

        public enum RecordStatus
        {
            Grabado = 1,
            Agregado = 2,
            Editado = 3,
            Eliminado = 4
        }

        public enum RecordType
        {
            Temporal = 1,
            NoTemporal = 2
        }

        public enum masterService
        {
            Ocupational = 2,
            //Assistence = 10,
            Control = 21,
            ConsultaInformatica = 4,
            ConsultaMedica = 3,
            ConsultaNutricional = 6,
            ConsultaPsicológica = 7,
            ProcEnfermería = 8,
            AtxMedica = 9,
            AtxMedicaParticular = 10

        }

        public enum ServiceStatus
        {
            PorIniciar = 1,
            Iniciado = 2,
            Culminado = 3,
            Incompleto = 4,
            Cancelado = 5,
            EsperandoAptitud = 6
        }

        public enum AptitudeStatus
        {
            SinAptitud = 1,
            Apto = 2,
            NoApto = 3,
            Observado = 4,
            AptoRestriccion = 5,
            Asistencial = 6,
            Evaluado = 7
        }

        public enum FinalQualification
        {
            SinCalificar = 1,
            Definitivo = 2,
            Presuntivo = 3,
            Descartado = 4

        }

        public enum EsoType
        {
            PreOcupacional = 1,
            PeriodicoAnual = 2,
            Retiro = 3,
            Preventivo = 4,
            Reubicacion = 5,
            Chequeo = 6,
            Visita = 7
        }

        public enum SystemUserType
        {
            Internal = 1,
            External = 2
        }

        public enum WorkerActive
        {
            Enabled = 1,
            Disabled = 2
        }

        public enum LogEventType
        {
            AccessSystem = 1,
            Create = 2,
            Update = 3,
            Delete = 4,
            Export = 5,
            GenerateReport = 6,
            Proccess = 7
        }
        
        public enum Success
        {
            Failed = 0,
            Ok = 1
        }

        public enum ComponenteProcedencia
        {
            Interno = 1,
            Externo = 2
        }

        public enum QueueStatusId
        {
            Libre = 1,
            llamando = 2,
            ocupado = 3
        }

        public enum Flag_Call
        {
            NoseLlamo = 0,
            Sellamo = 1
        }

        public enum Operator2Values
        {
            X_esIgualque_A = 1,
            X_noesIgualque_A = 2,
            X_esMenorque_A = 3,
            X_esMenorIgualque_A = 4,
            X_esMayorque_A = 5,
            X_esMayorIgualque_A = 6,
            X_esMayorque_A_yMenorque_B = 7,
            X_esMayorque_A_yMenorIgualque_B = 8,
            X_esMayorIgualque_A_yMenorque_B = 9,
            X_esMayorIgualque_A_yMenorIgualque_B = 12,
        }

        public enum GrupoEtario
        {
            Ninio = 4,
            Adolecente = 2,
            Adulto = 1,
            AdultoMayor = 3
        }

        public enum GenderConditional
        {
            Masculino = 1,
            Femenino = 2,
            Ambos = 3
        }

        public enum CalendarStatus
        {
            Agendado = 1,
            Atendido = 2,
            Vencido = 3,
            Cancelado = 4,
            Ingreso = 5
        }

        public enum Modality
        {
            NuevoServicio = 1,
            ContinuacionServicio = 2,
        }

        public enum LineStatus
        {
            EnCircuito = 1,
            FueraCircuito = 2,
        }

        public enum ComponentType
        {
            Examen = 1,
            NoExamen = 2
        }

        public enum Typifying
        {
            Recomendaciones = 1,
            Restricciones = 2
        }

        public enum TypeSchedule
        {
            Agendado = 1,
            AgendadoIniciado = 2
        }

        public enum StateVigilancia
        {
            Iniciado = 1,
            Finalizado = 2
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
            Denied  = 2 
        }

        public enum TypeNotification
        {
            AlertaMedica = 1,
            CitaMedica = 2,
            CampaniaSalud = 2
        }

        public enum StateNotification
        {
            Sent = 1,
            Pending =2
        }

        public enum AutoManual
        {
            Automático = 1,
            Manual = 2
        }

        public enum PreQualification
        {
            SinPreCalificar = 1,
            Aceptado = 2,
            Rechazado = 3
        }

        public enum TipoDx
        {
            Enfermedad_Comun = 1,
            Enfermedad_Ocupacional = 2,
            Accidente_Común = 3,
            Accidente_Ocupacional = 4,
            Otros = 5,
            Normal = 6,
            SinDx = 7
        }
        public enum ProcessType
        {
            LOCAL = 1,
            REMOTO = 2
        }

    }
}
