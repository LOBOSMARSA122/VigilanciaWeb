using BE.Common;
using BE.Z_CommonSAMBHS;
using BE.Z_SAMBHSCUSTOM.Productos;
using SAMBHSDAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.z_ProductsSAMBHS
{
    public class ProductSAMBHSDal
    {
        DateTime Fechanull = DateTime.Parse("01/01/1753");
        public BoardProductsSAMBHS GetProductsSAMBHS_PV(BoardProductsSAMBHS data)
        {
            try
            {
                DatabaseSAMBHSContext cnx = new DatabaseSAMBHSContext();
                var RucEmpresa = data.RucEmpresa == null ? "" : data.RucEmpresa;
                var fltValue = data.ProductName == null ? "" : data.ProductName;
                var ListaSaldosIniciales = new List<CostoNotasIngresoPedido>();
                data.IdAlmacen = int.Parse(System.Configuration.ConfigurationManager.AppSettings["appAlmacenPredeterminado"]);
                var periodo = DateTime.Now.Year.ToString();

                if (RucEmpresa == Constants.RucWortec)
                {
                    ListaSaldosIniciales = SaldosIniciales(periodo);
                }

                IQueryable query;
                if (data.ConStock)
                {
                    #region Query Con Stock

                    query = (from n in cnx.Producto
                             join D in cnx.ProductoDetalle on n.v_IdProducto equals D.v_IdProducto into D_join
                             from D in D_join.DefaultIfEmpty()

                             join J3 in cnx.ProductoAlmacen on new { a = D.v_IdProductoDetalle, b = data.IdAlmacen, eliminado = 0, _periodo = periodo } equals new { a = J3.v_ProductoDetalleId, b = J3.i_IdAlmacen, eliminado = J3.i_Eliminado.Value, _periodo = J3.v_Periodo } into J3_join
                             from J3 in J3_join.DefaultIfEmpty()

                             join J4 in cnx.DataHierarchy on new { a = n.i_IdUnidadMedida.Value, b = 17 }
                                 equals new { a = J4.i_ItemId.Value, b = J4.i_GroupId } into J4_join
                             from J4 in J4_join.DefaultIfEmpty()

                             join J5 in cnx.Linea on n.v_IdLinea equals J5.v_IdLinea into J5_join
                             from J5 in J5_join.DefaultIfEmpty()

                             join J6 in cnx.DataHierarchy on new { a = n.i_IdPerfilDetraccion.Value, b = 176 }
                                 equals new { a = J6.i_ItemId.Value, b = J6.i_GroupId } into J6_join
                             from J6 in J6_join.DefaultIfEmpty()

                             where (n.v_Descripcion.Contains(fltValue) || n.v_CodInterno.Contains(fltValue) || fltValue == "") &&n.i_Eliminado == 0 && n.i_EsServicio == 0 && n.i_EsActivoFijo == 0 && n.i_EsActivo == 1
                                   && J3.d_StockActual > 0 &&
                                   J3_join.Any(p => p.v_ProductoDetalleId == D.v_IdProductoDetalle)    // <---linea clave

                             select new
                             {
                                 n.v_IdProducto,
                                 D.v_IdProductoDetalle,
                                 n.v_Descripcion,
                                 n.v_CodInterno,
                                 n.i_EsServicio,
                                 n.i_EsLote,
                                 n.d_PrecioVenta,
                                 n.i_IdTipoProducto,
                                 stockActual = J3.d_StockActual,
                                 J3.v_IdProductoAlmacen,
                                 d_separacion = J3.d_SeparacionTotal ?? 0,
                                 n.i_IdUnidadMedida,
                                 EmpaqueUnidadMedida = J4.v_Value1,
                                 n.d_Empaque,
                                 n.i_EsAfectoDetraccion,
                                 TasaDetraccion = J6 != null ? J6.v_Value2 : "0",
                                 TopeDetraccion = J6 != null ? J6.v_Field : "0",
                                 n.i_NombreEditable,
                                 StockDisponible = J3.d_StockActual - J3.d_SeparacionTotal,
                                 n.i_ValidarStock,
                                 n.i_EsAfectoPercepcion,
                                 n.d_TasaPercepcion,
                                 n.i_PrecioEditable,
                                 ProductoAlmacen = J3.v_IdProductoAlmacen,
                                 NroCuentaVenta = J5.v_NroCuentaVenta,
                                 NroCuentaCompra = J5.v_NroCuentaCompra,
                                 J3.i_IdAlmacen,
                                 v_NroPedidoExportacion = J3.v_NroPedido,
                                 ValorUM = string.IsNullOrEmpty(J4.v_Value2) ? "0" : J4.v_Value2,
                                 UM = "UNIDADES",
                                 Observaciones = n.v_Caracteristica,
                                 AfectoIsc = n.i_EsAfectoIsc,
                                 StockMinimo = n.d_StockMinimo == null ? 1 : J3 == null || J3.d_StockActual == null ? 1 : J3.d_StockActual <= n.d_StockMinimo ? 1 : 0,
                                 n.v_Descripcion2,

                                 i_SolicitarNroLoteIngreso = n.i_SolicitarNroLoteIngreso ?? 0,
                                 i_SolicitarNroSerieIngreso = n.i_SolicitarNroSerieIngreso ?? 0,
                                 i_SolicitaOrdenProduccionIngreso = n.i_SolicitaOrdenProduccionIngreso ?? 0,
                                 i_SolicitarNroSerieSalida = n.i_SolicitarNroSerieSalida ?? 0,
                                 i_SolicitarNroLoteSalida = n.i_SolicitarNroLoteSalida ?? 0,
                                 i_SolicitaOrdenProduccionSalida = n.i_SolicitaOrdenProduccionSalida ?? 0,

                                 J3.v_NroLote,
                                 J3.v_NroSerie,
                                 t_FechaCaducidad = J3.t_FechaCaducidad == null ? Fechanull : J3.t_FechaCaducidad,

                             }).ToList().Select(p =>
                             {
                                 var PrecioWortec = Constants.RucWortec == RucEmpresa
                                     ? ListaSaldosIniciales.Where(
                                         a =>
                                             a.v_IdProductoDetalle == p.v_IdProductoDetalle &&
                                             a.NroPedido == p.v_NroPedidoExportacion).ToList()
                                     : null;
                                 return new ProductCustomSAMBHS
                                 {
                                     TasaDetraccion = p.TasaDetraccion,
                                     TopeDetraccion = p.TopeDetraccion,
                                     v_IdProducto = p.v_IdProducto,
                                     v_IdProductoDetalle = p.v_IdProductoDetalle,
                                     v_Descripcion = p.v_Descripcion,
                                     v_CodInterno = p.v_CodInterno,
                                     i_EsServicio = p.i_EsServicio,
                                     i_EsLote = p.i_EsLote,
                                     i_IdTipoProducto = p.i_IdTipoProducto,
                                     stockActual = p.stockActual,
                                     v_IdProductoAlmacen = p.v_IdProductoAlmacen,
                                     d_separacion = p.d_separacion,
                                     i_IdUnidadMedida = p.i_IdUnidadMedida,
                                     EmpaqueUnidadMedida = p.EmpaqueUnidadMedida,
                                     d_Empaque = p.d_Empaque,
                                     i_EsAfectoDetraccion = p.i_EsAfectoDetraccion,
                                     i_NombreEditable = p.i_NombreEditable,
                                     StockDisponible = p.StockDisponible,
                                     i_ValidarStock = p.i_ValidarStock,
                                     i_EsAfectoPercepcion = p.i_EsAfectoPercepcion,
                                     d_TasaPercepcion = p.d_TasaPercepcion,
                                     i_PrecioEditable = p.i_PrecioEditable,
                                     d_Precio = Constants.RucWortec == RucEmpresa ? (PrecioWortec.Any() ? DevuelveValorRedondeado(PrecioWortec.Average(x => x.Costo), 4)
                                                 : 0)
                                             : p.d_PrecioVenta,
                                     d_Descuento = 0,
                                     IdMoneda = 1,
                                     NroCuentaVenta = p.NroCuentaVenta,
                                     NroCuentaCompra = p.NroCuentaCompra,
                                     IdAlmacen = p.i_IdAlmacen,
                                     v_NroPedidoExportacion = p.v_NroPedidoExportacion,
                                     StockActualUM = p.stockActual / int.Parse(p.ValorUM),
                                     SeparacionActualUM = p.d_separacion / int.Parse(p.ValorUM),
                                     SaldoUM = (p.stockActual / int.Parse(p.ValorUM)) - p.d_separacion / int.Parse(p.ValorUM),
                                     UM = p.UM,
                                     Observacion = p.Observaciones.Trim(),
                                     EsAfectoIsc = p.AfectoIsc.HasValue && p.AfectoIsc == 1,
                                     StockMinimo = p.StockMinimo,
                                     v_Descripcion2 = p.v_Descripcion2,

                                     i_SolicitarNroLoteIngreso = p.i_SolicitarNroLoteIngreso,
                                     i_SolicitarNroSerieIngreso = p.i_SolicitarNroSerieIngreso,
                                     i_SolicitaOrdenProduccionIngreso = p.i_SolicitaOrdenProduccionIngreso,
                                     i_SolicitarNroSerieSalida = p.i_SolicitarNroSerieSalida,
                                     i_SolicitarNroLoteSalida = p.i_SolicitarNroLoteSalida,
                                     i_SolicitaOrdenProduccionSalida = p.i_SolicitaOrdenProduccionSalida,



                                     v_NroLote = p.v_NroLote,
                                     v_NroSerie = p.v_NroSerie,
                                     t_FechaCaducidad = p.t_FechaCaducidad.Value,
                                 };

                             }).AsQueryable();
                    int skip = (data.Index - 1) * data.Take;
                    var FinalList = query.Cast<ProductCustomSAMBHS>().ToList().OrderBy(o => o.t_FechaCaducidad).ToList();
                    var ListProducts = FinalList.GroupBy(g => g.v_IdProducto).Select(s => s.First()).ToList();
                    data.TotalRecords = ListProducts.Count;
                    if (data.Take > 0)
                        ListProducts = ListProducts.Skip(skip).Take(data.Take).ToList();
                    data.List = ListProducts;

                    #endregion
                }
                else
                {
                    #region Query Sin Importar Stock

                    query = (from n in cnx.Producto
                             join D in cnx.ProductoDetalle on n.v_IdProducto equals D.v_IdProducto into D_join
                             from D in D_join.DefaultIfEmpty()

                             join J4 in cnx.DataHierarchy on new { a = n.i_IdUnidadMedida, b = 17 }
                                 equals new { a = J4.i_ItemId, b = J4.i_GroupId } into J4_join
                             from J4 in J4_join.DefaultIfEmpty()

                             join J5 in cnx.Linea on n.v_IdLinea equals J5.v_IdLinea into J5_join
                             from J5 in J5_join.DefaultIfEmpty()

                             join J6 in cnx.ProductoAlmacen on new { a = D.v_IdProductoDetalle, b = data.IdAlmacen, eliminado = 0, _periodo = periodo }
                                 equals new { a = J6.v_ProductoDetalleId, b = J6.i_IdAlmacen, eliminado = J6.i_Eliminado.Value, _periodo = J6.v_Periodo } into J6_join
                             from J6 in J6_join.DefaultIfEmpty()

                             join J7 in cnx.DataHierarchy on new { a = n.i_IdPerfilDetraccion, b = 176 }
                                 equals new { a = J7.i_ItemId, b = J7.i_GroupId } into J7_join
                             from J7 in J7_join.DefaultIfEmpty()

                             where n.i_Eliminado == 0 && n.i_EsServicio == 0 && n.i_EsActivoFijo == 0 && n.i_EsActivo == 1
                             && D_join.Any(p => p.v_IdProductoDetalle.Equals(J6.v_ProductoDetalleId))
                             select new
                             {
                                 v_IdProducto = n.v_IdProducto,
                                 v_IdProductoDetalle = D.v_IdProductoDetalle,
                                 v_Descripcion = n.v_Descripcion,
                                 v_CodInterno = n.v_CodInterno,
                                 i_EsServicio = n.i_EsServicio,
                                 i_EsLote = n.i_EsLote,
                                 n.d_PrecioVenta,
                                 i_IdTipoProducto = n.i_IdTipoProducto,
                                 i_IdUnidadMedida = n.i_IdUnidadMedida,
                                 EmpaqueUnidadMedida = J4.v_Value1,
                                 d_Empaque = n.d_Empaque,
                                 i_EsAfectoDetraccion = n.i_EsAfectoDetraccion,
                                 i_NombreEditable = n.i_NombreEditable,
                                 i_ValidarStock = n.i_ValidarStock,
                                 i_EsAfectoPercepcion = n.i_EsAfectoPercepcion,
                                 d_TasaPercepcion = n.d_TasaPercepcion,
                                 i_PrecioEditable = n.i_PrecioEditable,
                                 NroCuentaVenta = J5.v_NroCuentaVenta,
                                 NroCuentaCompra = J5.v_NroCuentaCompra,
                                 NroPedido = J6.v_NroPedido,
                                 stockActual = J6.d_StockActual,
                                 v_IdProductoAlmacen = J6.v_IdProductoAlmacen,
                                 d_Separacion = J6.d_SeparacionTotal == null ? 0 : J6.d_SeparacionTotal,
                                 d_StockActual = J6.d_StockActual,
                                 i_IdAlmacen = J6.i_IdAlmacen,
                                 v_NroPedidoExportacion = J6.v_NroPedido,
                                 ValorUM = string.IsNullOrEmpty(J4.v_Value2) ? "0" : J4.v_Value2,
                                 UM = "UNIDADES",
                                 AfectoIsc = n.i_EsAfectoIsc,
                                 StockMinimo = n.d_StockMinimo == null ? 1 : J6 == null || J6.d_StockActual == null ? 1 : J6.d_StockActual <= n.d_StockMinimo ? 1 : 0,
                                 v_Descripcion2 = n.v_Descripcion2,
                                 TasaDetraccion = J7 != null ? J7.v_Value2 : "0",
                                 TopeDetraccion = J7 != null ? J7.v_Field : "0",



                                 i_SolicitarNroLoteIngreso = n.i_SolicitarNroLoteIngreso ?? 0,
                                 i_SolicitarNroSerieIngreso = n.i_SolicitarNroSerieIngreso ?? 0,
                                 i_SolicitaOrdenProduccionIngreso = n.i_SolicitaOrdenProduccionIngreso ?? 0,
                                 i_SolicitarNroSerieSalida = n.i_SolicitarNroSerieSalida ?? 0,
                                 i_SolicitarNroLoteSalida = n.i_SolicitarNroLoteSalida ?? 0,
                                 i_SolicitaOrdenProduccionSalida = n.i_SolicitaOrdenProduccionSalida ?? 0,


                                 v_NroLote = J6.v_NroLote,
                                 v_NroSerie = J6.v_NroSerie,
                                 t_FechaCaducidad = J6.t_FechaCaducidad == null ? Fechanull : J6.t_FechaCaducidad.Value,

                             }).ToList().Select(p =>
                             {
                                 decimal? PrecioWortec = Constants.RucWortec == RucEmpresa
                                     ? ListaSaldosIniciales.Where(
                                         a =>
                                             a.v_IdProductoDetalle == p.v_IdProductoDetalle &&
                                             a.NroPedido == p.v_NroPedidoExportacion).Sum(a => a.Costo)
                                     : 0;
                                 return new ProductCustomSAMBHS
                                 {
                                     v_IdProducto = p.v_IdProducto,
                                     v_IdProductoDetalle = p.v_IdProductoDetalle,
                                     v_Descripcion = p.v_Descripcion,
                                     v_CodInterno = p.v_CodInterno,
                                     i_EsServicio = p.i_EsServicio,
                                     i_EsLote = p.i_EsLote,
                                     i_IdTipoProducto = p.i_IdTipoProducto,
                                     stockActual = p.v_IdProductoAlmacen != null ? p.stockActual ?? 0 : 0,
                                     v_IdProductoAlmacen = p.v_IdProductoAlmacen,
                                     d_separacion = p.v_IdProductoAlmacen != null ? p.d_Separacion ?? 0 : 0,
                                     i_IdUnidadMedida = p.i_IdUnidadMedida,
                                     EmpaqueUnidadMedida = p.EmpaqueUnidadMedida,
                                     d_Empaque = p.d_Empaque,
                                     i_EsAfectoDetraccion = p.i_EsAfectoDetraccion,
                                     i_NombreEditable = p.i_NombreEditable,
                                     StockDisponible = p.d_StockActual,
                                     i_ValidarStock = p.i_ValidarStock,
                                     i_EsAfectoPercepcion = p.i_EsAfectoPercepcion,
                                     d_TasaPercepcion = p.d_TasaPercepcion,
                                     i_PrecioEditable = p.i_PrecioEditable,
                                     d_Precio = p.v_IdProductoAlmacen != null ? RucEmpresa == Constants.RucWortec ? PrecioWortec != null ? PrecioWortec : 0 : p.d_PrecioVenta : 0,
                                     d_Descuento = 0,
                                     IdMoneda = 1,
                                     NroCuentaVenta = p.NroCuentaVenta,
                                     NroCuentaCompra = p.NroCuentaCompra,
                                     IdAlmacen =
                                         p.v_IdProductoAlmacen != null ? -1 : p.i_IdAlmacen == 0 ? -1 : p.i_IdAlmacen,
                                     v_NroPedidoExportacion = p.NroPedido,
                                     StockActualUM = p.stockActual == 0 || p.ValorUM == "0" ? 0 : p.stockActual / int.Parse(p.ValorUM),
                                     SeparacionActualUM = p.d_Separacion == 0 || p.ValorUM == "0" ? 0 : p.d_Separacion / int.Parse(p.ValorUM),
                                     SaldoUM =
                                         p.ValorUM == "0"
                                             ? 0
                                             : p.stockActual != 0 && p.ValorUM != "0" && p.d_Separacion != 0
                                                 ? (p.stockActual / int.Parse(p.ValorUM)) -
                                                   p.d_Separacion / int.Parse(p.ValorUM)
                                                 : p.stockActual == 0 && p.ValorUM != "0" && p.d_Separacion != 0
                                                     ? -p.d_Separacion / int.Parse(p.ValorUM)
                                                     : p.d_Separacion == 0 && p.ValorUM != "0" && p.stockActual != 0
                                                         ? (p.stockActual / int.Parse(p.ValorUM))
                                                         : 0,
                                     UM = p.UM,
                                     EsAfectoIsc = p.AfectoIsc.HasValue && p.AfectoIsc == 1,
                                     StockMinimo = p.StockMinimo,
                                     v_Descripcion2 = p.v_Descripcion2,
                                     TasaDetraccion = p.TasaDetraccion,
                                     TopeDetraccion = p.TopeDetraccion,
                                     i_SolicitarNroLoteIngreso = p.i_SolicitarNroLoteIngreso,
                                     i_SolicitarNroSerieIngreso = p.i_SolicitarNroSerieIngreso,
                                     i_SolicitaOrdenProduccionIngreso = p.i_SolicitaOrdenProduccionIngreso,
                                     i_SolicitarNroSerieSalida = p.i_SolicitarNroSerieSalida,
                                     i_SolicitarNroLoteSalida = p.i_SolicitarNroLoteSalida,
                                     i_SolicitaOrdenProduccionSalida = p.i_SolicitaOrdenProduccionSalida,

                                     v_NroSerie = p.v_NroSerie,
                                     v_NroLote = p.v_NroLote,
                                     t_FechaCaducidad = p.t_FechaCaducidad,
                                 };
                             }).AsQueryable();
                    int skip = (data.Index - 1) * data.Take;
                    var FinalList = query.Cast<ProductCustomSAMBHS>().ToList().OrderBy(o => o.t_FechaCaducidad).ToList();
                    var ListProducts = FinalList.GroupBy(g => g.v_IdProducto).Select(s => s.First()).ToList();
                    data.TotalRecords = ListProducts.Count;
                    if (data.Take > 0)
                        ListProducts = ListProducts.Skip(skip).Take(data.Take).ToList();
                    data.List = ListProducts;
                    #endregion

                }
                return data;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static decimal DevuelveValorRedondeado(decimal valor, int nroDecimales, MidpointRounding midpointRounding = MidpointRounding.AwayFromZero)
        {
            return decimal.Parse(Math.Round(valor, nroDecimales, midpointRounding).ToString(CultureInfo.InvariantCulture));
        }

        public List<CostoNotasIngresoPedido> SaldosIniciales(string Periodo)
        {

            using (DatabaseSAMBHSContext dbContex = new DatabaseSAMBHSContext())
            {
                var SaldosIniciales = (from a in dbContex.MovimientoDetalle

                                       join b in dbContex.Movimiento on new { m = a.v_IdMovimiento, eliminado = 0 } equals new { m = b.v_IdMovimiento, eliminado = b.i_Eliminado.Value } into b_join

                                       from b in b_join.DefaultIfEmpty()

                                       join c in dbContex.Cliente on new { c = b.v_IdCliente, eliminado = 0 } equals new { c = c.v_IdCliente, eliminado = c.i_Eliminado.Value } into c_join

                                       from c in c_join.DefaultIfEmpty()

                                       join d in dbContex.Documento on new { d = a.i_IdTipoDocumento.Value, eliminado = 0 } equals new { d = d.i_CodigoDocumento, eliminado = d.i_Eliminado.Value } into d_join

                                       from d in d_join.DefaultIfEmpty()

                                       join e in dbContex.ProductoDetalle on new { pd = a.v_IdProductoDetalle, eliminado = 0 } equals new { pd = e.v_IdProductoDetalle, eliminado = e.i_Eliminado.Value } into e_join

                                       from e in e_join.DefaultIfEmpty()
                                       where a.i_Eliminado == 0
                                        && b.i_IdTipoMovimiento == (int)Enumeratores.TipoDeMovimiento.NotadeIngreso
                                        && b.v_Periodo == Periodo
                                       select new
                                       {
                                           NroPedido = a.v_NroPedido.Trim(),

                                           Costo = b.i_IdMoneda == (int)Enumeratores.Currency.Dolares ? b.v_OrigenTipo == "I" ? a.d_Precio : a.d_PrecioCambio : a.d_Precio / b.d_TipoCambio,
                                           v_IdProductoDetalle = e.v_IdProductoDetalle,



                                       }).ToList().AsQueryable().Select(x => new CostoNotasIngresoPedido
                                       {

                                           Costo = x.Costo ?? 0,
                                           v_IdProductoDetalle = x.v_IdProductoDetalle,
                                           NroPedido = x.NroPedido,
                                       }).ToList();
                return SaldosIniciales;
            }
        }


    }

}
