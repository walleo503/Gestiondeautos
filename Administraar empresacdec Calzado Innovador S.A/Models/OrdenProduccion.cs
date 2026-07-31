using System.ComponentModel.DataAnnotations;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.Enums;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Models
{
    public class OrdenProduccion
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de orden es obligatorio.")]
        [StringLength(30, ErrorMessage = "El número de orden no puede superar los 30 caracteres.")]
        [Display(Name = "Número de orden")]
        public string NumeroOrden { get; set; } = string.Empty;

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El producto no puede superar los 100 caracteres.")]
        public string Producto { get; set; } = string.Empty;

        [Required(ErrorMessage = "La cantidad a producir es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad a producir debe ser mayor que cero.")]
        [Display(Name = "Cantidad a producir")]
        public int CantidadAProducir { get; set; }

        [Required(ErrorMessage = "La fecha de creación es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de creación")]
        public DateTime FechaCreacion { get; set; } = DateTime.Today;

        [Required(ErrorMessage = "La fecha estimada de entrega es obligatoria.")]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha estimada de entrega")]
        public DateTime FechaEntregaEstimada { get; set; } = DateTime.Today.AddDays(7);

        [Required]
        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        [StringLength(500, ErrorMessage = "Las observaciones no pueden superar los 500 caracteres.")]
        public string? Observaciones { get; set; }

        public ICollection<OrdenProceso> OrdenProcesos { get; set; } = new List<OrdenProceso>();
    }
}
