using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Diagnostic
{
    public class DiagnosticCustom
    {
        public string ServiceId { get; set; }
        public string DiagnosticRepositoryId { get; set; }
        public string DiseaseId { get; set; }
        public string ComponentId { get; set; }
        public string DiseaseName { get; set; }
        public string AutoManual { get; set; }
        public int? PreQualificationId { get; set; }
        public int? FinalQualificationId { get; set; }
        public int? DiagnosticTypeId { get; set; }
        public string PreQualificationName { get; set; }
        public string FinalQualificationName { get; set; }
        public string DiagnosticTypeName { get; set; }
        public int RecordStatus { get; set; }
        public string ComponentFieldsId { get; set; }
        public int? AutoManualId { get; set; }
        public int RecordType { get; set; }
        public DateTime? ExpirationDateDiagnostic { get; set; }
        public int? DiagnosticSourceId { get; set; }
        public int? ShapeAccidentId { get; set; }
        public int? BodyPartId { get; set; }
        public int? ClassificationOfWorkAccidentId { get; set; }
        public int? RiskFactorId { get; set; }
        public int? ClassificationOfWorkdiseaseId { get; set; }
        public int? IsSentToAntecedent { get; set; }
        public string Cie10 { get; set; }
        public List<Recommendation> Recommendations { get; set; }
        public List<Restriction> Restrictions { get; set; }

    }

    public class Recommendation
    {
        public string DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public string RecommendationId { get; set; }
        public string MasterRecommendationId { get; set; }
        public string RecommendationName { get; set; }
        public int RecordStatus { get; set; }
        public int RecordType { get; set; }
    }

    public class Restriction
    {
        public string DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public string RestrictionId { get; set; }
        public string MasterRestrictionId { get; set; }
        public string RestrictionName { get; set; }
        public int RecordStatus { get; set; }
        public int RecordType { get; set; }
    }
}