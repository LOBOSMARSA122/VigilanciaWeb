using BE.Categoria;
using BE.Common;
using BE.Component;
using BE.Message;
using BE.Sigesoft;
using BL.Common;
using BL.Service;
using DAL;
using DAL.Service;
using NetPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static BE.Common.Enumeratores;

namespace BL.Consultorio
{
    public class ConsultorioBL
    {
        public BoardExamsCustom CallingPacient(BoardExamsCustom data)
        {
            MessageCustom _Message = new MessageCustom();

            List<Dropdownlist> ListaCetegorias = new SystemParameterBL().GetParametroByGrupoId(116);
            var ListaExamenesPrevios = new SystemParameterBL().GetParametroByGrupoId(306).Find(p => p.Value2 == data.CategoryId);

            if (ListaExamenesPrevios != null)
            {
                var consultorioPrevio = int.Parse(ListaExamenesPrevios.Field);

                if (consultorioPrevio == -1)
                {
                    var examenesNoCulminados = new ServiceBl().GetServiceComponentsCulminados(data.ServiceId);
                    if (examenesNoCulminados == null)
                    {
                        _Message.Error = true;
                        _Message.Status = 400;
                        _Message.Message = "Sucedio un error generando las consultas, por favor refresque y vuelva a intentar.";
                        data.Message = _Message;
                        return data;
                    }
                    var exam = examenesNoCulminados.FindAll(p => p.i_CategoryId != data.CategoryId);

                    if (exam.Count != 0)
                    {
                        _Message.Error = true;
                        _Message.Status = 200;
                        _Message.Message = "Este paciente debe primero CULIMINAR TODOS los examenes anteriores.";
                        data.Message = _Message;
                        return data;
                    }
                }

                var listaExamenesProtocolo = new ServiceBl().GetServiceComponents(data.ServiceId).Find(p => p.i_CategoryId == consultorioPrevio);
                if (listaExamenesProtocolo == null)
                {
                    _Message.Error = true;
                    _Message.Status = 400;
                    _Message.Message = "Sucedio un error generando las consultas, por favor refresque y vuelva a intentar.";
                    data.Message = _Message;
                    return data;
                }
                if (listaExamenesProtocolo != null)
                {
                    var examenesNoCulminados = new ServiceBl().GetServiceComponentsCulminados(data.ServiceId);
                    if (examenesNoCulminados == null)
                    {
                        _Message.Error = true;
                        _Message.Status = 400;
                        _Message.Message = "Sucedio un error generando las consultas, por favor refresque y vuelva a intentar.";
                        data.Message = _Message;
                        return data;
                    }
                    var result = examenesNoCulminados.Find(p => p.i_CategoryId == consultorioPrevio);

                    if (result != null)
                    {
                        _Message.Error = true;
                        _Message.Status = 200;
                        int field = int.Parse(ListaExamenesPrevios.Field);
                        _Message.Message = "Este paciente debe primero CULIMINAR  el examen " + ListaCetegorias.Find(p => p.Id == field).Value;
                        data.Message = _Message;
                        return data;
                    }
                }
            }

            if (data.Piso != -1)
            {
                var ResultPiso = new ServiceBl().PermitirLlamar(data.ServiceId, data.Piso);
                if (!ResultPiso)
                {
                    _Message.Error = true;
                    _Message.Status = 200;
                    _Message.Message = "El Paciente tiene consultorios por culminar, antes de ser llamado por este. Verifíquelo en unos minutos";
                    data.Message = _Message;
                    return data;
                }
            }

            if (data.ServiceStatusId == (int)ServiceStatus.EsperandoAptitud)
            {
                _Message.Error = true;
                _Message.Status = 200;
                _Message.Message = "Este paciente ya tiene el servicio en espera de Aptitud, no puede ser llamado.";
                data.Message = _Message;
                return data;
            }
            var oServiceComponentList = new ServiceBl().GetServiceComponentByCategoryId(data.CategoryId, data.ServiceId);
            List<string> _ServiceComponentId = new List<string>();
            foreach (var item in oServiceComponentList)
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }
            bool resultOff = new ServiceDal().UpdateServiceComponentOfficeLlamando(_ServiceComponentId, data.Oficina);
            if (!resultOff)
            {
                _Message.Error = true;
                _Message.Status = 500;
                _Message.Message = "Sucedio un error actualizando la llamada, vuelva a intentar por favor.";
                data.Message = _Message;
                return data;
            }
            _Message.Error = false;
            _Message.Status = 200;
            data.Message = _Message;
            return data;
        }

        public MessageCustom EliminarExamen(BoardExamsCustom data, int userId)
        {
            MessageCustom _Message = new MessageCustom();
            var _auxiliaryExams = new List<ServiceComponentList>();
            if (data.CategoryId == -1)
            {
                ServiceComponentList auxiliaryExam = new ServiceComponentList();
                auxiliaryExam.v_ServiceComponentId = data.ServicecomponentId;
                _auxiliaryExams.Add(auxiliaryExam);
            }
            else
            {
                var oServiceComponentList = new ServiceBl().GetServiceComponentByCategoryId(data.CategoryId, data.ServiceId);
                if (oServiceComponentList == null)
                {
                    _Message.Error = true;
                    _Message.Status = (int)StatusHttp.BadRequest;
                    _Message.Message = "Sucedió un error al consultar los componentes del servicio.";
                    return _Message;
                }
                foreach (var scid in oServiceComponentList)
                {
                    ServiceComponentList auxiliaryExam = new ServiceComponentList();
                    auxiliaryExam.v_ServiceComponentId = scid.v_ServiceComponentId;
                    _auxiliaryExams.Add(auxiliaryExam);
                }

                
            }
            bool result = new ServiceBl().UpdateAdditionalExam(_auxiliaryExams, data.ServiceId, (int)SiNo.No, userId);
            if (!result)
            {
                _Message.Error = true;
                _Message.Status = (int)StatusHttp.BadRequest;
                _Message.Message = "Sucedió un error al actualizar los examenes, no se guardó ningun cambio.";
                return _Message;
            }

            _Message.Error = false;
            _Message.Status = (int)StatusHttp.Ok;
            _Message.Message = "Los cambios se guardaron correctamente.";
            return _Message;
        }

        public List<Categoria> GetAdditionalExams(BoardExamsCustom data)
        {
            
            var listAllServicecomponents = new ServiceDal().GetAllComponents(data.TipoBusqueda, data.Value);
            var ListaFinal = new List<Categoria>();

            return listAllServicecomponents;
        }

        public bool BuscarCoincidencia(string serviceId, string componentId)
        {
            var listServicecomponents = new ServiceBl().GetServiceComponents(serviceId);

            var find = listServicecomponents.Find(x => x.v_ComponentId == componentId);
            if (find != null)
            {
                return true;
            }
            return false;
        }

        public MessageCustom SaveadditionalExams(List<AdditionalExamCustom> listExams, int userId, int nodeId)
        {
            try
            {
                MessageCustom _MessageCustom = new MessageCustom();
                var resul = new ServiceBl().AddAdditionalExam(listExams, userId, nodeId);

                if (resul)
                {
                    _MessageCustom.Error = false;
                    _MessageCustom.Status = (int)StatusHttp.Ok;
                    _MessageCustom.Message = "Los exámenes se agregaron correctamente";

                }
                else
                {
                    _MessageCustom.Error = true;
                    _MessageCustom.Status = (int)StatusHttp.BadRequest;
                    _MessageCustom.Message = "Sucedió un error y no se agregaron los exámenes adicionales";
                }

                #region Para imprimir los exámenes
                var _serviceId = listExams[0].ServiceId;
                var datosGrabo = new ServiceBl().DevolverDatosUsuarioFirma(userId);
                var MedicalCenter = new ServiceBl().GetInfoMedicalCenter();
                var DatosPaciente = new PacientBL().DevolverDatosPaciente(_serviceId);
                List<Categoria> AdditionalExam = new List<Categoria>();
                List<Categoria> DataSource = new List<Categoria>();
                List<string> ComponentList = new List<string>();
                var ListadditExam = new ServiceDal().GetAdditionalExamByServiceId_all(_serviceId, userId);

                foreach (var componenyId in ListadditExam)
                {
                    ComponentList.Add(componenyId.ComponentId);
                }

                foreach (var componentId in ComponentList)
                {
                    var ListServiceComponent = new ServiceDal().GetAllComponents((int)TipoBusqueda.ComponentId, componentId);

                    Categoria categoria = DataSource.Find(x => x.i_CategoryId == ListServiceComponent[0].i_CategoryId);
                    if (categoria != null)
                    {
                        List<ComponentDetailList> componentDetail = new List<ComponentDetailList>();
                        componentDetail = ListServiceComponent[0].Componentes;
                        DataSource.Find(x => x.i_CategoryId == ListServiceComponent[0].i_CategoryId).Componentes.AddRange(componentDetail);
                    }
                    else
                    {
                        DataSource.AddRange(ListServiceComponent);
                    }
                }

                
                string CMP = "SIN-PROFESIONAL";
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioExamAdicional"]);
                var rutaBasura = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioBasura"]);
                string pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-SIN-PROFESIONAL"));
                if (datosGrabo != null)
                {
                    if (datosGrabo.CMP != null)
                    {
                        CMP = datosGrabo.CMP;
                        pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-" + datosGrabo.CMP));
                    }

                }
                new PrintAdditionalExam().GenerateAdditionalexam(pathFile, MedicalCenter, DatosPaciente, datosGrabo, listExams[0].Commentary, DataSource, ListadditExam);
                List<string> pdfList = new List<string>();
                pdfList.Add(pathFile);
                MergeExPDF _mergeExPDF = new MergeExPDF();
                _mergeExPDF.FilesName = pdfList;
                _mergeExPDF.DestinationFile = string.Format("{0}.pdf", Path.Combine(rutaBasura, _serviceId + "-" + "ORDEN-EX-MED-ADICI-" + CMP));
                _mergeExPDF.Execute();
                #endregion
                _MessageCustom.Id = string.Format("{0}.pdf", Path.Combine(_serviceId + "-" + "ORDEN-EX-MED-ADICI-" + CMP));
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public bool LiberarPaciente(int Category, string ServiceId, string CategoryName, string CalendarId)
        {
            var serviceComponent = new ServiceDal().GetServiceComponentByCategoryId(Category, ServiceId);
            int statusAntiguo = serviceComponent[0].i_ServiceComponentStatusId.Value;
            List<string> _ServiceComponentId = new List<string>();
            foreach (var item in serviceComponent)
            {
                _ServiceComponentId.Add(item.v_ServiceComponentId);
            }

            DatabaseContext ctx = new DatabaseContext();

            //var objCalendar = ctx.Calendar.Where(x => x.v_CalendarId == CalendarId).FirstOrDefault();
            //objCalendar.i_LineStatusId = 2;
            //ctx.SaveChanges();
            if (CategoryName == "LABORATORIO")
            {
                return new ServiceDal().LiberarPacientelaboratorio(_ServiceComponentId, statusAntiguo);
            }
            else
            {
                return new ServiceDal().LiberarPaciente(_ServiceComponentId);
            }


            
        }

        public MessageCustom ReImprimirAdicional(int userId, string serviceId)
        {

            MessageCustom _MessageCustom = new MessageCustom();
            try
            {
                MergeExPDF _mergeExPDF = new MergeExPDF();
                #region BuscarPDF
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioExamAdicional"]);
                var rutaBasura = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioBasura"]);
                var datosGrabo = new ServiceDal().DevolverDatosUsuarioFirma(userId);
                var CMP = datosGrabo == null ? "SIN-PROFESIONAL" : datosGrabo.CMP;
                List<string> pdfList = new List<string>();
                pdfList.Add(string.Format("{0}.pdf", Path.Combine(ruta, serviceId + "-" + "ORDEN-EX-MED-ADICI-" + CMP)));
                _mergeExPDF.FilesName = pdfList;
                _mergeExPDF.DestinationFile = string.Format("{0}.pdf", Path.Combine(rutaBasura, "REIMPRESO-" + serviceId + "-" + CMP));
                _mergeExPDF.Execute();

                _MessageCustom.Id = string.Format("{0}.pdf", Path.Combine("REIMPRESO-" + serviceId + "-" + CMP));
                _MessageCustom.Error = false;
                _MessageCustom.Status = 200;
                #endregion
                return _MessageCustom;
            }
            catch (Exception ex)
            {
                _MessageCustom.Message = "No existen PDFs por reimprimir.";
                _MessageCustom.Error = true;
                _MessageCustom.Status = 500;
                return _MessageCustom;
            }
            
        }
    }
}
