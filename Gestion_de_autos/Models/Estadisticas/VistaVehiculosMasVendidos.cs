using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    [Table("vista_vehiculos_mas_vendidos")]
    public class VistaVehiculosMasVendidos
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        [Column("veces_vendido")]
        public int VecesVendido { get; set; }
        [Column("total_generado")]
        public decimal TotalGenerado { get; set; }
    }
}
