using BE.Z_SAMBHSCUSTOM.Clientes;
using DAL.ClientesSAMBHS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.z_ClientesSAMBHS
{
    public class ClientesBL
    {
        public BoardCliente GetClients(BoardCliente  data)
        {
            return new ClientesDal().GetClients(data);
        }
    }
}
