using Microsoft.EntityFrameworkCore;
using ProduccionApp.Models;

namespace ProduccionApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OrdenProduccion> OrdenesProduccion { get; set; } = null!;
        public DbSet<ProcesoFabricacion> ProcesosFabricacion { get; set; } = null!;
        public DbSet<OrdenProceso> OrdenProcesos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrdenProduccion>()
                .HasIndex(o => o.Codigo)
                .IsUnique();

            modelBuilder.Entity<OrdenProduccion>()
                .Property(o => o.Estado)
                .HasConversion<string>()
                .HasMaxLength(20);

            modelBuilder.Entity<ProcesoFabricacion>()
                .HasIndex(p => p.Nombre)
                .IsUnique();

            modelBuilder.Entity<OrdenProceso>()
                .HasIndex(op => new { op.OrdenProduccionId, op.ProcesoFabricacionId })
                .IsUnique();

            modelBuilder.Entity<OrdenProceso>()
                .HasOne(op => op.OrdenProduccion)
                .WithMany(o => o.OrdenProcesos)
                .HasForeignKey(op => op.OrdenProduccionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdenProceso>()
                .HasOne(op => op.ProcesoFabricacion)
                .WithMany(p => p.OrdenProcesos)
                .HasForeignKey(op => op.ProcesoFabricacionId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
