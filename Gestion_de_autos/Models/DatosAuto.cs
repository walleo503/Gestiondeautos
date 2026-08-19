using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("datos_auto")]
    public class DatosAuto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Vendedor")]
        public int Usuario { get; set; }

        [ForeignKey("Usuario")]
        public Usuario? Vendedor { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria")]
        [StringLength(100)]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio")]
        [StringLength(100)]
        public string Modelo { get; set; } = string.Empty;

        [Required]
        [Column("costo_compra", TypeName = "decimal(10,2)")]
        [Display(Name = "Costo de compra")]
        public decimal CostoCompra { get; set; }

        [Required]
        [Column("precio_venta", TypeName = "decimal(10,2)")]
        [Display(Name = "Precio de venta")]
        public decimal PrecioVenta { get; set; }

        [Required]
        [StringLength(255)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("danos")]
        [Display(Name = "Danos")]
        public string Danos { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        [Column("piezas_faltantes")]
        [Display(Name = "Piezas faltantes")]
        public string PiezasFaltantes { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Estado { get; set; } = "disponible";
    }
}
