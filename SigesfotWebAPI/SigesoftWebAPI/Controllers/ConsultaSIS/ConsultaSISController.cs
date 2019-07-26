using OpenQA.Selenium;
using Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System.Web.Http;
using System.Net.Http;
using System.IO;
using System.Net;
using System;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using BE.RUC;
using BE.Message;

namespace SigesoftWebAPI.Controllers.ConsultaSIS
{

    public class ConsultaSISController : ApiController
    {
        public static IWebDriver driver { get; set; }

        [HttpGet]
        public IHttpActionResult GetCapcha()
        {
            try
            {
                var options = new PhantomJSOptions();
                options.AddAdditionalCapability("IsJavaScriptEnabled", true);

                //aqui poner la ruta completa del archivo phantomjs.exe que está en \App_Data\Component
                string rutaPhantonExe = System.Configuration.ConfigurationManager.AppSettings["directorioPhantonExe"];
                string ruta = string.Format(@"{0}", rutaPhantonExe);
                var driverService = PhantomJSDriverService.CreateDefaultService(ruta, "phantomjs.exe");

                driverService.HideCommandPromptWindow = true;
                driver = new PhantomJSDriver(driverService, options);
                driver.Navigate().GoToUrl("http://app.sis.gob.pe/SisConsultaEnLinea/Consulta/frmConsultaEnLinea.aspx");
                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var imgGuid = driver.PageSource.Substring(driver.PageSource.IndexOf("?guid="), 42).Replace("?guid=", "");
                string path = string.Format("http://app.sis.gob.pe/SisConsultaEnLinea/Consulta/CaptchaImage.aspx?guid=" + imgGuid);
                var img = ReadCapcha(path);
                var selectTipoBusqueda = new SelectElement(driver.FindElement(By.Id("cboTipoBusqueda")));
                selectTipoBusqueda.SelectByValue("2");
                var selectTipoDocumento = new SelectElement(driver.FindElement(By.Id("cboTipoDocumento")));
                selectTipoDocumento.SelectByValue("1"); //1 DNI; 2 carnet ext.


                MemoryStream ms = new MemoryStream(converterDemo(img));
                StreamReader reader = new StreamReader(ms);
                
                string response = reader.ReadToEnd();
                //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new StreamContent(ms);
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                MessageCustom _msg = new MessageCustom();
                _msg.Error = false;
                _msg.Status = (int)HttpStatusCode.NoContent;
                _msg.Message = "El paciente no se encontró en la BD. Ingrese el codigo captcha para hacer una busqueda en el SIS";
                _msg.Id = Convert.ToBase64String(ms.ToArray());
                return Ok(_msg);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetData(string captcha, string dni)
        {
            RootObjectSIS response = new RootObjectSIS();
            
            try
            {
                var txtCapcha = driver.FindElement(By.Name("CaptchaControl1"));
                txtCapcha.SendKeys(captcha);

                var txtNroDocumento = driver.FindElement(By.Id("txtNroDocumento"));
                txtNroDocumento.SendKeys(dni);

                var btnBuscar = driver.FindElement(By.Name("btnConsultar"));
                btnBuscar.Click();

                var existeData = driver.FindElement(By.XPath("//*[@id=\"lblMensaje\"]")).Text == string.Empty;
                ResultSIS _ResultSIS = new ResultSIS();
                if (existeData)
                {
                    

                    _ResultSIS.ApellidoMaterno = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/ tbody/tr[2]/td[6]")).First().Text;
                    _ResultSIS.ApellidoPaterno = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/tbody/tr[2]/td[5]")).First().Text;
                    _ResultSIS.NroAfiliacion = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/tbody/tr[2]/td[3]")).First().Text;
                    _ResultSIS.Nombres = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/ tbody/tr[2]/td[7]")).First().Text;
                    _ResultSIS.TipoAsegurado = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/tbody/tr[2]/td[8]")).First().Text;
                    _ResultSIS.Estado = driver.FindElements(By.XPath("//*[@id=\"dgConsulta\"]/tbody/tr[2]/td[9]")).First().Text;

                    response.BaseDatos = false;
                    response.Error = false;
                    response.Status = (int)HttpStatusCode.OK;
                    response.Result = _ResultSIS;

                }
                else
                {
                    response.Error = true;
                    response.Status = (int)HttpStatusCode.NoContent;
                    response.Message = "El paciente no se encuentra registrado en el SIS.";
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Error = true;
                response.Status = (int)HttpStatusCode.BadRequest;
                response.Message = "Codigo captcha incorrecto.";
                return Ok(response);
            }
        }

        public static byte[] converterDemo(Image x)
        {
            ImageConverter _imageConverter = new ImageConverter();
            byte[] xByte = (byte[])_imageConverter.ConvertTo(x, typeof(byte[]));
            return xByte;
        }

        private Image ReadCapcha(string path)
        {
            try
            {
                var myWebRequest = (HttpWebRequest)WebRequest.Create(path);
                myWebRequest.Proxy = null;
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;

                using (var myWebResponse = (HttpWebResponse)myWebRequest.GetResponse())
                {
                    using (var myImgStream = myWebResponse.GetResponseStream())
                    {
                        return myImgStream != null ? Image.FromStream(myImgStream) : null;
                    }
                }
            }
            catch (Exception ex)
            {
                return null;
            }

        }
    }


}
