using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL.Service;

namespace BL.Service
{
    public class ServiceBl
    {
        public string DarDeBaja(string personId)
        {
            return new ServiceDal().DarDeBaja(personId);
        }
    }
}
