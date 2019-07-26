using BE.Antecedentes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;
using BE.Ninio;
using BE.Common;
using BE.Embarazo;

namespace DAL.Antecedentes
{
    public class EsoAntecedentesDal
    {

        public List<EsoAntecedentesPadre> ObtenerEsoAntecedentesPorGrupoId(int GrupoId, int GrupoEtario, string PersonaId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                int isNotDeleted = (int)SiNo.No;

                var data = (from a in dbContext.SystemParameter
                            where a.i_IsDeleted == isNotDeleted &&
                            a.i_GroupId == GrupoId
                            select new EsoAntecedentesPadre
                            {
                                GrupoId = a.i_GroupId,
                                ParametroId = a.i_ParameterId,
                                Nombre = a.v_Value1
                            }).ToList();


                foreach (var P in data)
                {
                    int grupoHijo = int.Parse(P.GrupoId.ToString() + P.ParametroId.ToString());
                    P.Hijos = (from a in dbContext.SystemParameter
                               join b in dbContext.AntecedentesAsistencial on new { a = a.i_ParameterId, b = GrupoEtario, c = PersonaId, d = P.ParametroId } equals new { a = b.i_ParametroId, b = b.i_GrupoEtario, c = b.v_personId, d = b.i_GrupoData } into temp
                               from b in temp.DefaultIfEmpty()
                               where a.i_IsDeleted == isNotDeleted &&
                               a.i_GroupId == grupoHijo
                               select new EsoAntecedentesHijo
                               {
                                   Nombre = a.v_Value1,
                                   GrupoId = a.i_GroupId,
                                   ParametroId = a.i_ParameterId,
                                   SI = b == null ? false : b.i_Valor.HasValue ? b.i_Valor.Value == (int)SiNo.Si : false,
                                   NO = b == null ? false : b.i_Valor.HasValue ? b.i_Valor.Value == (int)SiNo.No : false
                               }).ToList();
                }

                return data;
            }
            catch (Exception e)
            {
                return new List<EsoAntecedentesPadre>();
            }
        }

        public List<EsoCuidadosPreventivosFechas> ObtenerFechasCuidadosPreventivos(string PersonId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                int isNotDeleted = (int)SiNo.No;

                var data = (from a in dbContext.Service
                            where a.i_IsDeleted == isNotDeleted &&
                            a.v_PersonId == PersonId &&
                            a.d_ServiceDate != null
                            select new EsoCuidadosPreventivosFechas()
                            {
                                FechaServicio = a.d_ServiceDate.Value
                            }).ToList();

                foreach (var item in data)
                {
                    item.FechaServicio = item.FechaServicio.Date;
                }

                return data;
            }
            catch (Exception e)
            {
                return new List<EsoCuidadosPreventivosFechas>();
            }
        }

        public List<EsoCuidadosPreventivos> ObtenerListadoCuidadosPreventivos(int GrupoPadre, string PersonId, DateTime FechaServicio)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                int isNotDeleted = (int)SiNo.No;

                var data = (from a in dbContext.SystemParameter
                            join b in dbContext.CuidadoPreventivo on new { a = PersonId, b = FechaServicio, c = a.i_GroupId, d = a.i_ParameterId, e = (int)isNotDeleted } equals new { a = b.v_PersonId, b = b.d_ServiceDate.Value, c = b.i_GrupoId, d = b.i_ParametroId, e = b.i_IsDeleted.Value } into temp
                            from b in temp.DefaultIfEmpty()
                            where a.i_IsDeleted == isNotDeleted &&
                            a.i_GroupId == GrupoPadre
                            select new EsoCuidadosPreventivos
                            {
                                ParameterId = a.i_ParameterId,
                                Nombre = a.v_Value1,
                                GrupoId = a.i_GroupId,
                                i_Valor = b.i_Valor,
                                Valor = b == null ? false : b.i_Valor == (int)SiNo.Si ? true : false
                            }).ToList();

                if (data.Count == 0)
                    return null;

                foreach (var D in data)
                {
                    int nuevoGrupo = int.Parse(GrupoPadre.ToString() + D.ParameterId.ToString());
                    D.Hijos = ObtenerListadoCuidadosPreventivos(nuevoGrupo, PersonId, FechaServicio);
                }


                return data;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public List<EsoCuidadosPreventivosComentarios> ObtenerComentariosCuidadosPreventivos(string PersonaId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                int isNotDeleted = (int)SiNo.No;

                var data = (from a in dbContext.CuidadoPreventivoComentario
                            where a.v_PersonId == PersonaId &&
                            a.i_IsDeleted == isNotDeleted
                            select new EsoCuidadosPreventivosComentarios()
                            {
                                GrupoId = a.i_GrupoId,
                                ParametroId = a.i_ParametroId,
                                Comentario = a.v_Comentario
                            }).ToList();

                return data;
            }
            catch (Exception e)
            {
                return new List<EsoCuidadosPreventivosComentarios>();
            }
        }

        public List<EmbarazoCustom> GetEmbarazos(string personId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var query = from a in dbContext.Embarazo
                            join b in dbContext.Person on a.v_PersonId equals b.v_PersonId
                            where a.v_PersonId == personId && a.i_IsDeleted == 0
                            select new EmbarazoCustom
                            {
                                PersonId = a.v_PersonId,
                                EmbarazoId = a.v_PersonId,
                                Anio = a.v_Anio,
                                Cpn = a.v_Cpn,
                                Complicacion = a.v_Complicacion,
                                Parto = a.v_Parto,
                                PesoRn = a.v_PesoRn,
                                Puerpio = a.v_Puerpio,
                                ObservacionesGestacion = a.v_ObservacionesGestacion
                            };

                List<EmbarazoCustom> objData = query.ToList();
                return objData;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AdultoMayorCustom DevolverAdultoMayor(string pstrServiceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var objEntity = (from a in dbContext.Service
                                 join b in dbContext.AdultoMayor on a.v_PersonId equals b.v_PersonId
                                 select new AdultoMayorCustom
                                 {
                                     v_NombreCuidador = b.v_NombreCuidador,
                                     v_EdadCuidador = b.v_EdadCuidador,
                                     v_DniCuidador = b.v_DniCuidador,

                                     v_MedicamentoFrecuente = b.v_MedicamentoFrecuente,
                                     v_ReaccionAlergica = b.v_ReaccionAlergica,
                                     v_InicioRS = b.v_InicioRS,
                                     v_NroPs = b.v_NroPs,
                                     v_FechaUR = b.v_FechaUR,
                                     v_RC = b.v_RC,
                                     v_Parto = b.v_Parto,
                                     v_Prematuro = b.v_Prematuro,
                                     v_Aborto = b.v_Aborto,
                                     v_DescripciónAntecedentes = b.v_DescripciónAntecedentes,
                                     v_FlujoVaginal = b.v_FlujoVaginal,
                                     v_ObservacionesEmbarazo = b.v_ObservacionesEmbarazo
                                 }).ToList();

                var result = (from a in objEntity
                              select new AdultoMayorCustom
                              {
                                  v_NombreCuidador = a.v_NombreCuidador,
                                  v_EdadCuidador = a.v_EdadCuidador,
                                  v_DniCuidador = a.v_DniCuidador,
                                  v_MedicamentoFrecuente = a.v_MedicamentoFrecuente,
                                  v_ReaccionAlergica = a.v_ReaccionAlergica,
                                  v_InicioRS = a.v_InicioRS,
                                  v_NroPs = a.v_NroPs,
                                  v_FechaUR = a.v_FechaUR,
                                  v_RC = a.v_RC,
                                  v_Parto = a.v_Parto,
                                  v_Prematuro = a.v_Prematuro,
                                  v_Aborto = a.v_Aborto,
                                  v_DescripciónAntecedentes = a.v_DescripciónAntecedentes,
                                  v_FlujoVaginal = a.v_FlujoVaginal,
                                  v_ObservacionesEmbarazo = a.v_ObservacionesEmbarazo
                              }
                        ).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public AdultoCustom DevolverAdulto(string pstrServiceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var objEntity = (from a in dbContext.Service
                                 join b in dbContext.Adulto on a.v_PersonId equals b.v_PersonId
                                 select new AdultoCustom
                                 {
                                     v_NombreCuidador = b.v_NombreCuidador,
                                     v_EdadCuidador = b.v_EdadCuidador,
                                     v_DniCuidador = b.v_DniCuidador,
                                     v_MedicamentoFrecuente = b.v_MedicamentoFrecuente,
                                     v_ReaccionAlergica = b.v_ReaccionAlergica,
                                     v_InicioRS = b.v_InicioRS,
                                     v_NroPs = b.v_NroPs,
                                     v_FechaUR = b.v_FechaUR,
                                     v_RC = b.v_RC,
                                     v_Parto = b.v_Parto,
                                     v_Prematuro = b.v_Prematuro,
                                     v_Aborto = b.v_Aborto,
                                     v_OtrosAntecedentes = b.v_OtrosAntecedentes,
                                     v_DescripcionAntecedentes = b.v_DescripcionAntecedentes,
                                     v_FlujoVaginal = b.v_FlujoVaginal,
                                     v_ObservacionesEmbarazo = b.v_ObservacionesEmbarazo

                                 }).ToList();

                var result = (from a in objEntity
                              select new AdultoCustom
                              {
                                  v_NombreCuidador = a.v_NombreCuidador,
                                  v_EdadCuidador = a.v_EdadCuidador,
                                  v_DniCuidador = a.v_DniCuidador,
                                  v_MedicamentoFrecuente = a.v_MedicamentoFrecuente,
                                  v_ReaccionAlergica = a.v_ReaccionAlergica,
                                  v_InicioRS = a.v_InicioRS,
                                  v_NroPs = a.v_NroPs,
                                  v_FechaUR = a.v_FechaUR,
                                  v_RC = a.v_RC,
                                  v_Parto = a.v_Parto,
                                  v_Prematuro = a.v_Prematuro,
                                  v_Aborto = a.v_Aborto,
                                  v_OtrosAntecedentes = a.v_OtrosAntecedentes,
                                  v_DescripcionAntecedentes = a.v_DescripcionAntecedentes,
                                  v_FlujoVaginal = a.v_FlujoVaginal,
                                  v_ObservacionesEmbarazo = a.v_ObservacionesEmbarazo
                              }
                        ).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public AdolescenteCustom DevolverAdolescente(string pstrServiceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var objEntity = (from a in dbContext.Service
                                 join b in dbContext.Adolescente on a.v_PersonId equals b.v_PersonId
                                 select new AdolescenteCustom
                                 {
                                     v_NombreCuidador = b.v_NombreCuidador,
                                     v_EdadCuidador = b.v_EdadCuidador,
                                     v_DniCuidador = b.v_DniCuidador,
                                     v_ViveCon = b.v_ViveCon,
                                     v_EdadInicioTrabajo = b.v_EdadInicioTrabajo,
                                     v_TipoTrabajo = b.v_TipoTrabajo,
                                     v_NroHorasTv = b.v_NroHorasTv,
                                     v_NroHorasJuegos = b.v_NroHorasJuegos,
                                     v_MenarquiaEspermarquia = b.v_MenarquiaEspermarquia,
                                     v_EdadInicioRS = b.v_EdadInicioRS,
                                     v_Observaciones = b.v_Observaciones
                                 }).ToList();

                var result = (from a in objEntity
                              select new AdolescenteCustom
                              {
                                  v_NombreCuidador = a.v_NombreCuidador,
                                  v_EdadCuidador = a.v_EdadCuidador,
                                  v_DniCuidador = a.v_DniCuidador,
                                  v_ViveCon = a.v_ViveCon,
                                  v_EdadInicioTrabajo = a.v_EdadInicioTrabajo,
                                  v_TipoTrabajo = a.v_TipoTrabajo,
                                  v_NroHorasTv = a.v_NroHorasTv,
                                  v_NroHorasJuegos = a.v_NroHorasJuegos,
                                  v_MenarquiaEspermarquia = a.v_MenarquiaEspermarquia,
                                  v_EdadInicioRS = a.v_EdadInicioRS,
                                  v_Observaciones = a.v_Observaciones
                              }
                        ).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public NinioCustom DevolverNinio(string pstrServiceId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();
                var objEntity = (from a in dbContext.Service
                                 join b in dbContext.Ninio on a.v_PersonId equals b.v_PersonId
                                 select new NinioCustom
                                 {
                                     v_NombreCuidador = b.v_NombreCuidador,
                                     v_EdadCuidador = b.v_EdadCuidador,
                                     v_DniCuidador = b.v_DniCuidador,
                                     v_NombreMadre = b.v_NombreMadre,
                                     v_NombrePadre = b.v_NombrePadre,
                                     v_EdadMadre = b.v_EdadMadre,
                                     v_EdadPadre = b.v_EdadPadre,
                                     v_DniMadre = b.v_DniMadre,
                                     v_DniPadre = b.v_DniPadre,
                                     i_TipoAfiliacionMadre = b.i_TipoAfiliacionMadre,
                                     i_TipoAfiliacionPadre = b.i_TipoAfiliacionPadre,
                                     v_CodigoAfiliacionMadre = b.v_CodigoAfiliacionMadre,
                                     v_CodigoAfiliacionPadre = b.v_CodigoAfiliacionPadre,
                                     i_GradoInstruccionMadre = b.i_GradoInstruccionMadre,
                                     i_GradoInstruccionPadre = b.i_GradoInstruccionPadre,
                                     v_OcupacionMadre = b.v_OcupacionMadre,
                                     v_OcupacionPadre = b.v_OcupacionPadre,
                                     i_EstadoCivilIdMadre1 = b.i_EstadoCivilIdMadre1,
                                     i_EstadoCivilIdPadre = b.i_EstadoCivilIdPadre,
                                     v_ReligionMadre = b.v_ReligionMadre,
                                     v_ReligionPadre = b.v_ReligionPadre,
                                     v_PatologiasGestacion = b.v_PatologiasGestacion,
                                     v_nEmbarazos = b.v_nEmbarazos,
                                     v_nAPN = b.v_nAPN,
                                     v_LugarAPN = b.v_LugarAPN,
                                     v_ComplicacionesParto = b.v_ComplicacionesParto,
                                     v_Atencion = b.v_Atencion,
                                     v_EdadGestacion = b.v_EdadGestacion,
                                     v_Peso = b.v_Peso,
                                     v_Talla = b.v_Talla,
                                     v_PerimetroCefalico = b.v_PerimetroCefalico,
                                     v_PerimetroToracico = b.v_PerimetroToracico,
                                     v_EspecificacionesNac = b.v_EspecificacionesNac,
                                     v_LME = b.v_LME,
                                     v_Mixta = b.v_Mixta,
                                     v_Artificial = b.v_Artificial,
                                     v_InicioAlimentacionComp = b.v_InicioAlimentacionComp,
                                     v_AlergiasMedicamentos = b.v_AlergiasMedicamentos,
                                     v_OtrosAntecedentes = b.v_OtrosAntecedentes,
                                     v_EspecificacionesAgua = b.v_EspecificacionesAgua,
                                     v_EspecificacionesDesague = b.v_EspecificacionesDesague,
                                     v_TiempoHospitalizacion = b.v_TiempoHospitalizacion,
                                     v_QuienTuberculosis = b.v_QuienTuberculosis,
                                     v_QuienAsma = b.v_QuienAsma,
                                     v_QuienVIH = b.v_QuienVIH,
                                     v_QuienDiabetes = b.v_QuienDiabetes,
                                     v_QuienEpilepsia = b.v_QuienEpilepsia,
                                     v_QuienAlergias = b.v_QuienAlergias,
                                     v_QuienViolenciaFamiliar = b.v_QuienViolenciaFamiliar,
                                     v_QuienAlcoholismo = b.v_QuienAlcoholismo,
                                     v_QuienDrogadiccion = b.v_QuienDrogadiccion,
                                     v_QuienHeptitisB = b.v_QuienHeptitisB,
                                     i_QuienTuberculosis = b.i_QuienTuberculosis,
                                     i_QuienAsma = b.i_QuienAsma,
                                     i_QuienVIH = b.i_QuienVIH,
                                     i_QuienDiabetes = b.i_QuienDiabetes,
                                     i_QuienEpilepsia = b.i_QuienEpilepsia,
                                     i_QuienAlergias = b.i_QuienAlergias,
                                     i_QuienViolenciaFamiliar = b.i_QuienViolenciaFamiliar,
                                     i_QuienAlcoholismo = b.i_QuienAlcoholismo,
                                     i_QuienDrogadiccion = b.i_QuienDrogadiccion,
                                     i_QuienHeptitisB = b.i_QuienHeptitisB
                                 }).ToList();

                var result = (from a in objEntity
                              select new NinioCustom
                              {
                                  v_NombreCuidador = a.v_NombreCuidador,
                                  v_EdadCuidador = a.v_EdadCuidador,
                                  v_DniCuidador = a.v_DniCuidador,
                                  v_NombreMadre = a.v_NombreMadre,
                                  v_NombrePadre = a.v_NombrePadre,
                                  v_EdadMadre = a.v_EdadMadre,
                                  v_EdadPadre = a.v_EdadPadre,
                                  v_DniMadre = a.v_DniMadre,
                                  v_DniPadre = a.v_DniPadre,
                                  i_TipoAfiliacionMadre = a.i_TipoAfiliacionMadre,
                                  i_TipoAfiliacionPadre = a.i_TipoAfiliacionPadre,
                                  v_CodigoAfiliacionMadre = a.v_CodigoAfiliacionMadre,
                                  v_CodigoAfiliacionPadre = a.v_CodigoAfiliacionPadre,
                                  i_GradoInstruccionMadre = a.i_GradoInstruccionMadre,
                                  i_GradoInstruccionPadre = a.i_GradoInstruccionPadre,
                                  v_OcupacionMadre = a.v_OcupacionMadre,
                                  v_OcupacionPadre = a.v_OcupacionPadre,
                                  i_EstadoCivilIdMadre1 = a.i_EstadoCivilIdMadre1,
                                  i_EstadoCivilIdPadre = a.i_EstadoCivilIdPadre,
                                  v_ReligionMadre = a.v_ReligionMadre,
                                  v_ReligionPadre = a.v_ReligionPadre,
                                  v_PatologiasGestacion = a.v_PatologiasGestacion,
                                  v_nEmbarazos = a.v_nEmbarazos,
                                  v_nAPN = a.v_nAPN,
                                  v_LugarAPN = a.v_LugarAPN,
                                  v_ComplicacionesParto = a.v_ComplicacionesParto,
                                  v_Atencion = a.v_Atencion,
                                  v_EdadGestacion = a.v_EdadGestacion,
                                  v_Peso = a.v_Peso,
                                  v_Talla = a.v_Talla,
                                  v_PerimetroCefalico = a.v_PerimetroCefalico,
                                  v_PerimetroToracico = a.v_PerimetroToracico,
                                  v_EspecificacionesNac = a.v_EspecificacionesNac,
                                  v_LME = a.v_LME,
                                  v_Mixta = a.v_Mixta,
                                  v_Artificial = a.v_Artificial,
                                  v_InicioAlimentacionComp = a.v_InicioAlimentacionComp,
                                  v_AlergiasMedicamentos = a.v_AlergiasMedicamentos,
                                  v_OtrosAntecedentes = a.v_OtrosAntecedentes,
                                  v_EspecificacionesAgua = a.v_EspecificacionesAgua,
                                  v_EspecificacionesDesague = a.v_EspecificacionesDesague,
                                  v_TiempoHospitalizacion = a.v_TiempoHospitalizacion,
                                  v_QuienTuberculosis = a.v_QuienTuberculosis,
                                  v_QuienAsma = a.v_QuienAsma,
                                  v_QuienVIH = a.v_QuienVIH,
                                  v_QuienDiabetes = a.v_QuienDiabetes,
                                  v_QuienEpilepsia = a.v_QuienEpilepsia,
                                  v_QuienAlergias = a.v_QuienAlergias,
                                  v_QuienViolenciaFamiliar = a.v_QuienViolenciaFamiliar,
                                  v_QuienAlcoholismo = a.v_QuienAlcoholismo,
                                  v_QuienDrogadiccion = a.v_QuienDrogadiccion,
                                  v_QuienHeptitisB = a.v_QuienHeptitisB,
                                  i_QuienTuberculosis = a.i_QuienTuberculosis,
                                  i_QuienAsma = a.i_QuienAsma,
                                  i_QuienVIH = a.i_QuienVIH,
                                  i_QuienDiabetes = a.i_QuienDiabetes,
                                  i_QuienEpilepsia = a.i_QuienEpilepsia,
                                  i_QuienAlergias = a.i_QuienAlergias,
                                  i_QuienViolenciaFamiliar = a.i_QuienViolenciaFamiliar,
                                  i_QuienAlcoholismo = a.i_QuienAlcoholismo,
                                  i_QuienDrogadiccion = a.i_QuienDrogadiccion,
                                  i_QuienHeptitisB = a.i_QuienHeptitisB
                              }
                        ).FirstOrDefault();
                return result;
            }
            catch (Exception)
            {
                return null;
            }

        }

        public string SaveDataNinio(NinioBE data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 356 , "NÑ");
                data.v_NinioId = newId;
                data.i_IsDeleted = (int)SiNo.No;
                data.i_InsertUserId = userId;
                data.d_InsertDate = DateTime.Now;

                ctx.Ninio.Add(data);
                ctx.SaveChanges();
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdateDataNinio(NinioCustom data, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objNinio = ctx.Ninio.Where(x => x.v_NinioId == data.v_NinioId).FirstOrDefault();

                objNinio.v_NombreCuidador = data.v_NombreCuidador;
                objNinio.v_EdadCuidador = data.v_EdadCuidador;
                objNinio.v_DniCuidador = data.v_DniCuidador;
                objNinio.v_NombrePadre = data.v_NombrePadre;
                objNinio.v_EdadPadre = data.v_DniPadre;
                objNinio.v_DniPadre = data.v_DniPadre;
                objNinio.i_TipoAfiliacionPadre = data.i_TipoAfiliacionPadre;
                objNinio.v_CodigoAfiliacionPadre = data.v_CodigoAfiliacionPadre;
                objNinio.i_GradoInstruccionPadre = data.i_GradoInstruccionPadre;
                objNinio.v_OcupacionPadre = data.v_OcupacionPadre;
                objNinio.i_EstadoCivilIdPadre = data.i_EstadoCivilIdPadre;
                objNinio.v_ReligionPadre = data.v_ReligionPadre;
                objNinio.v_NombreMadre = data.v_NombreMadre;
                objNinio.v_EdadMadre = data.v_EdadMadre;
                objNinio.v_DniMadre = data.v_DniMadre;
                objNinio.i_TipoAfiliacionMadre = data.i_TipoAfiliacionMadre;
                objNinio.v_CodigoAfiliacionMadre = data.v_CodigoAfiliacionMadre;
                objNinio.i_GradoInstruccionMadre = data.i_GradoInstruccionMadre;
                objNinio.v_OcupacionMadre = data.v_OcupacionMadre;
                objNinio.i_EstadoCivilIdMadre1 = data.i_EstadoCivilIdMadre1;
                objNinio.v_ReligionMadre = data.v_ReligionMadre;
                objNinio.v_PatologiasGestacion = data.v_PatologiasGestacion;
                objNinio.v_nEmbarazos = data.v_nEmbarazos;
                objNinio.v_nAPN = data.v_nAPN;
                objNinio.v_LugarAPN = data.v_LugarAPN;
                objNinio.v_ComplicacionesParto = data.v_ComplicacionesParto;
                objNinio.v_Atencion = data.v_Atencion;
                objNinio.v_EdadGestacion = data.v_EdadGestacion;
                objNinio.v_Peso = data.v_Peso;
                objNinio.v_Talla = data.v_Talla;
                objNinio.v_PerimetroCefalico = data.v_PerimetroCefalico;
                objNinio.v_PerimetroToracico = data.v_PerimetroToracico;
                objNinio.v_EspecificacionesNac = data.v_EspecificacionesNac;
                objNinio.v_InicioAlimentacionComp = data.v_InicioAlimentacionComp;
                objNinio.v_AlergiasMedicamentos = data.v_AlergiasMedicamentos;
                objNinio.v_OtrosAntecedentes = data.v_OtrosAntecedentes;
                objNinio.v_EspecificacionesAgua = data.v_EspecificacionesAgua;
                objNinio.v_EspecificacionesDesague = data.v_EspecificacionesDesague;
                objNinio.v_TiempoHospitalizacion = data.v_TiempoHospitalizacion;
                objNinio.v_QuienTuberculosis = data.v_QuienTuberculosis;
                objNinio.i_QuienTuberculosis = data.i_QuienTuberculosis;
                objNinio.v_QuienAsma = data.v_QuienAsma;
                objNinio.i_QuienAsma = data.i_QuienAsma;
                objNinio.v_QuienVIH = data.v_QuienVIH;
                objNinio.i_QuienVIH = data.i_QuienVIH;
                objNinio.v_QuienDiabetes = data.v_QuienDiabetes;
                objNinio.i_QuienDiabetes = data.i_QuienDiabetes;
                objNinio.v_QuienEpilepsia = data.v_QuienEpilepsia;
                objNinio.i_QuienEpilepsia = data.i_QuienEpilepsia;
                objNinio.v_QuienAlergias = data.v_QuienAlergias;
                objNinio.i_QuienAlergias = data.i_QuienAlergias;
                objNinio.v_QuienViolenciaFamiliar = data.v_QuienViolenciaFamiliar;
                objNinio.i_QuienViolenciaFamiliar = data.i_QuienViolenciaFamiliar;
                objNinio.v_QuienAlcoholismo = data.v_QuienAlcoholismo;
                objNinio.i_QuienAlcoholismo = data.i_QuienAlcoholismo;
                objNinio.v_QuienDrogadiccion = data.v_QuienDrogadiccion;
                objNinio.i_QuienDrogadiccion = data.i_QuienDrogadiccion;
                objNinio.v_QuienHeptitisB = data.v_QuienHeptitisB;
                objNinio.i_QuienHeptitisB = data.i_QuienHeptitisB;


                objNinio.i_UpdateUserId = userId;
                objNinio.d_UpdateDate = DateTime.Now;
                ctx.SaveChanges();
                return data.v_NinioId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveDataAdolescente(AdolescenteBE data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 353, "JO");
                data.v_AdolescenteId = newId;
                data.i_IsDeleted = (int)SiNo.No;
                data.i_InsertUserId = userId;
                data.d_InsertDate = DateTime.Now;

                ctx.Adolescente.Add(data);
                ctx.SaveChanges();
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdateDataAdolescente(AdolescenteCustom data, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objAdolescente= ctx.Adolescente.Where(x => x.v_AdolescenteId == data.v_AdolescenteId).FirstOrDefault();

                objAdolescente.v_NombreCuidador = data.v_NombreCuidador;
                objAdolescente.v_EdadCuidador = data.v_EdadCuidador;
                objAdolescente.v_DniCuidador = data.v_DniCuidador;
                objAdolescente.v_EdadInicioTrabajo = data.v_EdadInicioTrabajo;
                objAdolescente.v_TipoTrabajo = data.v_TipoTrabajo;
                objAdolescente.v_NroHorasTv = data.v_NroHorasTv;
                objAdolescente.v_NroHorasJuegos = data.v_NroHorasJuegos;
                objAdolescente.v_MenarquiaEspermarquia = data.v_MenarquiaEspermarquia;
                objAdolescente.v_ViveCon = data.v_ViveCon;
                objAdolescente.v_EdadInicioRS = data.v_EdadInicioRS;
                objAdolescente.v_Observaciones = data.v_Observaciones;

                objAdolescente.i_UpdateUserId = userId;
                objAdolescente.d_UpdateDate = DateTime.Now;

                ctx.SaveChanges();
                return data.v_AdolescenteId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveDataAdulto(AdultoBE data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 354, "AD");
                data.v_AdultoId = newId;
                data.i_IsDeleted = (int)SiNo.No;
                data.i_InsertUserId = userId;
                data.d_InsertDate = DateTime.Now;

                ctx.Adulto.Add(data);
                ctx.SaveChanges();

                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdateDataAdulto(AdultoCustom data, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objAdulto = ctx.Adulto.Where(x => x.v_AdultoId == data.v_AdultoId).FirstOrDefault();

                objAdulto.v_NombreCuidador = data.v_NombreCuidador;
                objAdulto.v_EdadCuidador = data.v_EdadCuidador;
                objAdulto.v_DniCuidador = data.v_DniCuidador;
                objAdulto.v_MedicamentoFrecuente = data.v_MedicamentoFrecuente;
                objAdulto.v_ReaccionAlergica = data.v_ReaccionAlergica;
                objAdulto.v_InicioRS = data.v_InicioRS;
                objAdulto.v_NroPs = data.v_NroPs;
                objAdulto.v_FechaUR = data.v_FechaUR;
                objAdulto.v_RC = data.v_RC;
                objAdulto.v_Parto = data.v_Parto;
                objAdulto.v_Prematuro = data.v_Prematuro;
                objAdulto.v_Aborto = data.v_Aborto;
                objAdulto.v_DescripcionAntecedentes = data.v_DescripcionAntecedentes;
                objAdulto.v_OtrosAntecedentes = data.v_OtrosAntecedentes;
                objAdulto.v_FlujoVaginal = data.v_FlujoVaginal;
                objAdulto.v_ObservacionesEmbarazo = data.v_ObservacionesEmbarazo;

                objAdulto.i_UpdateUserId = userId;
                objAdulto.d_UpdateDate = DateTime.Now;
                ctx.SaveChanges();
                return data.v_AdultoId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string SaveDataAdultoMayor(AdultoMayorBE data, int userId, int nodeId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var newId = new Common.Utils().GetPrimaryKey(nodeId, 355, "AM");
                data.v_AdultoMayorId = newId;
                data.i_IsDeleted = (int)SiNo.No;
                data.i_InsertUserId = userId;
                data.d_InsertDate = DateTime.Now;

                ctx.AdultoMayor.Add(data);
                ctx.SaveChanges();
                return newId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string UpdateDataAdultoMayor(AdultoMayorCustom data, int userId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objAdultoMayor = ctx.AdultoMayor.Where(x => x.v_AdultoMayorId == data.v_AdultoMayorId).FirstOrDefault();
                objAdultoMayor.v_NombreCuidador = data.v_NombreCuidador;
                objAdultoMayor.v_EdadCuidador = data.v_EdadCuidador;
                objAdultoMayor.v_DniCuidador = data.v_DniCuidador;
                objAdultoMayor.v_MedicamentoFrecuente = data.v_MedicamentoFrecuente;
                objAdultoMayor.v_ReaccionAlergica = data.v_ReaccionAlergica;
                objAdultoMayor.v_InicioRS = data.v_InicioRS;
                objAdultoMayor.v_NroPs = data.v_NroPs;
                objAdultoMayor.v_FechaUR = data.v_FechaUR;
                objAdultoMayor.v_RC = data.v_RC;
                objAdultoMayor.v_Parto = data.v_Parto;
                objAdultoMayor.v_Prematuro = data.v_Prematuro;
                objAdultoMayor.v_Aborto = data.v_Aborto;
                objAdultoMayor.v_DescripciónAntecedentes = data.v_DescripciónAntecedentes;
                objAdultoMayor.v_FlujoVaginal = data.v_FlujoVaginal;
                objAdultoMayor.v_ObservacionesEmbarazo = data.v_ObservacionesEmbarazo;

                objAdultoMayor.d_UpdateDate = DateTime.Now;
                objAdultoMayor.i_UpdateUserId = userId;
                ctx.SaveChanges();
                return data.v_AdultoMayorId;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public NinioCustom GetNinio(string _PersonId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from a in dbContext.Ninio
                                 where a.v_PersonId == _PersonId
                                 select new NinioCustom
                                 {
                                    v_NinioId = a.v_NinioId,
                                    v_NombreCuidador = a.v_NombreCuidador,
                                    v_EdadCuidador = a.v_EdadCuidador,
                                    v_DniCuidador = a.v_DniCuidador,
                                    v_NombrePadre = a.v_NombrePadre,
                                    v_EdadPadre = a.v_DniPadre,
                                    v_DniPadre = a.v_DniPadre,
                                    i_TipoAfiliacionPadre = a.i_TipoAfiliacionPadre,
                                    v_CodigoAfiliacionPadre = a.v_CodigoAfiliacionPadre,
                                    i_GradoInstruccionPadre = a.i_GradoInstruccionPadre,
                                    v_OcupacionPadre = a.v_OcupacionPadre,
                                    i_EstadoCivilIdPadre = a.i_EstadoCivilIdPadre,
                                    v_ReligionPadre = a.v_ReligionPadre,
                                    v_NombreMadre = a.v_NombreMadre,
                                    v_EdadMadre = a.v_EdadMadre,
                                    v_DniMadre = a.v_DniMadre,
                                    i_TipoAfiliacionMadre = a.i_TipoAfiliacionMadre,
                                    v_CodigoAfiliacionMadre = a.v_CodigoAfiliacionMadre,
                                    i_GradoInstruccionMadre = a.i_GradoInstruccionMadre,
                                    v_OcupacionMadre = a.v_OcupacionMadre,
                                    i_EstadoCivilIdMadre1 = a.i_EstadoCivilIdMadre1,
                                    v_ReligionMadre = a.v_ReligionMadre,
                                    v_PatologiasGestacion = a.v_PatologiasGestacion,
                                    v_nEmbarazos = a.v_nEmbarazos,
                                    v_nAPN = a.v_nAPN,
                                    v_LugarAPN = a.v_LugarAPN,
                                    v_ComplicacionesParto = a.v_ComplicacionesParto,
                                    v_Atencion = a.v_Atencion,
                                    v_EdadGestacion = a.v_EdadGestacion,
                                    v_Peso = a.v_Peso,
                                    v_Talla = a.v_Talla,
                                    v_PerimetroCefalico = a.v_PerimetroCefalico,
                                    v_PerimetroToracico = a.v_PerimetroToracico,
                                    v_EspecificacionesNac = a.v_EspecificacionesNac,
                                    v_InicioAlimentacionComp = a.v_InicioAlimentacionComp,
                                    v_AlergiasMedicamentos = a.v_AlergiasMedicamentos,
                                    v_OtrosAntecedentes = a.v_OtrosAntecedentes,
                                    v_EspecificacionesAgua = a.v_EspecificacionesAgua,
                                    v_EspecificacionesDesague = a.v_EspecificacionesDesague,
                                    v_TiempoHospitalizacion = a.v_TiempoHospitalizacion,
                                    v_QuienTuberculosis = a.v_QuienTuberculosis,
                                    i_QuienTuberculosis = a.i_QuienTuberculosis,
                                    v_QuienAsma = a.v_QuienAsma,
                                    i_QuienAsma = a.i_QuienAsma,
                                    v_QuienVIH = a.v_QuienVIH,
                                    i_QuienVIH = a.i_QuienVIH,
                                    v_QuienDiabetes = a.v_QuienDiabetes,
                                    i_QuienDiabetes = a.i_QuienDiabetes,
                                    v_QuienEpilepsia = a.v_QuienEpilepsia,
                                    i_QuienEpilepsia = a.i_QuienEpilepsia,
                                    v_QuienAlergias = a.v_QuienAlergias,
                                    i_QuienAlergias = a.i_QuienAlergias,
                                    v_QuienViolenciaFamiliar = a.v_QuienViolenciaFamiliar,
                                    i_QuienViolenciaFamiliar = a.i_QuienViolenciaFamiliar,
                                    v_QuienAlcoholismo = a.v_QuienAlcoholismo,
                                    i_QuienAlcoholismo = a.i_QuienAlcoholismo,
                                    v_QuienDrogadiccion = a.v_QuienDrogadiccion,
                                    i_QuienDrogadiccion = a.i_QuienDrogadiccion,
                                    v_QuienHeptitisB = a.v_QuienHeptitisB,
                                    i_QuienHeptitisB = a.i_QuienHeptitisB,

                                    }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AdolescenteCustom GetAdolescente(string _PersonId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from a in dbContext.Adolescente
                                 where a.v_PersonId == _PersonId
                                 select new AdolescenteCustom
                                 {
                                    v_AdolescenteId = a.v_AdolescenteId,
                                    v_NombreCuidador = a.v_NombreCuidador,
                                    v_EdadCuidador = a.v_EdadCuidador,
                                    v_DniCuidador = a.v_DniCuidador,
                                    v_EdadInicioTrabajo = a.v_EdadInicioTrabajo,
                                    v_TipoTrabajo = a.v_TipoTrabajo,
                                    v_NroHorasTv = a.v_NroHorasTv,
                                    v_NroHorasJuegos = a.v_NroHorasJuegos,
                                    v_MenarquiaEspermarquia = a.v_MenarquiaEspermarquia,
                                    v_ViveCon = a.v_ViveCon,
                                    v_EdadInicioRS = a.v_EdadInicioRS,
                                    v_Observaciones = a.v_Observaciones,

                                }).FirstOrDefault();
                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AdultoCustom GetAdulto(string _PersonId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from a in dbContext.Adulto
                                 where a.v_PersonId == _PersonId
                                 select new AdultoCustom
                                 {
                                    v_AdultoId = a.v_AdultoId,
                                    v_NombreCuidador = a.v_NombreCuidador,
                                    v_EdadCuidador = a.v_EdadCuidador,
                                    v_DniCuidador = a.v_DniCuidador,
                                    v_MedicamentoFrecuente = a.v_MedicamentoFrecuente,
                                    v_ReaccionAlergica = a.v_ReaccionAlergica,
                                    v_InicioRS = a.v_InicioRS,
                                    v_NroPs = a.v_NroPs,
                                    v_FechaUR = a.v_FechaUR,
                                    v_RC = a.v_RC,
                                    v_Parto = a.v_Parto,
                                    v_Prematuro = a.v_Prematuro,
                                    v_Aborto = a.v_Aborto,
                                    v_DescripcionAntecedentes = a.v_DescripcionAntecedentes,
                                    v_OtrosAntecedentes = a.v_OtrosAntecedentes,
                                    v_FlujoVaginal = a.v_FlujoVaginal,
                                    v_ObservacionesEmbarazo = a.v_ObservacionesEmbarazo,
    
                                 }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public AdultoCustom GetAdultoMayor(string _PersonId)
        {
            try
            {
                DatabaseContext dbContext = new DatabaseContext();

                var objEntity = (from a in dbContext.AdultoMayor
                                 where a.v_PersonId == _PersonId
                                 select new AdultoCustom
                                 {
                                    v_AdultoId = a.v_AdultoMayorId,
                                    v_NombreCuidador = a.v_NombreCuidador,
                                    v_EdadCuidador = a.v_EdadCuidador,
                                    v_DniCuidador = a.v_DniCuidador,
                                    v_MedicamentoFrecuente = a.v_MedicamentoFrecuente,
                                    v_ReaccionAlergica = a.v_ReaccionAlergica,
                                    v_InicioRS = a.v_InicioRS,
                                    v_NroPs = a.v_NroPs,
                                    v_FechaUR = a.v_FechaUR,
                                    v_RC = a.v_RC,
                                    v_Parto = a.v_Parto,
                                    v_Prematuro = a.v_Prematuro,
                                    v_Aborto = a.v_Aborto,
                                    v_DescripcionAntecedentes = a.v_DescripciónAntecedentes,
                                    v_FlujoVaginal = a.v_FlujoVaginal,
                                    v_ObservacionesEmbarazo = a.v_ObservacionesEmbarazo,
                                  }).FirstOrDefault();

                return objEntity;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool GuardarAntecedenteAsistencial(List<EsoAntecedentesPadre> Listado, int SesionUserId, string PacienteId, int GrupoEtario, int intNodeId)
        {
            try
            {
                int IsNotDeleted = (int)SiNo.No;
                DatabaseContext dbContext = new DatabaseContext();
                AntecedentesAsistencialBE AA = new AntecedentesAsistencialBE();
                int row = 0;

                bool YaEstaRegistrado = (from a in dbContext.AntecedentesAsistencial where a.i_IsDeleted == IsNotDeleted && a.v_personId == PacienteId && a.i_GrupoEtario == GrupoEtario select a).Count() > 0;

                if (YaEstaRegistrado)
                {
                    foreach (var P in Listado)
                    {
                        foreach (var H in P.Hijos)
                        {
                            int grupoData = int.Parse(H.GrupoId.ToString().ToCharArray()[4].ToString());
                            var data = (from a in dbContext.AntecedentesAsistencial where a.i_IsDeleted == IsNotDeleted && a.v_personId == PacienteId && a.i_GrupoEtario == GrupoEtario && a.i_GrupoData == grupoData && a.i_ParametroId == H.ParametroId select a).FirstOrDefault();

                            if (data == null)
                            {
                                AA = new AntecedentesAsistencialBE()
                                {
                                    i_InsertUserId = SesionUserId,
                                    d_InsertDate = DateTime.Now,
                                    v_personId = PacienteId,
                                    i_GrupoEtario = GrupoEtario,
                                    i_GrupoData = grupoData,
                                    i_ParametroId = H.ParametroId,
                                    i_Valor = H.SI ? (int?)SiNo.Si : H.NO ? (int?)SiNo.No : null,
                                    i_IsDeleted = IsNotDeleted,
                                    v_AntecendenteAsistencialId = new Common.Utils().GetPrimaryKey(intNodeId, 328, "AT")
                                };
                                dbContext.AntecedentesAsistencial.Add(AA);
                            }
                            else
                            {
                                int? valor = H.SI ? (int?)SiNo.Si : H.NO ? (int?)SiNo.No : null;

                                if (data.i_Valor != valor)
                                {
                                    data.i_UpdateUserId = SesionUserId;
                                    data.d_UpdateDate = DateTime.Now;
                                    data.i_Valor = valor;
                                    row = row + dbContext.SaveChanges();
                                }
                            }
                        }
                    }
                }
                else
                {
                    foreach (var P in Listado)
                    {
                        foreach (var H in P.Hijos)
                        {
                            int grupoData = int.Parse(H.GrupoId.ToString().ToCharArray()[4].ToString());
                            AA = new AntecedentesAsistencialBE()
                            {
                                i_InsertUserId = SesionUserId,
                                d_InsertDate = DateTime.Now,
                                v_personId = PacienteId,
                                i_GrupoEtario = GrupoEtario,
                                i_GrupoData = grupoData,
                                i_ParametroId = H.ParametroId,
                                i_Valor = H.SI ? (int?)SiNo.Si : H.NO ? (int?)SiNo.No : null,
                                i_IsDeleted = IsNotDeleted,
                                v_AntecendenteAsistencialId = new Common.Utils().GetPrimaryKey(intNodeId, 328, "AT")
                            };
                            dbContext.AntecedentesAsistencial.Add(AA);
                        }
                    }
                }

                row = row + dbContext.SaveChanges();
                return row > 0;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool SaveCuidadosPreventivos(EsoCuidadosPreventivosFechas data, string personId, int userId, int nodeId)
        {
            try
            {
                int IsNotDeleted = (int)SiNo.No;
                DatabaseContext dbContext = new DatabaseContext();
                int row = 0;

                bool YaEstaRegistrado = (from a in dbContext.CuidadoPreventivo where a.i_IsDeleted == IsNotDeleted && a.v_PersonId == personId && a.d_ServiceDate == data.FechaServicio.Date select a).Count() > 0;

                if (YaEstaRegistrado)
                {
                    foreach (var D in data.Listado)
                    {
                        var temp = (from a in dbContext.CuidadoPreventivo where a.i_IsDeleted == IsNotDeleted && a.v_PersonId == personId && a.d_ServiceDate == data.FechaServicio.Date && a.i_GrupoId == D.GrupoId && a.i_ParametroId == D.ParameterId select a).FirstOrDefault();

                        if (temp == null)
                        {
                            CuidadoPreventivoBE CP = new CuidadoPreventivoBE()
                            {
                                d_InsertDate = DateTime.Now,
                                d_ServiceDate = data.FechaServicio.Date,
                                i_GrupoId = D.GrupoId,
                                i_IsDeleted = IsNotDeleted,
                                i_InsertUserId = userId,
                                i_ParametroId = D.ParameterId,
                                i_Valor = D.Valor ? (int)SiNo.Si : (int)SiNo.No,
                                v_PersonId = personId,
                                v_CuidadoPreventivoId = new Common.Utils().GetPrimaryKey(nodeId, 329, "CP")
                            };

                            dbContext.CuidadoPreventivo.Add(CP);
                        }
                        else
                        {
                            if (temp.i_Valor != (D.Valor ? (int)SiNo.Si : (int)SiNo.No))
                            {
                                temp.d_UpdateDate = DateTime.Now;
                                temp.i_UpdateUserId = userId;
                                temp.i_Valor = D.Valor ? (int)SiNo.Si : (int)SiNo.No;

                                row = +dbContext.SaveChanges();
                            }
                        }
                    }

                }
                else
                {
                    foreach (var D in data.Listado)
                    {
                        CuidadoPreventivoBE CP = new CuidadoPreventivoBE()
                        {
                            d_InsertDate = DateTime.Now,
                            d_ServiceDate = data.FechaServicio.Date,
                            i_GrupoId = D.GrupoId,
                            i_IsDeleted = IsNotDeleted,
                            i_InsertUserId = userId,
                            i_ParametroId = D.ParameterId,
                            i_Valor = D.Valor ? (int)SiNo.Si : (int)SiNo.No,
                            v_PersonId = personId,
                            v_CuidadoPreventivoId = new Common.Utils().GetPrimaryKey(nodeId, 329, "CP")
                        };

                        dbContext.CuidadoPreventivo.Add(CP);
                    }
                }

                row = row + dbContext.SaveChanges();

                foreach (var C in data.Listado)
                {
                    var temporal = (from a in dbContext.CuidadoPreventivoComentario
                                    where
                                        a.i_IsDeleted == IsNotDeleted &&
                                        a.v_PersonId == personId &&
                                        a.i_ParametroId == C.ParameterId &&
                                        a.i_GrupoId == C.GrupoId
                                    select a).FirstOrDefault();

                    if (temporal != null)
                    {
                        if (temporal.v_Comentario != C.Comentario)
                        {
                            temporal.i_UpdateUserId = userId;
                            temporal.d_UpdateDate = DateTime.Now;
                            temporal.v_Comentario = C.Comentario;
                            row = row + dbContext.SaveChanges();
                        }
                    }
                    else
                    {
                        CuidadoPreventivoComentarioBE CPC = new CuidadoPreventivoComentarioBE()
                        {
                            d_InsertDate = DateTime.Now,
                            i_GrupoId = C.GrupoId,
                            i_InsertUserId = userId,
                            i_IsDeleted = IsNotDeleted,
                            i_ParametroId = C.ParameterId,
                            v_Comentario = C.Comentario,
                            v_PersonId = personId,
                            v_CuidadoPreventivoComentarioId = new Common.Utils().GetPrimaryKey(nodeId, 330, "CC")
                        };

                        dbContext.CuidadoPreventivoComentario.Add(CPC);
                        row = row + dbContext.SaveChanges();
                    }
                }

                row = row + dbContext.SaveChanges();

                return true;
            }
            catch (Exception ex )
            {
                return false;
            }
        }

    }

}
