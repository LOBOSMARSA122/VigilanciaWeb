using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Utils
{
    public class Enums
    {
        public enum ActionType
        {
            Create = 1,
            Edit = 2
        }
        public enum DataHierarchy
        {
            DocType = 106,
            CategoryProd = 103,
            MeasurementUnit = 150,
            Sector = 104,
            NivelEstudio = 108,
            CondicionPago = 23,
            Moneda = 18,
            TipoPago = 46,
        }

        public enum SystemParameter
        {
            OrgType = 103,
            TypeMovement = 109,
            Gender = 100,         
            MotiveMovement = 110,
            AuditType = 127,
            EstateEso = 125,
            NotificationType = 347,
            StateNotification = 348,
            TypeService = 119,
            MasterSerive = -1,
            Modalidad = 121,
            LineStatus = 120,
            Vip = 111,
            CalendarStatus = 122,
            EstadoCivil = 101,
            GrupoSanguineo = 154,
            FactorSanguineo = 155,
            DistProvDep = 113,
            TipoSeguro = 188,
            ResideLugar = 111,
            AltitudLabor = 208,
            LugarLabor = 204,
            TipoEso = 118,
            Parentesco = 207
        }

        public enum RolUser
        {
            Administrator = 1
        }


        public enum UserId
        {
            Sa = 11
        }

        public enum StateNotification
        {
            Sent = 1,
            Pending = 2
        }
    }
}