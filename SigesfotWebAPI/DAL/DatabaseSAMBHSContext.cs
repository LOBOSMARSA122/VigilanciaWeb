using System.Data.Entity;
using BE.Z_CommonSAMBHS;
using SAMBHSBE.Common;
using SAMBHSBE.Documento;
using SAMBHSBE.EstablecimientoDetalle;

namespace SAMBHSDAL
{
    public class DatabaseSAMBHSContext : DbContext
    {
        public DatabaseSAMBHSContext() : base("name=BDSambhs") { }

        public DbSet<DocumentoBE> Documento { get; set; }
        public DbSet<EstablecimientoDetalleBE> EstablecimientoDetalle { get; set; }
        public DbSet<DataHierarchyBE> DataHierarchy { get; set; }
        public DbSet<VendedorBE> Vendedor { get; set; }
        public DbSet<ClienteBE> Cliente { get; set; }
        public DbSet<SystemParameterBE> SystemParameter { get; set; }
        public DbSet<SystemUserBE> SystemUser { get; set; }
        public DbSet<ClienteDireccionesBE> ClienteDirecciones { get; set; }
        public DbSet<ProductoBE> Producto { get; set; }
        public DbSet<ProductoDetalleBE> ProductoDetalle { get; set; }
        public DbSet<ProductoAlmacenBE> ProductoAlmacen { get; set; }
        public DbSet<LineaBE> Linea { get; set; }
        public DbSet<MovimientoBE> Movimiento { get; set; }
        public DbSet<MovimientoDetalleBE> MovimientoDetalle { get; set; }
    }
}
