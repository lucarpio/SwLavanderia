using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SwLavanderia.Models;

namespace SwLavanderia.Data
{
    public class LavanderiaContext : IdentityDbContext
    {
        public DbSet<Almacen> Almacenes {get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Servicio> Servicios { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<Estado> Estados { get; set; }
        public DbSet<Contact> Contactos { get; set; }
        public DbSet<ServiciosDisponibles> ServiciosDisponibles { get; set; }
        public LavanderiaContext(DbContextOptions dbo) : base(dbo)
        {
            
        }
    }
}