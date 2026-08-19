using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("lista_autos")]
    public class ListaAuto
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Usuario { get; set; }

        [ForeignKey("Usuario")]
        public Usuario? Comprador { get; set; }

        [Required]
        [Column("datos_auto")]
        public int DatosAutoId { get; set; }

        [ForeignKey("DatosAutoId")]
        public DatosAuto? Vehiculo { get; set; }

        public DateTime FechaAgregado { get; set; } = DateTime.Now;
    }
}
