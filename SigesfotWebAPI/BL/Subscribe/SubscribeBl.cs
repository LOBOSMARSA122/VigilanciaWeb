using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Subscription;
using DAL.Subscription;

namespace BL.Subscribe
{
    public class SubscribeBl
    {
        public void Subscribe(int nodeId, string subs, string personId)
        {
            new SubscriptionDal().SubscriptionWorker(nodeId, subs, personId);
        }

        public string UnSubscribe(string personId)
        {
           return new SubscriptionDal().UnSubscriptionWorker(personId);
        }

        public List<SubscriptionDto> GetSubscriptions(string personId)
        {
            return new SubscriptionDal().GetSubscriptions(personId);
        }
    }
}
