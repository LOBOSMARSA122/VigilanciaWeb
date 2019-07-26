using SAMBHSBL.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SigesoftWebAPI.Controllers.Documento
{
    public class DocumentoController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetDocumentsForCombo(int UsadoCompras, int UsadoVentas)
        {
            var result = new DocumentoBL().GetDocumentsForCombo(UsadoCompras, UsadoVentas);
            return Ok(result);
        }

        [HttpGet]
        public IHttpActionResult GetSeriesDocumento(int IdEstablecimiento, int IdDocumento)
        {
            var result = new DocumentoBL().GetSeriesDocumento(IdEstablecimiento, IdDocumento);
            return Ok(result);
        }
    }
}
