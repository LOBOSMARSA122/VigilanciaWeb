using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.MedicalAssistance
{
    public class TopDiagnostic
    {
        //public string DiagnosticId { get; set; }
        //public string Diagnostic { get; set; }
        //public int TotalDiagnostic { get; set; }
        public int count { get; set; }
        public string name { get; set; }
        public int y { get; set; }
    }

    public class Indicators
    {
        public string PersonId { get; set; }
        public List<DataPatient> DataPatient { get; set; }
        public List<Weight> Weights { get; set; }
        public List<BloodPressureSis> BloodPressureSis { get; set; }
        public List<BloodPressureDia> BloodPressureDia { get; set; }

        public List<Cholesterol> Cholesterols { get; set; }
        public List<Glucose> Glucoses { get; set; }
        public List<Haemoglobin> Haemoglobins { get; set; }
        public List<Espiro> Espiros { get; set; }

    }
    public class DataPatient
    {
        public string Name { get; set; }
        public string Puesto { get; set; }
        public string Empresa { get; set; }
    }
    public class Weight
    {
        public string Date { get; set; }
        public string y { get; set; }

    }

    public class BloodPressureSis
    {
        public string Date { get; set; }
        public string y { get; set; }

    }

    public class BloodPressureDia
    {
        public string Date { get; set; }
        public string y { get; set; }
    }

    public class Cholesterol
    {
        public string Date { get; set; }
        public string y { get; set; }
    }

    public class Glucose
    {
        public string Date { get; set; }
        public string y { get; set; }
    }

    public class Haemoglobin
    {
        public string Date { get; set; }
        public string y { get; set; }
    }
    public class Espiro
    {
        public string Date { get; set; }
        public string y { get; set; }

    }
    public class MonthlyControls
    {
        public string Date { get; set; }
        public List<Day> NroDays { get; set; }
        public List<ControlDay> DailyControls { get; set; }
        public List<ControlCompletedDay> DailyControlsCompleted { get; set; }
    }
    
    public class Day
    {
        public string NroDay { get; set; }
    }

    public class ControlDay
    {
        public string Date { get; set; }
        public string y { get; set; }
    }

    public class ControlCompletedDay
    {
        public string Date { get; set; }
        public string y { get; set; }
    }

}
