using BE.RUC;
using HtmlAgilityPack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static BE.Common.Enumeratores;

namespace DAL.RUC
{
    public class RucDataDal
    {

        public RootObject GetDataContribuyente(string numDoc)
        {
            RootObject _RootObject = new RootObject();
            try
            {
                
                ControlResultadosRUC result = new ControlResultadosRUC();
                var myUrl = "https://api.sunat.cloud/ruc/" + numDoc;
                var myWebRequest = (HttpWebRequest)WebRequest.Create(myUrl);
                myWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:23.0) Gecko/20100101 Firefox/23.0";//esto creo que lo puse por gusto :/
                myWebRequest.Credentials = CredentialCache.DefaultCredentials;
                myWebRequest.Proxy = null;
                myWebRequest.ContentType = "text/xml;charset=\"utf-8\"";
                var myHttpWebResponse = (HttpWebResponse)myWebRequest.GetResponse();
                var myStream = myHttpWebResponse.GetResponseStream();
                StreamReader sr = new StreamReader(myStream);
                string sContentsa = sr.ReadToEnd();
                sr.Close();               
                if (sContentsa == "[]")
                {
                    _RootObject.success = false;
                    _RootObject.mensaje = "No se obtuvo ningún resultado";
                }
                else
                {
                    _RootObject.success = true;
                    result = JsonConvert.DeserializeObject<ControlResultadosRUC>(sContentsa);
                }



                _RootObject.result = result;
                return _RootObject;
            }
            catch (Exception)
            {
                _RootObject.success = false;
                _RootObject.mensaje = "No se obtuvo ningún resultado";
                return _RootObject;
            }
            
        }

    }
}
