using BE.Component;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.AdditionalExam
{
    public class AdditionalExamDal
    {
        public List<AdditionalExamUpdate> GetAdditionalExamForUpdateByServiceId(string serviceId, int userId)
        {
            DatabaseContext dbcontext = new DatabaseContext();

            var list = (from ade in dbcontext.AdditionalExam
                        join com in dbcontext.Component on ade.v_ComponentId equals com.v_ComponentId
                        join per in dbcontext.Person on ade.v_PersonId equals per.v_PersonId
                        where ade.v_ServiceId == serviceId && ade.i_IsDeleted == 0 && ade.i_IsProcessed == 0 && ade.i_InsertUserId == userId
                        select new AdditionalExamUpdate
                        {
                            v_AdditionalExamId = ade.v_AdditionalExamId,
                            v_ComponentId = ade.v_ComponentId,
                            v_ServiceId = ade.v_ServiceId,
                            v_ComponentName = com.v_Name,
                            i_IsProcessed = ade.i_IsProcessed.Value,
                            v_PacientName = per.v_FirstLastName + " " + per.v_SecondLastName + ", " + per.v_FirstName,
                        }).ToList();

            return list;
        }

        public bool UpdateComponentAdditionalExam(List<string> NewComponentId, List<string> _AdditionalExamId, int userId)
        {
            try
            {
                DatabaseContext dbcontext = new DatabaseContext();
                for (int i = 0; i < NewComponentId.Count; i++)
                {
                    var componente = NewComponentId[i];
                    var idExamen = _AdditionalExamId[i];
                    var obj = (from ade in dbcontext.AdditionalExam
                               where ade.v_AdditionalExamId == idExamen
                               select ade).FirstOrDefault();

                    obj.v_ComponentId = componente;
                    obj.d_UpdateDate = DateTime.Now;
                    obj.i_UpdateUserId = userId;
                }
                

                dbcontext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public bool DeleteAdditionalExam(string _AdditionalExamId, int userId)
        {
            try
            {
                DatabaseContext dbcontext = new DatabaseContext();
                var obj = (from ade in dbcontext.AdditionalExam
                           where ade.v_AdditionalExamId == _AdditionalExamId
                           select ade).FirstOrDefault();

                obj.i_IsDeleted = (int)SiNo.Si;
                obj.d_UpdateDate = DateTime.Now;
                obj.i_UpdateUserId = userId;

                return dbcontext.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }




        public List<AdditionalExamCustom> GetAdditionalExamByServiceId(string serviceId)
        {
            DatabaseContext dbcontext = new DatabaseContext();

            var list = (from ade in dbcontext.AdditionalExam
                        where ade.v_ServiceId == serviceId && ade.i_IsDeleted == 0 && ade.i_IsProcessed == 0
                        select new AdditionalExamCustom
                        {
                            ComponentId = ade.v_ComponentId,
                            ServiceId = ade.v_ServiceId,
                            IsNewService = ade.i_IsNewService.Value
                        }).ToList();

            return list;
        }

        public bool UpdateAdditionalExamByComponentIdAndServiceId(List<AdditionalExamCreate> data, int userId)
        {
            try
            {
                DatabaseContext dbcontext = new DatabaseContext();
                foreach (var objData in data)
                {
                    
                    var obj = (from ade in dbcontext.AdditionalExam
                               where ade.v_ComponentId == objData.ComponentId && ade.v_ServiceId == objData.ServiceId && ade.i_IsDeleted == 0 && ade.i_IsProcessed == 0
                               select ade).FirstOrDefault();

                    obj.i_IsNewService = (int)SiNo.No;
                    obj.i_IsProcessed = (int)SiNo.Si;
                    obj.i_IsDeleted = (int)SiNo.No;
                    obj.d_UpdateDate = DateTime.Now;
                    obj.i_UpdateUserId = userId;

                }
                

                dbcontext.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

    }
}
