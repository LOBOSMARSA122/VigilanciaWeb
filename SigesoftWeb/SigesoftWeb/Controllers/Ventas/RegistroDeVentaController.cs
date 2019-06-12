using SigesoftWeb.Controllers.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Ventas
{
    [SessionState(System.Web.SessionState.SessionStateBehavior.ReadOnly)]
    public class RegistroDeVentaController : Controller
    {
        [GeneralSecurity(Rol = "RegistroDeVenta-Index")]
        public ActionResult Index()
        {
            return View();
        }
    }
}