using BE.Antecedentes;
using BE.Common;
using BE.Message;
using BE.Ninio;
using BE.Sigesoft;
using BL.Common;
using DAL;
using DAL.Antecedentes;
using DAL.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using static BE.Common.Enumeratores;

namespace BL.Antecedentes
{
    public class EsoAntecedentesBL
    {
        public BoardEsoAntecedentes ObtenerEsoAntecedentesPorGrupoId(string PersonId)
        {
            BoardEsoAntecedentes _BoardEsoAntecedentes = new BoardEsoAntecedentes();
            DatabaseContext ctx = new DatabaseContext();
            DateTime BirthDatePacient = ctx.Person.Where(x => x.v_PersonId == PersonId).FirstOrDefault().d_Birthdate.Value;
            int Edad = new PacientBL().GetEdad(BirthDatePacient);
            int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
            int GrupoBase = 282; //Antecedentes
            int Grupo = int.Parse(GrupoBase.ToString() + GrupoEtario.ToString());

            var Actual = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(Grupo, GrupoEtario, PersonId);

            int GrupoEtarioAnterior = 0;

            switch (GrupoEtario)
            {
                case 1:
                    {
                        GrupoEtarioAnterior = 2;
                        break;
                    }
                case 2:
                    {
                        GrupoEtarioAnterior = 4;
                        break;
                    }
                case 3:
                    {
                        GrupoEtarioAnterior = 1;
                        break;
                    }
                case 4:
                    {
                        GrupoEtarioAnterior = 4;
                        break;
                    }
                default:
                    {
                        GrupoEtarioAnterior = 0;
                        break;
                    }
            }

            var Anterior = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(Grupo, GrupoEtarioAnterior, PersonId);

            _BoardEsoAntecedentes.AntecedenteActual = Actual;
            _BoardEsoAntecedentes.AntecedenteAnterior = Anterior;
            return _BoardEsoAntecedentes;
        }

        public List<EsoCuidadosPreventivosFechas> ObtenerFechasCuidadosPreventivos(string PersonId)
        {
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var objPacient = ctx.Person.Where(x => x.v_PersonId == PersonId).FirstOrDefault();
                int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                int _GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                int GrupoBase = 0;
                switch (_GrupoEtario)
                {
                    case (int)GrupoEtario.Ninio:
                        {
                            GrupoBase = 292;
                            break;
                        }
                    case (int)GrupoEtario.Adolecente:
                        {
                            GrupoBase = 285;
                            break;
                        }
                    case (int)GrupoEtario.Adulto:
                        {
                            if (objPacient.i_SexTypeId == 1)
                            {
                                GrupoBase = 284;
                                break;
                            }
                            else
                            {
                                GrupoBase = 283;
                                break;
                            }
                        }
                    case (int)GrupoEtario.AdultoMayor:
                        {
                            GrupoBase = 286;
                            break;
                        }
                    default:
                        {
                            GrupoBase = 0;
                            break;
                        }
                }
                var data = new EsoAntecedentesDal().ObtenerFechasCuidadosPreventivos(PersonId);
                var Comentarios = new EsoAntecedentesDal().ObtenerComentariosCuidadosPreventivos(PersonId);
                foreach (var F in data)
                {
                    F.Listado = new EsoAntecedentesDal().ObtenerListadoCuidadosPreventivos(GrupoBase, PersonId, F.FechaServicio);

                    foreach (var obj in F.Listado)
                    {
                        var newGrupo = obj.GrupoId.ToString() + obj.ParameterId.ToString();
                        int Group = int.Parse(newGrupo);
                        var find = Comentarios.Find(x => x.GrupoId == Group && x.ParametroId == obj.ParameterId);
                        if (find != null)
                        {
                            obj.DataComentario = find;
                        }
                    }

                }



                
                return data;
            }
            catch (Exception e)
            {
                return new List<EsoCuidadosPreventivosFechas>();
            }
        }

        public int ObtenerIdGrupoEtarioDePaciente(int _edad)
        {
            try
            {
                if (_edad <= 12)
                {
                    return 4;
                }
                else if (13 <= _edad && _edad <= 17)
                {
                    return 2;
                }
                else if (18 <= _edad && _edad <= 64)
                {
                    return 1;
                }
                else
                {
                    return 3;
                }
            }
            catch (Exception e)
            {
                return 0;
            }

        }

        public MessageCustom SaveDataNinio(BoardGenerales board, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {
                    var data = board.DataNinioCustom;
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(board.EsoAntecedentesPadre, userId, data.v_PersonId, GrupoEtario, nodeId);


                    NinioBE objNinio = new NinioBE();
                    //
                    #region Fuente
                    objNinio.v_PersonId = data.v_PersonId;
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

                    #endregion
                    //
                    var result = new EsoAntecedentesDal().SaveDataNinio(objNinio, userId, nodeId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Id = result;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Message = "Los datos se guardaron correctamente.";
                    }
                    ts.Complete();
                }


                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar.";
                return _MessageCustom;
            }
            
        }

        public MessageCustom UpdateDataNinio(BoardGenerales data, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();

            try
            {
                using (var ts = new TransactionScope())
                {
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.DataNinioCustom.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(data.EsoAntecedentesPadre, userId, data.DataNinioCustom.v_PersonId, GrupoEtario, nodeId);

                    var result = new EsoAntecedentesDal().UpdateDataNinio(data.DataNinioCustom, userId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se actualizaron correctamente.";
                    }
                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al actualizar, por favor vuelva a intentar.";
                return _MessageCustom;
            }            
        }

        public MessageCustom SaveDataAdolescente(BoardGenerales board, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {
                    var data = board.DataAdolescenteCustom;
                    int GrupoEtario = 0;
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(board.EsoAntecedentesPadre, userId, data.v_PersonId, GrupoEtario, nodeId);

                    AdolescenteBE objAdolescente = new AdolescenteBE();

                    #region Fuente
                    objAdolescente.v_PersonId = data.v_PersonId;
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

                    #endregion

                    var result = new EsoAntecedentesDal().SaveDataAdolescente(objAdolescente, userId, nodeId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se guardaron correctamente.";
                    }
                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar.";
                return _MessageCustom;
            }
            
        }

        public MessageCustom UpdateDataAdolescente(BoardGenerales data, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();

            try
            {
                using (var ts = new TransactionScope())
                {
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.DataAdolescenteCustom.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(data.EsoAntecedentesPadre, userId, data.DataAdolescenteCustom.v_PersonId, GrupoEtario, nodeId);


                    var result = new EsoAntecedentesDal().UpdateDataAdolescente(data.DataAdolescenteCustom, userId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se actualizaron correctamente.";
                    }
                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al actualizar, por favor vuelva a intentar.";
                return _MessageCustom;
            }

        }

        public MessageCustom SaveDataAdulto(BoardGenerales board, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {
                    
                    var data = board.DataAdultoCustom;
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(board.EsoAntecedentesPadre, userId, data.v_PersonId, GrupoEtario, nodeId);

                    AdultoBE objAdulto = new AdultoBE();

                    #region Fuente
                    objAdulto.v_PersonId = data.v_PersonId;
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
                    #endregion


                    var result = new EsoAntecedentesDal().SaveDataAdulto(objAdulto, userId, nodeId);
                    if (result == null)
                    {
                        _MessageCustom.Error = true;
                        _MessageCustom.Status = (int)StatusHttp.BadRequest;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar.";
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se guardaron correctamente.";
                    }
                    ts.Complete();
                    
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar.";
                return _MessageCustom;
            }
            
        }

        public MessageCustom UpdateDataAdulto(BoardGenerales data, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();

            try
            {
                using (var ts = new TransactionScope())
                {
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.DataAdultoCustom.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(data.EsoAntecedentesPadre, userId, data.DataAdultoCustom.v_PersonId, GrupoEtario, nodeId);


                    NinioBE _NinioBE = new NinioBE();
                    var result = new EsoAntecedentesDal().UpdateDataAdulto(data.DataAdultoCustom, userId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se actualizaron correctamente.";
                    }

                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al actualizar, por favor vuelva a intentar.";
                return _MessageCustom;
            }


        }

        public MessageCustom SaveDataAdultoMayor(BoardGenerales board, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                using (var ts = new TransactionScope())
                {

                    var data = board.DataAdultoMayorCustom;

                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(board.EsoAntecedentesPadre, userId, data.v_PersonId, GrupoEtario, nodeId);


                    AdultoMayorBE objAdultoMayor = new AdultoMayorBE();

                    #region Fuente
                    objAdultoMayor.v_PersonId = data.v_PersonId;
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
                    #endregion

                    var result = new EsoAntecedentesDal().SaveDataAdultoMayor(objAdultoMayor, userId, nodeId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se guardaron correctamente.";
                    }
                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al grabar, por favor vuelva a intentar.";
                return _MessageCustom;
            }
            
        }

        public MessageCustom UpdateDataAdultoMayor(BoardGenerales data, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();

            try
            {

                using (var ts = new TransactionScope())
                {
                    DatabaseContext ctx = new DatabaseContext();
                    var objPacient = ctx.Person.Where(x => x.v_PersonId == data.DataAdultoMayorCustom.v_PersonId).FirstOrDefault();
                    int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                    int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);
                    bool resultado = new EsoAntecedentesDal().GuardarAntecedenteAsistencial(data.EsoAntecedentesPadre, userId, data.DataAdultoMayorCustom.v_PersonId, GrupoEtario, nodeId);


                    var result = new EsoAntecedentesDal().UpdateDataAdultoMayor(data.DataAdultoMayorCustom, userId);
                    if (result == null)
                    {
                        throw new Exception("");
                    }
                    else
                    {
                        _MessageCustom.Error = false;
                        _MessageCustom.Status = (int)StatusHttp.Ok;
                        _MessageCustom.Id = result;
                        _MessageCustom.Message = "Los datos se actualizaron correctamente.";
                    }

                    

                    ts.Complete();
                }
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error al actualizar, por favor vuelva a intentar.";
                return _MessageCustom;
            }


            
        }

        public ServiceList GetServicePersonData(string pstrServiceId)
        {
            return new ServiceDal().GetServicePersonData(pstrServiceId);
        }

        public BoardGenerales GetDataGeneralEtario(string personId)
        {
            DatabaseContext ctx = new DatabaseContext();
            var objPacient = ctx.Person.Where(x => x.v_PersonId == personId).FirstOrDefault();
            int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
            //int grupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);

            BoardGenerales _BoardGenerales = new BoardGenerales();
            if (Edad < 13)
            {
                _BoardGenerales.DataNinioCustom = new EsoAntecedentesDal().GetNinio(personId);
            }
            else if (13 <= Edad && Edad <= 17)
            {
                _BoardGenerales.DataAdolescenteCustom = new EsoAntecedentesDal().GetAdolescente(personId);
            }
            else if (18 <= Edad && Edad <= 64)
            {
                _BoardGenerales.DataAdultoCustom = new EsoAntecedentesDal().GetAdulto(personId);
            }
            else
            {
                _BoardGenerales.DataAdultoCustom = new EsoAntecedentesDal().GetAdultoMayor(personId);
            }




            return _BoardGenerales;
        }

        public MessageCustom SaveCuidadosPreventivos(EsoCuidadosPreventivosFechas data, string personId, int userId, int nodeId)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            var result = new EsoAntecedentesDal().SaveCuidadosPreventivos(data, personId, userId, nodeId);
            if (!result)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error, por favor vuelva a intentar.";
            }
            else {
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                _MessageCustom.Message = "Se guardaron los cambios correctamente.";
            }

            return _MessageCustom;
        }
    }
}
