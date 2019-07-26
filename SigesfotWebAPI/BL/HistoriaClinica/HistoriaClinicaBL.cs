using BE.Antecedentes;
using BE.Common;
using BE.Message;
using BL.Antecedentes;
using BL.Common;
using BL.Service;
using DAL.Antecedentes;
using DAL.PlanIntegral;
using DAL.Service;
using DAL.Sigesoft;
using NetPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static BE.Common.Enumeratores;

namespace DAL.HistoriaClinica
{
    public class HistoriaClinicaBL
    {
    
        public MessageCustom GenerateHistoriaClinica(string personId, string serviceId)
        {

            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                DatabaseContext ctx = new DatabaseContext();
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioHistoriaClinica"]);
                var rutaBasura = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioBasura"]);
                string pathFile = string.Format("{0}.pdf", Path.Combine(ruta, serviceId + "-" + "HISTORIA-CLINICA"));
                var objPacient = ctx.Person.Where(x => x.v_PersonId == personId).FirstOrDefault();
                int Edad = new PacientBL().GetEdad(objPacient.d_Birthdate.Value);
                int GrupoEtario = ObtenerIdGrupoEtarioDePaciente(Edad);


                List<EsoCuidadosPreventivosComentarios> Comentarios = new EsoAntecedentesDal().ObtenerComentariosCuidadosPreventivos(personId);
                var listaProblema = new PlanIntegralDal().GetProblemaPagedAndFiltered(personId);
                var listPlanIntegral = new PlanIntegralDal().GetPlanIntegral(personId);
                var datosPersonales = new PacientBL().GetDatosPersonalesAtencion(serviceId);
                var datosP = new PacientBL().DevolverDatosPaciente(serviceId);
                int GrupoBase = ObtenerGrupoBase(GrupoEtario, objPacient);
                int Grupo = int.Parse(GrupoBase.ToString() + GrupoEtario.ToString());
                if (Edad <= 12)
                {
                    GrupoEtario = 4;
                    Grupo = 2824;
                }
                else if (13 <= Edad && Edad <= 17)
                {
                    GrupoEtario = 2;
                    Grupo = 2822;
                }
                else if (18 <= Edad && Edad <= 64)
                {
                    GrupoEtario = 1;
                    Grupo = 2821;
                }
                else
                {
                    GrupoEtario = 3;
                    Grupo = 2823;
                }
                var listAntecedentes = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(Grupo, GrupoEtario, personId);
                var datosNin = new EsoAntecedentesDal().DevolverNinio(serviceId);
                var datosAdol = new EsoAntecedentesDal().DevolverAdolescente(serviceId);
                var datosAdul = new EsoAntecedentesDal().DevolverAdulto(serviceId);
                var datosAdulMay = new EsoAntecedentesDal().DevolverAdultoMayor(serviceId);
                List<EsoCuidadosPreventivosFechas> Fechas = new EsoAntecedentesBL().ObtenerFechasCuidadosPreventivos(personId);

                foreach (var F in Fechas)
                {
                    F.Listado = new EsoAntecedentesDal().ObtenerListadoCuidadosPreventivos(GrupoBase, personId, F.FechaServicio);
                }
                if (Fechas.Count > 6)
                    Fechas = Fechas.Skip((Fechas.Count - 6)).ToList();
                var MedicalCenter = new ServiceDal().GetInfoMedicalCenter();

                var listEmb = new EsoAntecedentesDal().GetEmbarazos(personId);

                //primer pdf
                var pathFile2 = string.Format("{0}.pdf", Path.Combine(ruta, serviceId + "-" + "HISTORIA-CLINICA-INTEGRAL"));
                var exams = new SigesoftDal().GetServiceComponentsReport(serviceId);
                var medico = new PacientBL().ObtenerDatosMedicoMedicina(serviceId, Constants.ATENCION_INTEGRAL_ID, Constants.EXAMEN_FISICO_7C_ID);
                var datosGrabo = new ServiceDal().DevolverDatosUsuarioGraboExamen((int)CategoryTypeExam.ExamenFisico, serviceId);
                var diagnosticRepository = new ServiceDal().GetServiceComponentConclusionesDxServiceIdReport(serviceId);
                var medicina = new ServiceDal().GetReceta(serviceId);
                var medicoTratante = new ServiceDal().GetMedicoTratante(serviceId);
                AtencionIntegral.CreateAtencionIntegral(pathFile2, medico, datosP, listAntecedentes, MedicalCenter, exams, datosNin, datosAdol, datosAdul, listEmb, datosAdulMay, diagnosticRepository, medicina, exams, medicoTratante, datosGrabo);
                //////////////////

                if (GrupoEtario == (int)Enumeratores.GrupoEtario.Ninio)
                {
                    
                    Ninio.CreateAtencionNinio(pathFile, listaProblema, listPlanIntegral, datosPersonales, datosP, listAntecedentes, Fechas, MedicalCenter, datosNin, Comentarios);
                }
                else if (GrupoEtario == (int)Enumeratores.GrupoEtario.Adolecente)
                {
                    GrupoBase = 285;
                    if (datosPersonales.Genero.ToUpper() == "MUJER")
                        GrupoBase = 283;
                    var Fechas2 = new EsoAntecedentesDal().ObtenerFechasCuidadosPreventivos(personId);
                    foreach (var F in Fechas2)
                    {
                        F.Listado = new EsoAntecedentesDal().ObtenerListadoCuidadosPreventivos(GrupoBase, personId, F.FechaServicio);
                        foreach (var obj in F.Listado)
                        {
                            var find = Comentarios.Find(x => x.GrupoId == obj.GrupoId && x.ParametroId == obj.ParameterId);
                            if (find != null)
                            {
                                obj.DataComentario = find;
                            }
                        }

                    }
                    listAntecedentes = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(2822, GrupoEtario, personId);
                    AtencionIntegralAdolescente.CreateAtencionIntegral(pathFile, listaProblema, listPlanIntegral, datosPersonales, datosP, listAntecedentes, Fechas2, MedicalCenter, datosAdol, Comentarios);
                }
                else if (GrupoEtario == (int)Enumeratores.GrupoEtario.Adulto)
                {
                    listAntecedentes = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(2821, GrupoEtario, personId);
                    AtencionIntegralAdulto.CreateAtencionIntegral(pathFile, listaProblema, listPlanIntegral, datosPersonales, datosP, listAntecedentes, Fechas, MedicalCenter, datosAdul, listEmb, Comentarios);
                }
                else if (GrupoEtario == (int)Enumeratores.GrupoEtario.AdultoMayor)
                {

                    listAntecedentes = new EsoAntecedentesDal().ObtenerEsoAntecedentesPorGrupoId(2823, GrupoEtario, personId);
                    AtencionIntegralAdultoMayor.CreateAtencionIntegral(pathFile, listaProblema, listPlanIntegral, datosPersonales, datosP, listAntecedentes, Fechas, MedicalCenter, datosAdulMay, listEmb, Comentarios);
                }

                List<string> pdfList = new List<string>();
                
                pdfList.Add(pathFile2);
                pdfList.Add(pathFile);
                MergeExPDF _mergeExPDF = new MergeExPDF();
                _mergeExPDF.FilesName = pdfList;
                _mergeExPDF.DestinationFile = string.Format("{0}.pdf", Path.Combine(rutaBasura, serviceId + "-COPIA-HISTORIA-CLINICA"));
                _mergeExPDF.Execute();

                _MessageCustom.Id = string.Format("{0}.pdf", Path.Combine(serviceId + "-COPIA-HISTORIA-CLINICA"));
                _MessageCustom.Error = false;
                _MessageCustom.Status = (int)StatusHttp.Ok;
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = (int)StatusHttp.BadRequest;
                _MessageCustom.Message = "Sucedió un error, por favor vuelva a intentar.";
                return _MessageCustom;
            }

        }
        public int ObtenerGrupoBase(int _GrupoEtario, PersonDto objPacient)
        {
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

            return GrupoBase;
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
    }
}
