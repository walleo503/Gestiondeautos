using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProduccionApp.Models
{
    public class OrdenProceso
    {
        public int OrdenProcesoId { get; set; }

        [Required]
        [Display(Name = "Orden de Producción")]
        public int OrdenProduccionId { get; set; }

        [ForeignKey(nameof(OrdenProduccionId))]
        public OrdenProduccion? OrdenProduccion { get; set; }

        [Required]
        [Display(Name = "Proceso de Fabricación")]
        public int ProcesoFabricacionId { get; set; }

        [ForeignKey(nameof(ProcesoFabricacionId))]
        public ProcesoFabricacion? ProcesoFabricacion { get; set; }

        [Display(Name = "Completado")]
        public bool Completado { get; set; } = false;

        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Completado")]
        public DateTime? FechaCompletado { get; set; }

        [Range(1, 100, ErrorMessage = "La secuencia debe ser mayor que cero.")]
        [Display(Name = "Secuencia")]
        public int Secuencia { get; set; } = 1;
    }
}
