using BE.Message;
using BE.RUC;
using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.ConsultaRUC
{
    public class ConsultaRUCController : ApiController
    {
        public static IWebDriver driver { get; set; }
        private readonly CookieContainer _myCookie;
        [HttpGet]
        public IHttpActionResult GetCaptcha()
        {
            try
            {
                var options = new PhantomJSOptions();
                options.AddAdditionalCapability("IsJavaScriptEnabled", true);

                string rutaPhantonExe = System.Configuration.ConfigurationManager.AppSettings["directorioPhantonExe"];
                string ruta = string.Format(@"{0}", rutaPhantonExe);
                var driverService = PhantomJSDriverService.CreateDefaultService(ruta, "phantomjs.exe");

                driverService.HideCommandPromptWindow = true;
                driver = new PhantomJSDriver(driverService, options);
                driver.Navigate().GoToUrl("http://www.sunat.gob.pe/cl-ti-itmrconsruc/FrameCriterioBusquedaMovil.jsp");

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                var pathImg = driver.FindElement(By.Id("imgCodigo")).GetAttribute("src");
                var myWebRequest = (HttpWebRequest)WebRequest.Create("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image");


                //var imgGuid = pathImg.Replace("http://www.sunat.gob.pe/cl-ti-itmrconsruc/captcha?accion=image&nmagic=", "");
                //string path = string.Format("http://app.sis.gob.pe/SisConsultaEnLinea/Consulta/captcha?accion=image&nmagic=" + imgGuid);

                //var img = ReadCapcha(pathImg);




                //HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                //response.Content = new StreamContent(ms);
                //response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");



                myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";//esto creo que lo puse por gusto :/
                myWebRequest.CookieContainer = _myCookie;
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                myWebRequest.Proxy = null;
                myWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
                myWebRequest.Accept = "text/xml";
                myWebRequest.Method = "POST";

                using (var myWebResponse = myWebRequest.GetResponse())
                {
                    var myImgStream = myWebResponse.GetResponseStream();

                    //return myImgStream != null ? Image.FromStream(myImgStream) : null;

                    MemoryStream ms = new MemoryStream(converterDemo(Image.FromStream(myImgStream)));
                    StreamReader reader = new StreamReader(ms);
                    string response = reader.ReadToEnd();

                    MessageCustom _msg = new MessageCustom();
                    _msg.Error = false;
                    _msg.Status = (int)HttpStatusCode.NoContent;
                    _msg.Message = "El paciente no se encontró en la BD. Ingrese el codigo captcha para hacer una busqueda en el SIS";
                    _msg.Id = Convert.ToBase64String(ms.ToArray());
                    return Ok(_msg);
                }


                
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        [HttpGet]
        public IHttpActionResult GetData(string captcha, string value, int tipoBusqueda)
        {
            RootObjectSIS response = new RootObjectSIS();

            try
            {
                var tipoPersona = value.Substring(0, 1) == "2" ? "1" : "0";
                var myUrl = string.Format("http://e-consultaruc.sunat.gob.pe/cl-ti-itmrconsruc/jcrS00Alias?accion=consPorRuc&nroRuc={0}&codigo={1}&tipdoc={2}", value, captcha, tipoPersona);

                var myWebRequest = (HttpWebRequest)WebRequest.Create(myUrl);
                myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";//esto creo que lo puse por gusto :/
                myWebRequest.CookieContainer = _myCookie;
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                myWebRequest.Proxy = null;
                myWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
                var myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();

                var myStream = myHttpWebResponse.GetResponseStream();
                if (myStream == null)
                {
                    response.Error = true;
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Message = "Error Server";
                    return Ok(response);
                }


                var myStreamReader = new StreamReader(myStream, Encoding.GetEncoding("ISO-8859-1"));
                var s = myStreamReader.ReadToEnd();

                HtmlDocument document = new HtmlAgilityPack.HtmlDocument();
                document.LoadHtml(s);
                var tabla = document.DocumentNode.SelectSingleNode("//*[contains(@class,'form-table')]");

                if (tabla == null)
                {
                    response.Error = true;
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Message = "Error Captcha";
                    return Ok(response);
                }







                if (tipoBusqueda == 1)//RUC
                {
                    var busquedaRuc = driver.FindElement(By.Id("btnPorRuc"));
                    busquedaRuc.Click();

                    var txtRuc = driver.FindElement(By.Id("txtRuc"));
                    txtRuc.SendKeys(value);
                    
                }
                else if (tipoBusqueda == 2)//DNI
                {
                    var busquedaDni = driver.FindElement(By.Id("btnPorDocumento"));
                    busquedaDni.Click();

                    var txtNumeroDocumento = driver.FindElement(By.Id("txtNumeroDocumento"));
                    txtNumeroDocumento.SendKeys(value);
                }
                else if (tipoBusqueda == 3)//NOMBRE
                {
                    var busquedaNombre = driver.FindElement(By.Id("btnPorRazonSocial"));
                    busquedaNombre.Click();
                    var txtNombreRazonSocial = driver.FindElement(By.Id("txtNombreRazonSocial"));
                    txtNombreRazonSocial.SendKeys(value);
                }
                var txtCodigo = driver.FindElement(By.Name("codigo"));
                txtCodigo.SendKeys(captcha);


                var btnBuscar = driver.FindElement(By.Id("btnAceptar"));
                var testo = btnBuscar.Text;
                btnBuscar.Click();
                var errorText = driver.FindElement(By.ClassName("error"));
                if (errorText != null)
                {
                    response.Error = true;
                    response.Status = (int)HttpStatusCode.BadRequest;
                    response.Message = errorText.Text;
                    return Ok(response);
                }
                var existeData = driver.FindElement(By.ClassName("panel-heading")).Text;
                ResultSIS _ResultSIS = new ResultSIS();
                if (existeData == "Resultado de la Búsqueda")
                {


                    response.BaseDatos = false;
                    response.Error = false;
                    response.Status = (int)HttpStatusCode.OK;
                    response.Result = _ResultSIS;

                }
                else if (existeData == "Relación de contribuyentes")
                {
                    var anchorLink = driver.FindElement(By.XPath("//*[@class=\"list-group-item\"]"));
                    anchorLink.Click();
                    if (tipoBusqueda == 1)
                    {
                        
                    }
                    else
                    {
                        response.Error = true;
                        response.Status = (int)HttpStatusCode.NoContent;
                        response.Message = "El paciente no se encuentra registrado en el SIS.";
                    }
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
