using BE.Common;
using BL.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Common
{
    public class SystemParameterController : ApiController
    {
        private SystemParameterBL oSystemParameterBL = new SystemParameterBL();

        [HttpGet]
        public IHttpActionResult GetParametroByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetParametroByGrupoId(grupoId);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetParameterTypeServiceByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetParameterTypeServiceByGrupoId(grupoId);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetParametroMasterServiceByGrupoId(int serviceType)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetParametroMasterServiceByGrupoId(serviceType);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetPuestos()
        {
            List<Dropdownlist> result = oSystemParameterBL.GetPuestos();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetGESO(string organizationId, string locationId)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetGeso(organizationId, locationId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetEmpresaFacturacion()
        {
            List<Dropdownlist> result = oSystemParameterBL.EmpresaFacturacion(9);
            return Ok(result);
        }
        [HttpGet]
        public IHttpActionResult GetProtocolsForCombo(int Service, int ServiceType)
        {
            List<Dropdownlist> result = oSystemParameterBL.GetProtocolsForCombo(Service, ServiceType);
            return Ok(result);
        }
        
        [HttpGet]
        public IHttpActionResult GetOrganizationAndLocation()
        {
            List<Dropdownlist> result = oSystemParameterBL.GetOrganizationAndLocation(9);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetUser()
        {
            List<Dropdownlist> result = oSystemParameterBL.GetUser();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetCentroCosto()
        {
            List<Dropdownlist> result = oSystemParameterBL.GetCentroCosto();
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetTitulares()
        {
            List<Dropdownlist> result = oSystemParameterBL.GetTitulares();
            return Ok(result);
        }
    }
}
