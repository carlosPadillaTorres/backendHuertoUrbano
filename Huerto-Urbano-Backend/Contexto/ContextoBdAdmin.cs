using Huerto_Urbano_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Huerto_Urbano_Backend.Contexto
{
    public class ContextoBdAdmin: DbContext
    {
        public ContextoBdAdmin(DbContextOptions<ContextoBdAdmin> options) : base(options)
        { }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Ciudad> Ciudad { get; set; }
        public DbSet<Domicilio> Domicilio { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Proveedor> Proveedor { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<LoteProducto> LoteProducto { get; set; }
        public DbSet<ComprasRealizadas> ComprasRealizadas { get; set; }
        public DbSet<DetalleCompra> DetalleCompra { get; set; }
        public DbSet<Venta> Venta { get; set; }
        public DbSet<DetalleVenta> DetalleVenta { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Asesoria> Asesoria { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Usuario>().HasKey(u => u.IdUsuario);

            base.OnModelCreating(modelBuilder);
            // Configuración adicional si es necesario
        }
    }
}
