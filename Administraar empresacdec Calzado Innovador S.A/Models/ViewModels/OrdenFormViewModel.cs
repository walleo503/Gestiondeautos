using System.ComponentModel.DataAnnotations;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.Enums;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Models.ViewModels
{
    public class OrdenFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El número de orden es obligatorio.")]
        [StringLength(30)]
        [Display(Name = "Número de orden")]
        public string NumeroOrden { get; set; } = string.Empty;

        [Required(ErrorMessage = "El producto es obligatorio.")]
        [StringLength(100)]
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

        public EstadoOrden Estado { get; set; } = EstadoOrden.Pendiente;

        [StringLength(500)]
        public string? Observaciones { get; set; }

        [Required(ErrorMessage = "La orden debe tener al menos un proceso asociado.")]
        [MinLength(1, ErrorMessage = "La orden debe tener al menos un proceso asociado.")]
        [Display(Name = "Procesos de fabricación")]
        public List<int> ProcesosSeleccionados { get; set; } = new List<int>();

        public List<ProcesoOpcionViewModel> ProcesosDisponibles { get; set; } = new List<ProcesoOpcionViewModel>();
    }

    public class ProcesoOpcionViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool Seleccionado { get; set; }
    }
}
