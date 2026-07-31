using System.ComponentModel.DataAnnotations;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Models
{
    public class ProcesoFabricacion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre del proceso es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        [Display(Name = "Nombre del proceso")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "La descripción no puede superar los 300 caracteres.")]
        public string? Descripcion { get; set; }

        [Range(1, 1000, ErrorMessage = "La duración estimada debe ser mayor que cero.")]
        [Display(Name = "Duración estimada (horas)")]
        public int DuracionEstimadaHoras { get; set; }

        public ICollection<OrdenProceso> OrdenProcesos { get; set; } = new List<OrdenProceso>();
    }
}
