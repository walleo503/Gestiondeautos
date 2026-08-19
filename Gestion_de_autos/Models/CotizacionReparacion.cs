using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("cotizacion_reparacion")]
    public class CotizacionReparacion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vendedor")]
        public int Usuario { get; set; }

        [ForeignKey("Usuario")]
        public Usuario? Vendedor { get; set; }

        [Required]
        [Column("datos_auto")]
        [Display(Name = "Vehiculo")]
        public int DatosAutoId { get; set; }

        [ForeignKey("DatosAutoId")]
        public DatosAuto? Vehiculo { get; set; }

        [Required]
        [StringLength(100)]
        public string Pieza { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Precio { get; set; }

        [StringLength(100)]
        public string? Otro { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Mano de obra")]
        public decimal ManoDeObra { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Total { get; set; }
    }
}
