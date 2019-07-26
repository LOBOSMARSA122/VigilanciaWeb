using SigesoftWeb.Models.Antecedentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SigesoftWeb.Models.AntecedentesGenerales
{
    public class BoardGenerales
    {
        public List<EsoAntecedentesPadre> EsoAntecedentesPadre { get; set; }
        public AdultoCustom DataAdultoCustom { get; set; }
        public AdolescenteCustom DataAdolescenteCustom { get; set; }
        public NinioCustom DataNinioCustom { get; set; }
        public AdultoMayorCustom DataAdultoMayorCustom { get; set; }
    }

    public class NinioCustom
    {
        public string v_NinioId { get; set; }
        public string v_PersonId { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_NombrePadre { get; set; }
        public string v_EdadPadre { get; set; }
        public string v_DniPadre { get; set; }
        public int? i_TipoAfiliacionPadre { get; set; }
        public string v_CodigoAfiliacionPadre { get; set; }
        public int? i_GradoInstruccionPadre { get; set; }
        public string v_OcupacionPadre { get; set; }
        public int? i_EstadoCivilIdPadre { get; set; }
        public string v_ReligionPadre { get; set; }
        public string v_NombreMadre { get; set; }
        public string v_EdadMadre { get; set; }
        public string v_DniMadre { get; set; }
        public int? i_TipoAfiliacionMadre { get; set; }
        public string v_CodigoAfiliacionMadre { get; set; }
        public int? i_GradoInstruccionMadre { get; set; }
        public string v_OcupacionMadre { get; set; }
        public int? i_EstadoCivilIdMadre1 { get; set; }
        public string v_ReligionMadre { get; set; }
        public string v_PatologiasGestacion { get; set; }
        public string v_nEmbarazos { get; set; }
        public string v_nAPN { get; set; }
        public string v_LugarAPN { get; set; }
        public string v_ComplicacionesParto { get; set; }
        public string v_Atencion { get; set; }
        public string v_EdadGestacion { get; set; }
        public string v_Peso { get; set; }
        public string v_Talla { get; set; }
        public string v_PerimetroCefalico { get; set; }
        public string v_PerimetroToracico { get; set; }
        public string v_EspecificacionesNac { get; set; }
        public string v_InicioAlimentacionComp { get; set; }
        public string v_AlergiasMedicamentos { get; set; }
        public string v_OtrosAntecedentes { get; set; }
        public string v_EspecificacionesAgua { get; set; }
        public string v_EspecificacionesDesague { get; set; }
        public string v_TiempoHospitalizacion { get; set; }
        public string v_QuienTuberculosis { get; set; }
        public int? i_QuienTuberculosis { get; set; }
        public string v_QuienAsma { get; set; }
        public int? i_QuienAsma { get; set; }
        public string v_QuienVIH { get; set; }
        public int? i_QuienVIH { get; set; }
        public string v_QuienDiabetes { get; set; }
        public int? i_QuienDiabetes { get; set; }
        public string v_QuienEpilepsia { get; set; }
        public int? i_QuienEpilepsia { get; set; }
        public string v_QuienAlergias { get; set; }
        public int? i_QuienAlergias { get; set; }
        public string v_QuienViolenciaFamiliar { get; set; }
        public int? i_QuienViolenciaFamiliar { get; set; }
        public string v_QuienAlcoholismo { get; set; }
        public int? i_QuienAlcoholismo { get; set; }
        public string v_QuienDrogadiccion { get; set; }
        public int? i_QuienDrogadiccion { get; set; }
        public string v_QuienHeptitisB { get; set; }
        public int? i_QuienHeptitisB { get; set; }

        public string v_LME { get; set; }
        public string v_Mixta { get; set; }
        public string v_Artificial { get; set; }

    }

    public class AdolescenteCustom
    {
        public string v_AdolescenteId { get; set; }
        public string v_PersonId { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_EdadInicioTrabajo { get; set; }
        public string v_TipoTrabajo { get; set; }
        public string v_NroHorasTv { get; set; }
        public string v_NroHorasJuegos { get; set; }
        public string v_MenarquiaEspermarquia { get; set; }
        public string v_ViveCon { get; set; }
        public string v_EdadInicioRS { get; set; }
        public string v_Observaciones { get; set; }
    }

    public class AdultoCustom
    {
        public string v_AdultoId { get; set; }
        public string v_PersonId { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_MedicamentoFrecuente { get; set; }
        public string v_ReaccionAlergica { get; set; }
        public string v_InicioRS { get; set; }
        public string v_NroPs { get; set; }
        public string v_FechaUR { get; set; }
        public string v_RC { get; set; }
        public string v_Parto { get; set; }
        public string v_Prematuro { get; set; }
        public string v_Aborto { get; set; }
        public string v_DescripcionAntecedentes { get; set; }
        public string v_OtrosAntecedentes { get; set; }
        //public string v_DescripciónAntecedentes { get; set; }
        public string v_FlujoVaginal { get; set; }
        public string v_ObservacionesEmbarazo { get; set; }
    }

    public class AdultoMayorCustom
    {
        public string v_AdultoMayorId { get; set; }
        public string v_PersonId { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
        public string v_MedicamentoFrecuente { get; set; }
        public string v_ReaccionAlergica { get; set; }
        public string v_InicioRS { get; set; }
        public string v_NroPs { get; set; }
        public string v_FechaUR { get; set; }
        public string v_RC { get; set; }
        public string v_Parto { get; set; }
        public string v_Prematuro { get; set; }
        public string v_Aborto { get; set; }

        public string v_DescripciónAntecedentes { get; set; }
        public string v_FlujoVaginal { get; set; }
        public string v_ObservacionesEmbarazo { get; set; }
    }
}