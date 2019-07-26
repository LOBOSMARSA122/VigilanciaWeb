using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.Antecedentes
{
    public class BoardEsoAntecedentes
    {
        public List<EsoAntecedentesPadre> AntecedenteActual { get; set; }
        public List<EsoAntecedentesPadre> AntecedenteAnterior { get; set; }
    }
    public class EsoAntecedentesPadre
    {
        public int GrupoId { get; set; }
        public int ParametroId { get; set; }
        public string Nombre { get; set; }
        public List<EsoAntecedentesHijo> Hijos { get; set; }
    }

    public class EsoAntecedentesHijo
    {
        public string Nombre { get; set; }
        public bool SI { get; set; }
        public bool NO { get; set; }
        public int GrupoId { get; set; }
        public int ParametroId { get; set; }
    }
}