using System;
using System.Collections.Generic;
using BE.Subscription;
using BE.Worker;
using DAL;
using DAL.Subscription;

namespace BL.WorkerPwa
{
    public class WorkerPwaBl
    {
        public BE.Worker.WorkerPwa WorkerInformationPwa(string personId)
        {
            return new WorkersDal().WorkerInformationPwa(personId);
        }

        public bool UpdateWorker(BE.Worker.WorkerPwa oWorkerPwa)
        {
            return new WorkersDal().UpdateWorker(oWorkerPwa);
        }
        
    }
}
