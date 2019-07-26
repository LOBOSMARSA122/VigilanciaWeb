using BE.Categoria;
using BE.Component;
using BE.Message;
using BL.Common;
using BL.Consultorio;
using BL.Service;
using DAL.AdditionalExam;
using DAL.Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static BE.Common.Enumeratores;

namespace BL.AdditionalExam
{
    public class AdditionalExamBL
    {
        public List<AdditionalExamUpdate> GetAdditionalExamForUpdateByServiceId(string serviceId, int userId)
        {
            return new AdditionalExamDal().GetAdditionalExamForUpdateByServiceId(serviceId, userId);
        }

        public MessageCustom UpdateComponentAdditionalExam(List<string> NewComponentId, List<string> _AdditionalExamId, int userId, string _serviceId, string comentario)
        {
            MessageCustom _MessageCustom = new MessageCustom();
            var result = new AdditionalExamDal().UpdateComponentAdditionalExam(NewComponentId, _AdditionalExamId, userId);


            if (result)
            {
                comentario = comentario == null ? "SIN COMENTARIOS" : comentario;
                string CMP = "SIN-PROFESIONAL";
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioExamAdicional"]);
                var rutaWeb = string.Format("{0}.pdf", Path.Combine(_serviceId + "-" + "ORDEN-EX-MED-ADICI-SIN-PROFESIONAL"));
                string pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-SIN-PROFESIONAL"));
                var datosGrabo = new ServiceBl().DevolverDatosUsuarioFirma(userId);
                if (datosGrabo != null)
                {
                    CMP = datosGrabo.CMP;
                    pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-" + datosGrabo.CMP));
                    rutaWeb = string.Format("{0}.pdf", Path.Combine(_serviceId + "-" + "ORDEN-EX-MED-ADICI-" + datosGrabo.CMP));
                }



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


                var MedicalCenter = new ServiceBl().GetInfoMedicalCenter();
                var DatosPaciente = new PacientBL().DevolverDatosPaciente(_serviceId);

                new PrintAdditionalExam().GenerateAdditionalexam(pathFile, MedicalCenter, DatosPaciente, datosGrabo, comentario, DataSource, ListadditExam);


                _MessageCustom.Error = false;
                _MessageCustom.Status = 200;
                _MessageCustom.Message = "Se actualizó correctamente";
                _MessageCustom.Id = rutaWeb;
            }
            else
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = 500;
                _MessageCustom.Message = "Sucedió un error, vuelva a intentar";
            }

            return _MessageCustom;
        }

        public MessageCustom SaveDeletedAdditionalExam(string additionalExamId, int userId, string comentario, string _serviceId)
        {
            comentario = comentario == null ? "SIN COMENTARIOS" : comentario;
            var result = new AdditionalExamDal().DeleteAdditionalExam(additionalExamId, userId);
            MessageCustom _MessageCustom = new MessageCustom();
            if (result)
            {
                comentario = comentario == null ? "SIN COMENTARIOS" : comentario;
                string CMP = "SIN-PROFESIONAL";
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioExamAdicional"]);
                var rutaWeb = string.Format("{0}.pdf", Path.Combine(_serviceId + "-" + "ORDEN-EX-MED-ADICI-SIN-PROFESIONAL"));
                string pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-SIN-PROFESIONAL"));
                var datosGrabo = new ServiceBl().DevolverDatosUsuarioFirma(userId);
                if (datosGrabo != null)
                {
                    CMP = datosGrabo.CMP;
                    pathFile = string.Format("{0}.pdf", Path.Combine(ruta, _serviceId + "-" + "ORDEN-EX-MED-ADICI-" + datosGrabo.CMP));
                    rutaWeb = string.Format("{0}.pdf", Path.Combine(_serviceId + "-" + "ORDEN-EX-MED-ADICI-" + datosGrabo.CMP));
                }



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


                var MedicalCenter = new ServiceBl().GetInfoMedicalCenter();
                var DatosPaciente = new PacientBL().DevolverDatosPaciente(_serviceId);

                new PrintAdditionalExam().GenerateAdditionalexam(pathFile, MedicalCenter, DatosPaciente, datosGrabo, comentario, DataSource, ListadditExam);





                _MessageCustom.Error = false;
                _MessageCustom.Status = 200;
                _MessageCustom.Message = "Se eliminó correctamente";
                _MessageCustom.Id = rutaWeb;
            }
            else
            {
                _MessageCustom.Error = true;
                _MessageCustom.Status = 500;
                _MessageCustom.Message = "Sucedió un error, vuelva a intentar";
            }
            return _MessageCustom;
        }

        public List<Categoria> GetAdditionalExamByServiceId(string serviceId)
        {
            List<string> ComponentAdditionalList = new List<string>();
            List<string> ComponentNewService = new List<string>();
            var ListAdditionalExams = new AdditionalExamDal().GetAdditionalExamByServiceId(serviceId);

            foreach (var obj in ListAdditionalExams)
            {
                ComponentAdditionalList.Add(obj.ComponentId);
                if (obj.IsNewService == (int)SiNo.Si)
                {
                    ComponentNewService.Add(obj.ComponentId);
                }
            }

            List<Categoria> DataSource = new List<Categoria>();

            foreach (var componentId in ComponentAdditionalList)
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
            foreach (var item in ListAdditionalExams)
            {
                foreach (var data in DataSource)
                {
                    foreach (var comp in data.Componentes)
                    {
                        if (comp.v_ComponentId == item.ComponentId)
                        {
                            if (item.IsNewService == 1)
                            {
                                comp.i_NewService = 1;
                            }
                            else
                            {
                                comp.i_NewService = 0;
                            }
                        }
                    }
                    
                }
            }
            return DataSource;
        }
    }
}
