using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("historial_vendidos")]
    public class HistorialVendido
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
        [Display(Name = "Nombre del comprador")]
        public string CompradorNombre { get; set; } = string.Empty;

        [StringLength(100)]
        [Display(Name = "Telefono del comprador")]
        public string? CompradorTelefono { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        [Display(Name = "Precio final")]
        public decimal PrecioFinal { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de venta")]
        public DateTime FechaVenta { get; set; } = DateTime.Today;
    }
}
