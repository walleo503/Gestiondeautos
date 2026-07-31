using System.ComponentModel.DataAnnotations;

namespace ProduccionApp.Models
{
    public class ProcesoFabricacion
    {
        public int ProcesoFabricacionId { get; set; }

        [Required(ErrorMessage = "El nombre del proceso es obligatorio.")]
        [StringLength(80, ErrorMessage = "El nombre no puede exceder 80 caracteres.")]
        [Display(Name = "Nombre del Proceso")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "La descripción no puede exceder 250 caracteres.")]
        [Display(Name = "Descripción")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "La duración estimada es obligatoria.")]
        [Range(1, 1000, ErrorMessage = "La duración estimada debe ser mayor que cero.")]
        [Display(Name = "Duración Estimada (horas)")]
        public int DuracionEstimadaHoras { get; set; }

        public ICollection<OrdenProceso> OrdenProcesos { get; set; } = new List<OrdenProceso>();
    }
}
