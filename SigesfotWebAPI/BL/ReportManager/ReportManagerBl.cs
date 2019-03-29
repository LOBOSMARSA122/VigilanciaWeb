using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using BE.Common;
using BE.ReportManager;
using DAL.ReportManager;
using DAL.Sigesoft;
using NetPdf;

namespace BL.ReportManager
{
   public class ReportManagerBl
    {
        public List<ReportManagerBe> ReportManagerByServiceId(string serviceId)
        {
            var organizationId = new ReportManagerDal().GetEmpresaId(serviceId);
            var ordenReporte = new ReportManagerDal().GetOrdenReportes(organizationId);
            var components = new ReportManagerDal().GetComponentsByServiceId(serviceId);

            var list = new List<ReportManagerBe>(); ;
            foreach (var orden in ordenReporte)
            {
                var result = components.Find(p => p.ComponentId == orden.ComponenteId);
                if (result != null)
                {
                    var oReportManagerBe = new ReportManagerBe();
                    oReportManagerBe.ServiceId = serviceId;
                    oReportManagerBe.ComponentId = orden.ComponenteId;
                    oReportManagerBe.ComponentName = orden.NombreReporte;

                    list.Add(oReportManagerBe);
                }
            }
            return list;
        }

        string _ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["directorioESO"]);
        private List<string> _filesNameToMerge = new List<string>();
        private MergeExPDF _mergeExPDF = new MergeExPDF();
        public string BuildReport(List<ComponentsServiceId> data)
        {
            var serviceId = data[0].ServiceId;

            foreach (var component in data)
            {
                switch (component.ComponentId)
                {
                    case BE.Sigesoft.Common.ACCIDENTES_DE_TRABAJO_F1:
                        GenerateAccidentesTrabajoF1(serviceId, component.ComponentId);
                        break;

                    case BE.Sigesoft.Common.ACCIDENTES_DE_TRABAJO_F2:
                        GenerateAccidentesTrabajoF2(serviceId, component.ComponentId);
                        break;
                }
            }

            var reportsPdf = _filesNameToMerge.ToList();
            _mergeExPDF.FilesName = reportsPdf;
            _mergeExPDF.DestinationFile = _ruta + "/" + serviceId + ".pdf";
            _mergeExPDF.Execute();

            return serviceId;
        }

        private void GenerateAccidentesTrabajoF1(string serviceId, string componentId)
        {
            var serviceComponents = new SigesoftDal().GetServiceComponentsReport(serviceId);
            var MedicalCenter = new SigesoftDal().GetInfoMedicalCenter();
            var pathFile = string.Format("{0}.pdf", Path.Combine(_ruta, serviceId + "-" + componentId));

            AccidentesTrabajo_F1.CreateAccidentesTrabajoF1(serviceComponents, MedicalCenter, pathFile);
            _filesNameToMerge.Add($"{Path.Combine(_ruta, serviceId + "-" + componentId)}.pdf");
        }

        private void GenerateAccidentesTrabajoF2(string serviceId, string componentId)
        {
            var serviceComponents = new SigesoftDal().GetServiceComponentsReport(serviceId);
            var MedicalCenter = new SigesoftDal().GetInfoMedicalCenter();
            var pathFile = string.Format("{0}.pdf", Path.Combine(_ruta, serviceId + "-" + componentId));

            AccidentesTrabajo_F2.CreateAccidentesTrabajoF2(serviceComponents, MedicalCenter, pathFile);
            _filesNameToMerge.Add($"{Path.Combine(_ruta, serviceId + "-" + componentId)}.pdf");
        }


    }
}
