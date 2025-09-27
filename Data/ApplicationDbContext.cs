using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PortalInmobiliario.Models;

namespace PortalInmobiliario.Data;
public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}
    public DbSet<Inmueble> Inmuebles => Set<Inmueble>();
    public DbSet<Visita>   Visitas    => Set<Visita>();
    public DbSet<Reserva>  Reservas   => Set<Reserva>();


    protected override void OnModelCreating(ModelBuilder b)
    {
        base.OnModelCreating(b);

        
        b.Entity<Inmueble>().HasIndex(i => i.Codigo).IsUnique();

       
        b.Entity<Inmueble>().HasData(
            new Inmueble{ Id=1, Codigo="DEP-001", Titulo="Depa Miraflores",
              Tipo=TipoInmueble.Departamento, Ciudad="Lima", Direccion="Av. Larco 123",
              Dormitorios=2, Banos=2, MetrosCuadrados=78, Precio=420000, Activo=true },
            new Inmueble{ Id=2, Codigo="CAS-002", Titulo="Casa Surco",
              Tipo=TipoInmueble.Casa, Ciudad="Lima", Direccion="Calle Robles 456",
              Dormitorios=3, Banos=3, MetrosCuadrados=140, Precio=720000, Activo=true },
            new Inmueble{ Id=3, Codigo="OFI-003", Titulo="Oficina San Isidro",
              Tipo=TipoInmueble.Oficina, Ciudad="Lima", Direccion="Navarrete 789",
              Dormitorios=0, Banos=2, MetrosCuadrados=95, Precio=650000, Activo=true }
        );
    }
}
