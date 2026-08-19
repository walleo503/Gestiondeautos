using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    // Sin llave primaria: mapea la VIEW de solo lectura vista_ganancias_mensuales
    [Table("vista_ganancias_mensuales")]
    public class VistaGananciasMensuales
    {
        [Column("usuario_id")]
        public int UsuarioId { get; set; }
        public string Vendedor { get; set; } = string.Empty;
        public int Anio { get; set; }
        public int Mes { get; set; }
        [Column("autos_vendidos")]
        public int AutosVendidos { get; set; }
        [Column("total_ventas")]
        public decimal TotalVentas { get; set; }
        [Column("total_costo")]
        public decimal TotalCosto { get; set; }
        [Column("ganancia_total")]
        public decimal GananciaTotal { get; set; }
    }
}
