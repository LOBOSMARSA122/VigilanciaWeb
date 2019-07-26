using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Security;
using SigesoftWeb.Utils;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers
{
    public class GeneralsController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToRoute("General_login");
        }

        [GeneralSecurity(Rol = "")]
        public ActionResult Home()
        {
            return View("~/Views/Generals/Index.cshtml", ViewBag.MENU);
        }

        public ActionResult Login()
        {
            if (TempData["MESSAGE"] != null)
            {
                ViewBag.MESSAGE = TempData["MESSAGE"];
            }
            return View("~/Views/Generals/Login.cshtml");
        }

        public ActionResult Logout()
        {
            Session.Remove("AutSigesoftWeb");
            Session.RemoveAll();
            return RedirectToRoute("General_login");
        }

        public ActionResult Login_authentication(FormCollection collection)
        {
            if (TempData["FormCollection"] != null)
                collection = (FormCollection)TempData["FormCollection"];

            if (ValidateEmptyFields(collection))
            {
                if (ValidateSystemUser(collection))

                    if(ViewBag.USER.SystemUserId == 0)
                        return RedirectToAction("Index", "WorkerData");
                    else
                        return RedirectToAction("Index", "Agenda");
                else
                    return RedirectToRoute("General_Login");
            }
            else
            {
                TempData["MESSAGE"] = "Debe ingresar el usuario y/o la contraseña";
                return RedirectToRoute("General_Login");
            }

        }

        private bool ValidateSystemUser(FormCollection collection)
        {
            TempData["FormCollection"] = null;

            Api API = new Api();
            var systemUser = API.Get<UserLogin>(relativePath: "Authorization/ValidateSystemUser", args: Arguments(collection));
            if (systemUser != null)
            {
                Session.Add("AutSigesoftWeb", PopulateClientSession(systemUser));
                return true;
            }
            else
            {
                TempData["MESSAGE"] = "Usuario o contraseña incorrectos";
                return false;
            }
        }

        private bool ValidateEmptyFields(FormCollection collection)
        {
            if (string.IsNullOrWhiteSpace(collection.Get("userName").Trim()) || string.IsNullOrWhiteSpace(collection.Get("password").Trim()))
                return false;

            return true;
        }

        private ClientSession PopulateClientSession(dynamic usuario)
        {
            ViewBag.USER = usuario;

            var oclientSession = new ClientSession
            {
                SystemUserId = ViewBag.USER.SystemUserId,
                PersonId = ViewBag.USER.PersonId,
                FullName = ViewBag.USER.FullName,
                PersonImage = ViewBag.USER.PersonImage,
                RucEmpresa = ViewBag.USER.RucEmpresa,
                SystemUserByOrganizationId = ViewBag.USER.SystemUserByOrganizationId,
                Permissions = ViewBag.USER.Permissions,
                EstablecimientoPredeterminado = ViewBag.USER.EstablecimientoPredeterminado,
                Organizations = ViewBag.USER.Organizations,
                Options = ViewBag.USER.Options,
                UserName = ViewBag.USER.UserName,
                TelephoneNumber = ViewBag.USER.TelephoneNumber,
                NodeId = int.Parse(ConfigurationManager.AppSettings["NodeId"])
        };
            return oclientSession;
        }

        private Dictionary<string, string> Arguments(FormCollection collection)
        {
            Dictionary<string, string> accessUser = new Dictionary<string, string>
            {
                { "nodeId", "9" },
                { "userName", collection.Get("userName").Trim() },
                { "password", Utils.Utils.Encrypt(collection.Get("password").Trim()) }
            };

            return accessUser;
        }

        public ActionResult SessionExpired()
        {
            Session.Remove("AutSigesoftWeb");
            Session.RemoveAll();
            return RedirectToRoute("General_login");
        }
    }
}