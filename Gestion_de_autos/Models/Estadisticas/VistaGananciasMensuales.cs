using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    // Sin llave primaria: mapea la VIEW de solo lectura vista_ganancias_mensuales
    [Table("vista_ganancias_mensuales")]
    public class VistaGananciasMensuales
    {
        public int UsuarioId { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int Mes { get; set; }
        public int AutosVendidos { get; set; }
        public decimal TotalVentas { get; set; }
        public decimal TotalCosto { get; set; }
        public decimal GananciaTotal { get; set; }
    }
}
