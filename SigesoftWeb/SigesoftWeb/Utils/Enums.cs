using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Utils
{
    public class Enums
    {
        public enum DataHierarchy
        {
            DocType = 106,
            CategoryProd = 103,
            MeasurementUnit = 150,
            Sector = 104,

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