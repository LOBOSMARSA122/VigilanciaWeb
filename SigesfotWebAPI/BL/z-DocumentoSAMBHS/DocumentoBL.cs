using BE.Common;
using SAMBHSDAL;
using SAMBHSDAL.Documento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSBL.Documento
{
    public class DocumentoBL
    {
        public List<KeyValueDTO> GetDocumentsForCombo(int UsadoCompras, int UsadoVentas)
        {
            return new DocumentoDal().GetDocumentsForCombo(UsadoCompras, UsadoVentas);
        }

        public string GetSeriesDocumento(int IdEstablecimient, int IdDocumento)
        {
            return new DocumentoDal().GetSeriesDocumento(IdEstablecimient, IdDocumento);
        }
    }
}
