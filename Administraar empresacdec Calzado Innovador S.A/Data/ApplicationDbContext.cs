using Microsoft.EntityFrameworkCore;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<OrdenProduccion> OrdenesProduccion { get; set; } = null!;
        public DbSet<ProcesoFabricacion> ProcesosFabricacion { get; set; } = null!;
        public DbSet<OrdenProceso> OrdenProcesos { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdenProceso>()
                .HasKey(op => new { op.OrdenProduccionId, op.ProcesoFabricacionId });

            modelBuilder.Entity<OrdenProceso>()
                .HasOne(op => op.OrdenProduccion)
                .WithMany(o => o.OrdenProcesos)
                .HasForeignKey(op => op.OrdenProduccionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<OrdenProceso>()
                .HasOne(op => op.ProcesoFabricacion)
                .WithMany(p => p.OrdenProcesos)
                .HasForeignKey(op => op.ProcesoFabricacionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ProcesoFabricacion>()
                .HasIndex(p => p.Nombre)
                .IsUnique();

            modelBuilder.Entity<OrdenProduccion>()
                .HasIndex(o => o.NumeroOrden)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
