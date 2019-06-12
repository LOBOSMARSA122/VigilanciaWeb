using BE.Common;
using BL.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Common
{
    public class DataHierarchyController : ApiController
    {
        private  DataHierarchyBL oDataHierarchyBL = new DataHierarchyBL();

        [HttpGet]
        public IHttpActionResult GetDataHierarchyByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = oDataHierarchyBL.GetDatahierarchyByGrupoId(grupoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetProvincia(string name)
        {
            List<Dropdownlist> resultDist = oDataHierarchyBL.GetDistritos(name);
            List<Dropdownlist> resultProv = oDataHierarchyBL.GetProvincia(resultDist[0].Id);
            return Ok(resultProv);
        }

        [HttpGet]
        public IHttpActionResult GetDepartamento(int idProv)
        { 
            List<Dropdownlist> resultProv = oDataHierarchyBL.GetProvincia(idProv);
            List<Dropdownlist> resultDep = oDataHierarchyBL.GetDepartamento(resultProv[0].Value2);
            return Ok(resultDep);
        }

        [HttpGet]
        public IHttpActionResult GetDataHierarchySAMBHSByGrupoId(int grupoId)
        {
            List<Dropdownlist> result = new SAMBHSBL.DataHierarchy.DataHierarchyBL().GetDatahierarchySAMBHSByGrupoId(grupoId);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetVendedorSAMBHS()
        {
            List<Dropdownlist> result = new SAMBHSBL.DataHierarchy.DataHierarchyBL().GetVendedor();
            return Ok(result);
        }
    }
}
