using BE.Common;
using BE.Message;
using BE.Organization;
using BL.Common;
using DAL.Organizarion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Script.Serialization;

namespace BL.Organization
{
    public class OrganizationBL
    {

        public static List<string> SearchOrganizations(string value)
        {
            return new OrganizationDal().SearchOrganizations(value);
        }

        public static MessageCustom SaveOrganization(OrganizationBE data, int userId, int nodeId)
        {
            MessageCustom msg = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {

                    data.i_OrganizationTypeId = 1;
                    string result = OrganizationDal.SaveOrganization(data, userId, nodeId);

                    if (result == null)
                    {
                        msg.Error = true;
                        msg.Message = "Sucedió un error al guardar la empresa, por favor actualiza y vuelva a intentar";
                        msg.Status = (int)HttpStatusCode.Conflict;
                    }
                    else
                    {
                        msg.Error = false;
                        msg.Message = "Se guardó correctamente.";
                        msg.Status = (int)HttpStatusCode.Created;
                        msg.Id = result;
                    }

                    ts.Complete();

                    return msg;
                }
            }
            catch (Exception ex)
            {
                return msg;
            }
            
            
        }

    }
}
