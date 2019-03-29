using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Common;
using BE.Subscription;

namespace DAL.Subscription
{
    public class SubscriptionDal
    {
        public void SubscriptionWorker(int nodeId, string subs, string personId)
        {
            //var objEntitySource = (from a in ctx.Person
            //    where a.v_PersonId == personId
            //    select a).FirstOrDefault();

            //objEntitySource.v_Subs = subs;
            //ctx.SaveChanges();

            using (var dbContext = new DatabaseContext())
            {
                var objEntity = new SubscriptionDto();

                objEntity.v_PersonId = personId;
                objEntity.v_Subs = subs;
                objEntity.i_IsDeleted = (int)Enumeratores.SiNo.No;
                objEntity.d_InsertDate = DateTime.Now;

                //Autogeneramos el Pk de la tabla
                objEntity.v_SubscriptionId = new Common.Utils().GetPrimaryKey(nodeId, 361, "SP");

                dbContext.Subscription.Add(objEntity);
                dbContext.SaveChanges();
            }

        }

        public string UnSubscriptionWorker(string personId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var objEntitySource = (from a in dbContext.Subscription
                    where a.v_PersonId == personId
                    select a).ToList();

                foreach (var item in objEntitySource)
                {
                    item.i_IsDeleted = (int)Enumeratores.SiNo.Si;
                    item.d_UpdateDate = DateTime.Now;
                }
                dbContext.SaveChanges();
                return personId;
            }
        }

        public List<SubscriptionDto> GetSubscriptions(string personId)
        {
            using (var dbContext = new DatabaseContext())
            {
                var objEntitySource = (from a in dbContext.Subscription
                    where a.v_PersonId == personId && a.i_IsDeleted == (int)Enumeratores.SiNo.No
                    select a).ToList();

                return objEntitySource;
            }
            
        }

    }
}
