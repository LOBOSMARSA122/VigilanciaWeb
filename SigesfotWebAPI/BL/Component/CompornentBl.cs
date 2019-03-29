using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Component;
using DAL.Component;

namespace BL.Component
{
   public class ComponentBl
    {
        public List<AdditionalExams> ListOfAdditionalExams()
        {
            return new ComponentDal().ListOfAdditionalExams();
        }
    }
}
