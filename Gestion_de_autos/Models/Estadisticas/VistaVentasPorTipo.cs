using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    // Mapea vista_ventas_por_tipo: ganancia separada entre autos vendidos
    // en su estado original y autos vendidos reparados.
    [Table("vista_ventas_por_tipo")]
    public class VistaVentasPorTipo
    {
        [Column("tipo_venta")]
        public string TipoVenta { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        [Column("ganancia_total")]
        public decimal GananciaTotal { get; set; }
    }
}
