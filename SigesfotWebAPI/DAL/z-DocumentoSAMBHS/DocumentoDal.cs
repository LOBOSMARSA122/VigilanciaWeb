using BE.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAMBHSDAL.Documento
{
    public class DocumentoDal
    {
        public List<KeyValueDTO> GetDocumentsForCombo(int pintUsadoCompras, int pintUsadoVentas)
        {
            try
            {
                int EstablecimientoPredeterminado = int.Parse(System.Configuration.ConfigurationManager.AppSettings["appEstablecimientoPredeterminado"]);
                string TipoDocumentoVentaRapida = System.Configuration.ConfigurationManager.AppSettings["csTipoDocumentoVentaRapida"];
                using (DatabaseSAMBHSContext dbContext = new DatabaseSAMBHSContext())
                {
                    var query = (from a in dbContext.Documento

                                 join A in dbContext.EstablecimientoDetalle on a.i_CodigoDocumento equals A.i_IdTipoDocumento //Trae solo documentos que fueron registrados en establecimientodetalle

                                 where a.i_Eliminado == 0 && A.i_Eliminado == 0 && A.i_Eliminado == 0 && A.i_IdEstablecimiento == EstablecimientoPredeterminado
                                 select a).Distinct();

                    if (pintUsadoCompras == 1)
                    {
                        query = query.Where(x => x.i_UsadoCompras == 1);
                    }
                    else if (pintUsadoVentas == 1)
                    {
                        query = query.Where(x => x.i_UsadoVentas == 1);
                    }


                    query = query.OrderBy(x => x.i_CodigoDocumento);

                    var query2 = query.AsEnumerable()
                                .Select(x => new KeyValueDTO
                                {
                                    Id = x.i_CodigoDocumento.ToString(),
                                    Value = x.v_Nombre,
                                    Value2 = x.v_Siglas,
                                    Value3 = TipoDocumentoVentaRapida,
                                    Value6 = x.i_UsadoDocumentoInterno == 1 ? true : false,
                                }).ToList();
                    return query2;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }

    }
}
