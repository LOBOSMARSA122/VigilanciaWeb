using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using static BE.Common.Enumeratores;

namespace DAL.Common
{
   public class PersonDal
    {
        private DatabaseContext ctx = new DatabaseContext();

        public PersonDto GetPersonDto(string personId)
        {
            var person = (from a in ctx.Person where a.v_PersonId == personId select a).FirstOrDefault();
            return person;

        }
    }
}
