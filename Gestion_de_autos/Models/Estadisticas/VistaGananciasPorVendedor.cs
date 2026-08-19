using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    [Table("vista_ganancias_por_vendedor")]
    public class VistaGananciasPorVendedor
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        [Column("autos_vendidos")]
        public int AutosVendidos { get; set; }
        [Column("ganancia_total")]
        public decimal GananciaTotal { get; set; }
    }
}
