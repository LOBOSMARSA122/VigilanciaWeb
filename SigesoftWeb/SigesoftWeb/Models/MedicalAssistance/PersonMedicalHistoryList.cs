using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.MedicalAssistance
{
    public class PersonMedicalHistoryList
    {
        public string v_PersonId { get; set; }
        public string v_DiseasesName { get; set; } //ok  Descripcion
        public string v_AntecedentTypeName { get; set; } //ok Tipo de atencion
        public string v_DateOrGroup { get; set; } //ok fecha/grupo
    }
}