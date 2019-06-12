using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Z_CommonSAMBHS
{
    [Table("producto")]
    public class ProductoBE
    {
        [Key]
        public string v_IdProducto{ get; set; }

        public string v_IdLinea{ get; set; }
        public string v_IdMarca{ get; set; }
        public string v_IdModelo{ get; set; }
        public string v_CodInterno{ get; set; }
        public string v_Descripcion{ get; set; }
        public decimal? d_Empaque { get; set; }
        public int? i_IdUnidadMedida{ get; set; }
        public decimal? d_Peso { get; set; }
        public string v_Ubicacion{ get; set; }
        public string v_Caracteristica{ get; set; }
        public string v_CodProveedor{ get; set; }
        public string v_Descripcion2{ get; set; }
        public int? i_IdTipoProducto{ get; set; }
        public string v_RutaFoto{ get; set; }
        public int? i_EsAfectoPercepcion{ get; set; }
        public decimal? d_TasaPercepcion { get; set; }
        public int? i_EsAfectoDetraccion{ get; set; }
        public int? i_EsServicio{ get; set; }
        public int? i_EsActivoFijo{ get; set; }
        public int? i_NombreEditable{ get; set; }
        public int? i_PrecioEditable{ get; set; }
        public int? i_EsActivo{ get; set; }
        public int? i_EsLote{ get; set; }
        public int? i_ValidarStock{ get; set; }
        public decimal? d_PrecioVenta{ get; set; }
        public decimal? d_PrecioCosto { get; set; }
        public decimal? d_StockMinimo { get; set; }
        public decimal? d_StockMaximo { get; set; }
        public int? i_IdProveedor{ get; set; }
        public int? i_IdTipo{ get; set; }
        public int? i_IdUsuario{ get; set; }
        public int? i_IdTela{ get; set; }
        public int? i_IdEtiqueta{ get; set; }
        public int? i_IdCuello{ get; set; }
        public int? i_IdAplicacion{ get; set; }
        public int? i_IdArte{ get; set; }
        public int? i_IdColeccion{ get; set; }
        public int? i_IdTemporada{ get; set; }
        public int? i_Anio{ get; set; }
        public int? i_Eliminado{ get; set; }
        public int? i_InsertaIdUsuario{ get; set; }
        public DateTime? t_InsertaFecha{ get; set; }
        public int? i_ActualizaIdUsuario{ get; set; }
        public DateTime? t_ActualizaFecha { get; set; }
        public string v_IdColor{ get; set; }
        public string v_IdTalla{ get; set; }
        public byte[] b_Foto{ get; set; }
        public string v_Modelo{ get; set; }
        public short? i_EsAfectoIsc{ get; set; }
        public int? i_CantidadFabricacionMensual{ get; set; }
        public string v_NroPartidaArancelaria{ get; set; }
        public int? i_IndicaFormaParteOtrosTributos{ get; set; }
        public string v_NroParte{ get; set; }
        public string v_NroOrdenProduccion{ get; set; }
        public int? i_IdTipoTributo{ get; set; }
        public decimal? d_Utilidad { get; set; }
        public decimal? d_PrecioMayorista { get; set; }
        public int? i_IdPerfilDetraccion{ get; set; }
        public int? i_SolicitarNroSerieIngreso{ get; set; }
        public int? i_SolicitarNroLoteIngreso{ get; set; }
        public int? i_SolicitaOrdenProduccionIngreso{ get; set; }
        public int? i_SolicitarNroSerieSalida{ get; set; }
        public int? i_SolicitarNroLoteSalida{ get; set; }
        public int? i_SolicitaOrdenProduccionSalida{ get; set; }
        public string v_AccionFarmaco{ get; set; }
        public string v_Presentacion{ get; set; }
        public string v_Concentracion{ get; set; }
        public string v_PrincipioActivo{ get; set; }
        public string v_Laboratorio{ get; set; }
    }
}
