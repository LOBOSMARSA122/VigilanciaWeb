using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_SAMBHSCUSTOM.Productos
{
    public class Boards
    {
        public int TotalRecords { get; set; }
        public int Index { get; set; }
        public int Take { get; set; }
    }

    public class BoardProductsSAMBHS : Boards
    {
        public string ProductName { get; set; }
        public int IdAlmacen { get; set; }
        public string RucEmpresa { get; set; }
        public string IdCliente { get; set; }
        public bool ConStock { get; set; }
        public List<ProductCustomSAMBHS> List { get; set; }
    }
    public class ProductCustomSAMBHS
    {
        public decimal? stockActual { get; set; }
        public string v_IdProductoAlmacen { get; set; }
        public decimal? d_separacion { get; set; }
        public string v_IdProductoDetalle { get; set; }
        public string EmpaqueUnidadMedida { get; set; }
        public int IdAlmacen { get; set; }
        public string v_IdProducto { get; set; }
        public string v_Descripcion { get; set; }
        public string v_CodInterno { get; set; }
        public int? i_EsServicio { get; set; }
        public int? i_EsLote { get; set; }
        public int? i_IdTipoProducto { get; set; }
        public int? i_IdUnidadMedida { get; set; }
        public decimal? d_Empaque { get; set; }
        public int? i_EsAfectoDetraccion { get; set; }
        public string AfectoDetraccion
        {
            get
            {
                return (i_EsAfectoDetraccion ?? 0) == 1 ? "SI" : "NO";
            }
        }
        public string TasaDetraccion { get; set; }
        public string TopeDetraccion { get; set; }
        public decimal DTasaDetraccion
        {
            get
            {
                decimal d;
                if (string.IsNullOrWhiteSpace(TasaDetraccion)) return 0m;
                return decimal.TryParse(TasaDetraccion, out d) ? d : 0m;
            }
        }

        public decimal DTopeDetraccion
        {
            get
            {
                decimal d;
                if (string.IsNullOrWhiteSpace(TopeDetraccion)) return 0m;
                return decimal.TryParse(TopeDetraccion, out d) ? d : 0m;
            }
        }
        public decimal? d_PrecioMinSoles { get; set; }
        public decimal? d_DescuentoLP { get; set; }
        public decimal? d_Precio { get; set; }
        public decimal? d_Descuento { get; set; }
        public int? i_NombreEditable { get; set; }
        public decimal? d_Costo { get; set; }
        public decimal? StockDisponible { get; set; }
        public int? i_ValidarStock { get; set; }
        public int? i_EsAfectoPercepcion { get; set; }
        public decimal? d_TasaPercepcion { get; set; }
        public int? i_PrecioEditable { get; set; }
        public string EmpaqueUnidadMedidaFinal { get; set; }
        public int IdMoneda { get; set; }
        public string NroCuentaVenta { get; set; }
        public string NroCuentaCompra { get; set; }
        public string ValorUM { get; set; }
        public decimal PrecioVenta { get; set; }
        public string Moneda { get; set; }
        public bool EsProductoFinal { get; set; }
        public string v_NroPedidoExportacion { get; set; }
        public int i_IdAlmacen { get; set; }
        public decimal? StockActualUM { get; set; }
        public decimal? SeparacionActualUM { get; set; }
        public decimal? SaldoUM { get; set; }
        public string UM { get; set; }
        public string Observacion { get; set; }
        public bool EsAfectoIsc { get; set; }
        public int StockMinimo { get; set; }
        public string v_Descripcion2 { get; set; }
        public decimal d_StockMinimo { get; set; }
        public string Linea { get; set; }
        public string Ubicacion { get; set; }

        public int i_SolicitarNroLoteIngreso { get; set; }
        public int i_SolicitarNroSerieIngreso { get; set; }
        public int i_SolicitaOrdenProduccionIngreso { get; set; }

        public int i_SolicitarNroSerieSalida { get; set; }
        public int i_SolicitarNroLoteSalida { get; set; }
        public int i_SolicitaOrdenProduccionSalida { get; set; }

        public string v_NroSerie { get; set; }
        public string v_NroLote { get; set; }
        public DateTime? t_FechaCaducidad { get; set; }
    }

    public class CostoNotasIngresoPedido
    {
        public decimal Costo { get; set; }
        public string NroPedido { get; set; }
        public string Codigo { get; set; }
        public string v_IdProductoDetalle { get; set; }
    }
}
