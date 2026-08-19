using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    [Table("usuarios")]
    public class Usuario
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "El apellido es obligatorio")]
        [StringLength(100)]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "El telefono es obligatorio")]
        [StringLength(100)]
        public string Telefono { get; set; } = string.Empty;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [StringLength(100)]
        [EmailAddress]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "El DUI es obligatorio")]
        [StringLength(100)]
        [Column("DUI")]
        [Display(Name = "DUI")]
        public string Dui { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contrasena es obligatoria")]
        [StringLength(100)]
        [DataType(DataType.Password)]
        public string Contrasena { get; set; } = string.Empty;
    }
}
