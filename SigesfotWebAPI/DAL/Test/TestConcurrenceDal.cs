using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Test;

namespace DAL.Test
{
    public class TestConcurrenceDal
    {
        public void AddTestConcurrence(TestConcurrenceDto oTestConcurrenceDto)
        {
            try
            {
                using (var ctx = new DatabaseContext())
                {
                    var TestConcurrenceId = new Common.Utils().GetPrimaryKey(1, 1000, "TT");

                    oTestConcurrenceDto.TestConcurrenceId = TestConcurrenceId;
                    oTestConcurrenceDto.Value = oTestConcurrenceDto.Value;

                    ctx.TestConcurrenc.Add(oTestConcurrenceDto);
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                AddTestConcurrence(oTestConcurrenceDto);
            }
            
            
        }
    }
}
