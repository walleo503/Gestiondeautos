using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("fotos_auto")]
    public class FotoAuto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("datos_auto")]
        public int DatosAutoId { get; set; }

        [ForeignKey("DatosAutoId")]
        public DatosAuto? Vehiculo { get; set; }

        [Required]
        [StringLength(255)]
        public string Ruta { get; set; } = string.Empty;

        [Column("fecha_subida")]
        public DateTime FechaSubida { get; set; } = DateTime.Now;
    }
}
