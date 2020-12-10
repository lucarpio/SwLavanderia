using Microsoft.EntityFrameworkCore;
using SwLavanderia.Models;

namespace SwLavanderia.Data
{
    public class LavanderiaContext : DbContext
    {
        public DbSet<Almacen> Almacenes {get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> servicios { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<DetalleServicio> detalleServicios { get; set; }
        public LavanderiaContext(DbContextOptions dbo) : base(dbo)
        {
            
        }
    }
}