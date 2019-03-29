using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BE.Test;
using DAL.Test;

namespace BL.Test
{
    public class TestBl
    {
        private DatabaseContext ctx = new DatabaseContext();

        public int Test()
        {
            try
            {
                int totalRecords = 0;
                for (int i = 0; i < 8000; i++)
                {
                    var preList = (from a in ctx.ServiceComponentFieldValues
                        select new 
                        {
                            PatientId = a.v_ServiceComponentFieldValuesId,

                        }).ToList();

                    totalRecords = preList.Count;
                }
                return totalRecords;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public int Test2()
        {
            try
            {
                int totalRecords = 0;
                for (int i = 0; i < 5; i++)
                {
                    var preList = (from a in ctx.Diseases
                        select new 
                        {
                            PatientId = a.v_DiseasesId,

                        }).ToList();

                    totalRecords = preList.Count;
                }
                return totalRecords;

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<TestObj> GetTest3(int nroPagina)
        {
            int totalRecords = 0;
            
                var obj = (from a in ctx.Pacient
                            join b in ctx.Person on a.v_PersonId equals  b.v_PersonId 
                            where  a.i_IsDeleted == 0 && b.b_PersonImage != null
                    select new TestObj
                    {
                        estudiante = b.v_FirstName + " " + b.v_FirstLastName + " " + b.v_SecondLastName,
                        //foto = b.b_PersonImage


                    }).ToList();

               var list = obj.Skip(nroPagina).Take(10).ToList();
            return list;
        }

        public class TestObj
        {
            public string estudiante { get; set; }
            public byte[] foto{ get; set; }
        }

        public void AddTestConcurrence(int count)
        {
            for (int i = 0; i < count; i++)
            {
                var task = new Task(() => xxx());
                task.Start();
            }
        }

        public void xxx()
        {
            var oTestConcurrenceDto = new TestConcurrenceDto();
            oTestConcurrenceDto.Value = Guid.NewGuid();
            new TestConcurrenceDal().AddTestConcurrence(oTestConcurrenceDto);
        }
    }
}
