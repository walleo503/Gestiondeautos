using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models
{
    // Esta clase representa la tabla "login": guarda un registro cada vez
    // que un usuario inicia sesion (historial de accesos), no las credenciales.
    [Table("login")]
    public class LoginLog
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int Usuario { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Contrasena { get; set; } = string.Empty;

        [Column("fecha_login")]
        public DateTime FechaLogin { get; set; } = DateTime.Now;
    }
}
