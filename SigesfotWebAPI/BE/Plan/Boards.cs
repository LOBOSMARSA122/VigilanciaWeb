using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Plan
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardPlanVigilancia : Boards
    {
        public string Name { get; set; }
        public string OrganizationId { get; set; }
        public List<PlanVigilanciaCustom> List { get; set; }
    }


    public class PlanVigilanciaCustom
    {
        public string PlanVigilanciaId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string OrganizationId { get; set; }
        public int NodeId { get; set; }
        public int SystemUserId { get; set; }
        public List<PlanDiseasesCustom> PlanDiseases { get; set; }
    }

    public class PlanDiseasesCustom
    {
        public string PlanVigilanciaDiseasesId { get; set; }
        public string PlanVigilanciaId { get; set; }
        public string DiseasesId { get; set; }
        public string DiseasesName { get; set; }
        public string Cie10 { get; set; }
        public int RecordStatus { get; set; }
        public int RecordType { get; set; }
    }

}
