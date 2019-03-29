using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    public class ClientSession
    {
        public int CurrentExecutionNodeId
        {
            get { return int.Parse(_objData[0]); }
            set { _objData[0] = value.ToString(); }
        }

        public int CurrentOrganizationId
        {
            get { return int.Parse(_objData[1]); }
            set { _objData[1] = value.ToString(); }
        }

        public int SystemUserId
        {
            get { return int.Parse(_objData[2]); }
            set { _objData[2] = value.ToString(); }
        }

        public string CurrentExecutionNodeName
        {
            get { return _objData[3]; }
            set { _objData[3] = value; }
        }

        public string CurrentOrganizationName
        {
            get { return _objData[4]; }
            set { _objData[4] = value; }
        }

        public string UserName
        {
            get { return _objData[5]; }
            set { _objData[5] = value; }
        }

        public string PersonId
        {
            get { return _objData[6]; }
            set { _objData[6] = value; }
        }

        public string RoleName
        {
            get { return _objData[7]; }
            set { _objData[7] = value; }
        }

        public int? RoleId
        {
            get { return int.Parse(_objData[8] == null ? "0" : _objData[8]); }
            set { _objData[8] = value.ToString(); }
        }

        public int SystemUserTypeId
        {
            get { return int.Parse(_objData[9]); }
            set { _objData[9] = value.ToString(); }
        }

        public int SystemUserCopyId
        {
            get { return int.Parse(_objData[10]); }
            set { _objData[10] = value.ToString(); }
        }

        public int? RolVentaId
        {
            get { return int.Parse(_objData[11]); }
            set { _objData[11] = value.ToString(); }
        }

        public int? ProfesionId
        {
            get { return int.Parse(_objData[12]); }
            set { _objData[12] = value.ToString(); }
        }

        public byte[] LogoOwner
        {
            get { return new[] { byte.Parse(_objData[13]) }; }
            set { _objData[13] = value.ToString(); }
        }

        public string TelephoneOwner
        {
            get { return _objData[14]; }
            set { _objData[14] = value; }
        }

        public string RucOwner
        {
            get { return _objData[15]; }
            set { _objData[15] = value; }
        }

        public string AddressOwner
        {
            get { return _objData[16]; }
            set { _objData[16] = value; }
        }

        public string OrganizationOwner
        {
            get { return _objData[17]; }
            set { _objData[17] = value; }
        }

        public string SectorName
        {
            get { return _objData[18]; }
            set { _objData[18] = value; }
        }

        private List<string> _objData;

        public ClientSession()
        {
            _objData = new List<string>(19);

            for (int i = 0; i < 19; i++)
            {
                _objData.Add(null);
            }
        }

        public List<string> GetAsList()
        {
            return _objData;
        }

        public string[] GetAsArray()
        {
            return _objData.ToArray();
        }

    }
}
