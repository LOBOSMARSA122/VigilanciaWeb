using BE.MedicalAssistance;
using BE.Worker;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BE.Plan;
using DAL.Plan;
using DAL.Vigilancia;
using static BE.Common.Enumeratores;

namespace BL.MedicalAssistance
{
   public class FilterWorkersBl
    {
        private List<ServiceWorkerBE> Workers = new List<ServiceWorkerBE>();

        private List<PlanDiseasesCustom> _filterDiseasesServices = new List<PlanDiseasesCustom>();

        private delegate void WorkerProcessor(ServiceWorkerBE serviceWorker);

        public BoardPatient FilterWorkers(BoardPatient data)
        {
            var workersDa = new WorkersDal();
                Workers = workersDa.WorkesWhitServices(out int totalRecords, data);
                if (data.PlanVigilanciaId != "-1")
                    _filterDiseasesServices = new PlanDal().ListPlanVigilanciaDiseases(data.PlanVigilanciaId);
                
                ProcessWorkers(ActiveWorker);
                ProcessWorkers(ResultEmoToReview);
                ProcessWorkers(ResultEmoToReviewCounter);
                ProcessWorkers(ResultControlInProgress);

                if (data.Workerstatus != -1)
                    Workers = FilterByState(Workers, data.Workerstatus);

                data.TotalRecords = totalRecords;
                data.List = TransformData(Workers);

                return data;
            
        }

        private void ProcessWorkers(WorkerProcessor process)
        {
            foreach (var worker in Workers)
                process(worker);
        }

        private List<ServiceWorkerBE> FilterByState(List<ServiceWorkerBE> oWorkesWhitServices, int filterState)
        {
            var active = filterState == (int)WorkerActive.Disabled ? false : true;
            return oWorkesWhitServices.FindAll(p => p.Active == active).ToList();
        }

        private void ResultControlInProgress(ServiceWorkerBE Worker)
        {
            Worker.ControlInProgress =  new VigilanciaDal().ControlInProgress(Worker.PatientId);
        }

        private List<Patients> TransformData(List<ServiceWorkerBE> WorkesWhitServices)
        {
            var list = new List<Patients>();
            foreach (var worker in WorkesWhitServices)
            {
                var oPatients = new Patients();
                oPatients.PatientId = worker.PatientId;
                oPatients.PatientFullName = worker.PatientFullName;
                oPatients.Gender = worker.Gender;
                oPatients.Age = worker.Age;
                oPatients.Occupation = worker.Occupation;
                oPatients.DocumentType = worker.DocumentType;
                oPatients.DocumentNumber = worker.DocumentNumber;
                oPatients.Active = worker.Active;
                oPatients.EmoToReview = worker.EmoToReview;
                oPatients.EmoToReviewCounter = worker.EmoToReviewCounter;
                oPatients.ControlInProgress = worker.ControlInProgress;
                oPatients.PlanVigilancia = worker.PlanVigilancia;
                oPatients.VigilanciaId = worker.VigilanciaId;
                list.Add(oPatients);
            }
            return list;
        }

        private void ActiveWorker(ServiceWorkerBE Worker)
        {
            var data = Worker.Services.FindAll(p =>
                p.StatusVigilanciaId == null || p.StatusVigilanciaId == (int)WorkerActive.Enabled);
            Worker.Active = data.Count > 0;

        }

        private void ResultEmoToReview(ServiceWorkerBE Worker)
        {
            var data = Worker.Services.FindAll(p => p.IsRevisedHistoryId == null);
            Worker.EmoToReview = data.Count > 0;
          
        }

        private void ResultEmoToReviewCounter(ServiceWorkerBE Worker)
        {
            var data = Worker.Services.FindAll(p => p.IsRevisedHistoryId == null);
            Worker.EmoToReviewCounter = data.Count;
          
        }

        private void MatchPlanVigilancia(ServiceWorkerBE worker)
        {
            if(worker.Services.Count > 0)return;

            var services = worker.Services.OrderByDescending(p => p.ServiceDate).ToList();
            var lastService = services[0];
            var diseases = lastService.ListDiseasesService;

            var result = false;
            foreach (var item in _filterDiseasesServices)
            {
                if (diseases.Find(p => p.DiseasesId == item.DiseasesId) != null)
                {
                    result = true;
                }
            }




        }
    }
}
