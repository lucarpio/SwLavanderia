using Microsoft.EntityFrameworkCore;
using SwLavanderia.Models;

namespace SwLavanderia.Data
{
    public class LavanderiaContext : DbContext
    {
        public DbSet<Almacen> Almacenes {get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public LavanderiaContext(DbContextOptions dbo) : base(dbo)
        {
            
        }
    }
}