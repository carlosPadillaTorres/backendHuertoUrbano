using Huerto_Urbano_Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Huerto_Urbano_Backend.Contexto
{
    public class ContextoBdEmployee: DbContext
    {
        public ContextoBdEmployee(DbContextOptions<ContextoBdEmployee> options) : base(options)
        { }
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Estado> Estado { get; set; }
        public DbSet<Ciudad> Ciudad { get; set; }
        public DbSet<Domicilio> Domicilio { get; set; }
        public DbSet<Persona> Persona { get; set; }
        public DbSet<Empleado> Empleado { get; set; }
        public DbSet<Producto> Producto { get; set; }
        public DbSet<LoteProducto> LoteProducto { get; set; }
        public DbSet<Servicio> Servicio { get; set; }
        public DbSet<Asesoria> Asesoria { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuración adicional si es necesario
        }
    }
}
