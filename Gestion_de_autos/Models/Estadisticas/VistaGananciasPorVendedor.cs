using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    [Table("vista_ganancias_por_vendedor")]
    public class VistaGananciasPorVendedor
    {
        public int UsuarioId { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public int AutosVendidos { get; set; }
        public decimal GananciaTotal { get; set; }
    }
}
