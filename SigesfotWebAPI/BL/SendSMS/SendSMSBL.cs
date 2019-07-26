using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BL.SendSMS
{
    public class SendSMSBL
    {
        public string SendSMS(string number)
        {
            
            string KeySMS = CreateKeySMS(5);

            string apicard = "0463979972";
            string apikey = "AF4B54793398";
            string smsnumber = number;
            string smstext = KeySMS;
            string smstype = "0";
            string url = "http://api2.gamanet.pe/smssend";

            string result = "";
            string strPost = "apicard=" + apicard + "&apikey=" + apikey + "&smsnumber=" + smsnumber + "&smstext=" + smstext + "&smstype=" + smstype;
            StreamWriter myWriter = null;

            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(url);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/x-www-form-urlencoded";

            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                return null;
            }
            finally
            {
                myWriter.Close();
            }

            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
            new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                    
                sr.Close();
            }

            return KeySMS;

        }

        private string CreateKeySMS(int longitud)
        {
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < longitud--)
            {
                res.Append(caracteres[rnd.Next(caracteres.Length)]);
            }
            return res.ToString();
        }
    }
}
