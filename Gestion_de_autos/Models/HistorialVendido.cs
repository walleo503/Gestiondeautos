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
        [Column("comprador_nombre")]
        [Display(Name = "Nombre del comprador")]
        public string CompradorNombre { get; set; } = string.Empty;

        [StringLength(100)]
        [Column("comprador_telefono")]
        [Display(Name = "Telefono del comprador")]
        public string? CompradorTelefono { get; set; }

        [Required]
        [Column("precio_final", TypeName = "decimal(10,2)")]
        [Display(Name = "Precio final")]
        public decimal PrecioFinal { get; set; }

        [Required]
        [Column("tipo_venta")]
        [Display(Name = "Tipo de venta")]
        public string TipoVenta { get; set; } = "original"; // 'original' o 'reparado'

        [Required]
        [DataType(DataType.Date)]
        [Column("fecha_venta")]
        [Display(Name = "Fecha de venta")]
        public DateTime FechaVenta { get; set; } = DateTime.Today;
    }
}
