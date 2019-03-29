using Newtonsoft.Json;
using SigesoftWeb.Controllers.Security;
using SigesoftWeb.Models;
using SigesoftWeb.Models.Common;
using SigesoftWeb.Models.Organization;
using SigesoftWeb.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SigesoftWeb.Controllers.Organization
{
    public class OrganizationController : Controller
    {

        [GeneralSecurity(Rol = "Organization-BoardCompany")]
        public ActionResult Index()
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "grupoId" , ((int)Enums.SystemParameter.OrgType).ToString() }
            };

            ViewBag.OrgType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", arg), Constants.All);
            return View();
        }

        public async Task<ActionResult> FilterCompany(BoardCompany data)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "OrganizationTypeId", data.OrganizationTypeId.ToString() },
                { "IdentificationNumber", data.IdentificationNumber},
                { "Name", data.Name},

                { "Index", data.Index.ToString()},
                { "Take", data.Take.ToString()}
            };

            return await Task.Run(() =>
            {
                ViewBag.Company = API.Post<BoardCompany>("Organization/GetBoardCompany", arg);
                return PartialView("_BoardCompanyPartial");
            });
            
        }

        [GeneralSecurity(Rol = "Organization-CreateCompany")]
        public async Task<ActionResult> CreateCompany(string organizationId)
        {
            Api API = new Api();
            Dictionary<string, string> argOrgType = new Dictionary<string, string>()
            {
                { "grupoId", ((int)Enums.SystemParameter.OrgType).ToString() }
            };
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "organizationId", organizationId }
            };

            return await Task.Run(() =>
            {
                ViewBag.OrgType = Utils.Utils.LoadDropDownList(API.Get<List<Dropdownlist>>("SystemParameter/GetParametroByGrupoId", argOrgType), Constants.Select);
                ViewBag.Company = API.Get<Company>("Organization/GetCompanyById", arg);
                return View();
            });
        }

        [GeneralSecurity(Rol = "Organization-CreateCompany")]
        public async Task<JsonResult> AddCompany(Company company)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(company) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() }
            };

            return await Task.Run(() =>
            {
                bool response = API.Post<bool>("Organization/AddCompany", arg);
                return Json(response);
            });
        }

        [GeneralSecurity(Rol = "Organization-CreateCompany")]
        public async Task<JsonResult> EditCompany(Company company)
        {
            Api API = new Api();
            Dictionary<string, string> arg = new Dictionary<string, string>()
            {
                { "String1", JsonConvert.SerializeObject(company) },
                { "Int1", ViewBag.USER.SystemUserId.ToString() }
            };

            return await Task.Run(() =>
            {
                bool response = API.Post<bool>("Organization/EditCompany", arg);
                return Json(response);
            });
        }
    }
}