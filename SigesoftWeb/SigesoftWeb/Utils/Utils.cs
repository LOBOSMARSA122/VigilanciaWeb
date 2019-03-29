using SigesoftWeb.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace SigesoftWeb.Utils
{
    public class Utils
    {
        public static string Encrypt(string pData)
        {
            System.Text.UnicodeEncoding parser = new System.Text.UnicodeEncoding();
            byte[] _original = parser.GetBytes(pData);
            MD5CryptoServiceProvider Hash = new MD5CryptoServiceProvider();
            byte[] _encrypt = Hash.ComputeHash(_original);
            return Convert.ToBase64String(_encrypt);
        }

        public static List<Dropdownlist> LoadDropDownList(List<Dropdownlist> list, string action)
        {
            Dropdownlist oDropdownlistModel = new Dropdownlist
            {
                Id = "-1"
            };

            if (action == "Seleccionar")
                oDropdownlistModel.Value = "--Seleccionar--";
            else
                oDropdownlistModel.Value = "--Todos--";

            list.Insert(0, oDropdownlistModel);
            return list;
        }

    }
}