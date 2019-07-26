using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Common
{
    [Table("Ninio")]
    public class NinioBE
    {
        [Key]
        public string v_NinioId { get; set; }

        public string v_PersonId { get; set; }
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
        public int? i_IsDeleted { get; set; }
        public int? i_InsertUserId { get; set; }
        public DateTime? d_InsertDate { get; set; }
        public int? i_UpdateUserId { get; set; }
        public DateTime? d_UpdateDate { get; set; }
        public string v_NombreCuidador { get; set; }
        public string v_EdadCuidador { get; set; }
        public string v_DniCuidador { get; set; }
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
        public string v_LME { get; set; }
        public string v_Mixta { get; set; }
        public string v_Artificial { get; set; }
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
        public string v_ComentaryUpdate { get; set; }
    }
}
