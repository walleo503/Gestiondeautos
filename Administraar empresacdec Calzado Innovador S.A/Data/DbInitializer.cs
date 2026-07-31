using Administraar_empresacdec_Calzado_Innovador_S.A_.Models;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.ProcesosFabricacion.Any())
            {
                return;
            }

            var procesos = new List<ProcesoFabricacion>
            {
                new ProcesoFabricacion { Nombre = "Corte", Descripcion = "Corte de piezas de cuero y textil según molde.", DuracionEstimadaHoras = 4 },
                new ProcesoFabricacion { Nombre = "Costura", Descripcion = "Unión de piezas cortadas para formar el corte del calzado.", DuracionEstimadaHoras = 6 },
                new ProcesoFabricacion { Nombre = "Ensamblado", Descripcion = "Unión del corte con la suela y plantilla.", DuracionEstimadaHoras = 5 },
                new ProcesoFabricacion { Nombre = "Control de calidad", Descripcion = "Revisión de acabados y estándares de calidad.", DuracionEstimadaHoras = 2 },
                new ProcesoFabricacion { Nombre = "Empaque", Descripcion = "Empaquetado final y etiquetado del producto.", DuracionEstimadaHoras = 1 }
            };

            context.ProcesosFabricacion.AddRange(procesos);
            context.SaveChanges();
        }
    }
}
