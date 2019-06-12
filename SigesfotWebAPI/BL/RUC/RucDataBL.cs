using BE.RUC;
using DAL.RUC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.RUC
{
    public class RucDataBL
    {
        public RootObject GetDataContribuyente(string numDoc)
        {
            return new RucDataDal().GetDataContribuyente(numDoc);
        }
    }
}
