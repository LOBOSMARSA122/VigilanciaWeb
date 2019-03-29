using SigesoftWeb.Models.Security;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace SigesoftWeb.Controllers.Security
{
    public class GeneralSecurityAttribute : ActionFilterAttribute
    {
        public string Rol { get; set; }
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ClientSession Usuario = (ClientSession)filterContext.HttpContext.Session.Contents["AutSigesoftWeb"];
            if (Usuario == null)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
                                       { "action", "Login" },
                                       { "controller", "Generals" }});
                return;
            }

            filterContext.Controller.ViewBag.USER = Usuario;


            if (string.IsNullOrWhiteSpace(Rol))
                return;


            //bool aceptado = false;
            //AuthorizationModel AUT = Usuario.Autorizacion.Where(x => x.Descripcion.Contains(Rol.Split('-')[0])).FirstOrDefault();
            //if (AUT != null)
            //{
            //    if (AUT.SubMenus.Where(x => x.Descripcion.Contains(Rol.Split('-')[1])).FirstOrDefault() != null)
            //        aceptado = true;
            //}

            //if (!aceptado)
            //{
            //    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary{
            //                           { "action", "Home" },
            //                           { "controller", "Generals" }});
            //    return;
            //}


            return;
        }

    }
}