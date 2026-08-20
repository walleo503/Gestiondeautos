using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Models;
using Gestion_de_autos.Models.Estadisticas;

namespace Gestion_de_autos.Data
{
    // Esta clase es el puente entre C# y la base de datos MySQL.
    // Cada DbSet representa una tabla; EF Core traduce LINQ a SQL automaticamente.
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<DatosAuto> DatosAuto { get; set; }
        public DbSet<CotizacionReparacion> CotizacionesReparacion { get; set; }
        public DbSet<HistorialVendido> HistorialVendidos { get; set; }
        public DbSet<ListaAuto> ListaAutos { get; set; }
        public DbSet<FotoAuto> FotosAuto { get; set; }
        public DbSet<LoginLog> LoginLogs { get; set; }

        // Las 3 vistas de estadisticas (solo lectura)
        public DbSet<VistaGananciasMensuales> GananciasMensuales { get; set; }
        public DbSet<VistaVehiculosMasVendidos> VehiculosMasVendidos { get; set; }
        public DbSet<VistaGananciasPorVendedor> GananciasPorVendedor { get; set; }
        public DbSet<VistaVentasPorTipo> VentasPorTipo { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Las VIEW de MySQL no tienen llave primaria propia, hay que avisarle a EF Core
            modelBuilder.Entity<VistaGananciasMensuales>().HasNoKey().ToView("vista_ganancias_mensuales");
            modelBuilder.Entity<VistaVehiculosMasVendidos>().HasNoKey().ToView("vista_vehiculos_mas_vendidos");
            modelBuilder.Entity<VistaGananciasPorVendedor>().HasNoKey().ToView("vista_ganancias_por_vendedor");
            modelBuilder.Entity<VistaVentasPorTipo>().HasNoKey().ToView("vista_ventas_por_tipo");
        }
    }
}
