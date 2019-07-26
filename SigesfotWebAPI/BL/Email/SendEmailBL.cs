using BE.Message;
using BE.ServiceOrder;
using DAL.Service;
using DAL.ServiceOrder;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static BE.Common.Enumeratores;

namespace BL.Email
{
    public class SendEmailBL
    {
        public static MessageCustom SendEmail(EmailModel model)
        {
            MessageCustom msg = new MessageCustom();

            try
            {
                List<string> ListPath = new List<string>();
                var MedicalCenter = new ServiceDal().GetInfoMedicalCenter();
                var ServiceOrder = ServiceOrderDal.GetOrganizationByServiceOrderId(model.ServiceOrderId);
                var ListServiceOrderDetail = ServiceOrderDal.GetServicesOrderDetail(model.ServiceOrderId);
                var ruta = HttpContext.Current.Server.MapPath("~/" + System.Configuration.ConfigurationManager.AppSettings["rutaReporteServiceOrder"]);
                var pathFile = string.Format("{0}.pdf", Path.Combine(ruta, model.ServiceOrderId + "-" + "Report"));
                if (model.TypeEmail == (int)TypeEmail.Ordenservicio)//OrdenServicio
                {

                    var result = Report_ServiceOrderConsolidate.GenerateOrderService(ListServiceOrderDetail, MedicalCenter, ServiceOrder, pathFile);
                    if (!result)
                    {
                        throw new Exception("Sucedió un error generando el reporte, por favor vuelva a intentar");
                    }

                    ListPath.Add(pathFile);
                }
                else
                {
                    var result = Report_ServiceOrderConsolidate.GenerateCotizacion(ListServiceOrderDetail, MedicalCenter, ServiceOrder, pathFile);
                    if (!result)
                    {
                        throw new Exception("Sucedió un error generando el reporte, por favor vuelva a intentar");
                    }

                    ListPath.Add(pathFile);
                }
                
                using (MailMessage mm = new MailMessage("jasondev2019@gmail.com", model.To))
                {
                    mm.Subject = model.Subject;
                    mm.Body = model.Body;
                    foreach (var path in ListPath)
                    {
                        mm.Attachments.Add(new Attachment(path));
                    }


                    mm.IsBodyHtml = false;
                    using (SmtpClient smtp = new SmtpClient())
                    {
                        smtp.Host = "smtp.gmail.com";
                        smtp.EnableSsl = true;
                        NetworkCredential NetworkCred = new NetworkCredential("jasondev2019@gmail.com", "74390363991646704");
                        smtp.UseDefaultCredentials = true;
                        smtp.Credentials = NetworkCred;
                        smtp.Port = 587;
                        smtp.Send(mm);
                    }
                }

                msg.Error = false;
                msg.Status = (int)HttpStatusCode.OK;
                msg.Message = "El mensaje se envió correctamente";
                return msg;
            }
            catch (Exception ex)
            {
                msg.Error = false;
                msg.Status = (int)HttpStatusCode.OK;
                msg.Message = ex.Message;
                return msg;
            }
        }
    }
}
