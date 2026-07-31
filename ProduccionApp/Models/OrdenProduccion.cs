using System.ComponentModel.DataAnnotations;

namespace ProduccionApp.Models
{
    public class OrdenProduccion : IValidatableObject
    {
        public int OrdenProduccionId { get; set; }

        [Required(ErrorMessage = "El código de la orden es obligatorio.")]
        [StringLength(20, ErrorMessage = "El código no puede exceder 20 caracteres.")]
        [Display(Name = "Código de Orden")]
        public string Codigo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre del producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El producto no puede exceder 100 caracteres.")]
        [Display(Name = "Producto")]
        public string Producto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad a producir es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad a producir debe ser mayor que cero.")]
        [Display(Name = "Cantidad a Producir")]
        public int CantidadAProducir { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha de entrega estimada es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de Entrega Estimada")]
        public DateTime FechaEntregaEstimada { get; set; } = DateTime.Today.AddDays(7);

        [Required]
        [Display(Name = "Estado")]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        [StringLength(200, ErrorMessage = "Las observaciones no pueden exceder 200 caracteres.")]
        [Display(Name = "Observaciones")]
        public string? Observaciones { get; set; }

        public ICollection<OrdenProceso> OrdenProcesos { get; set; } = new List<OrdenProceso>();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (FechaEntregaEstimada < FechaCreacion)
            {
                yield return new ValidationResult(
                    "La fecha de entrega estimada no puede ser anterior a la fecha de creación.",
                    new[] { nameof(FechaEntregaEstimada) });
            }
        }

        [Display(Name = "% de Avance")]
        public int PorcentajeAvance
        {
            get
            {
                if (OrdenProcesos == null || OrdenProcesos.Count == 0) return 0;
                var completados = OrdenProcesos.Count(op => op.Completado);
                return (int)Math.Round(completados * 100.0 / OrdenProcesos.Count);
            }
        }
    }
}
