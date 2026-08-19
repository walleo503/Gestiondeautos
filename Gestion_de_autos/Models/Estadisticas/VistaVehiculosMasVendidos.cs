using System.ComponentModel.DataAnnotations.Schema;

namespace Gestion_de_autos.Models.Estadisticas
{
    [Table("vista_vehiculos_mas_vendidos")]
    public class VistaVehiculosMasVendidos
    {
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public int VecesVendido { get; set; }
        public decimal TotalGenerado { get; set; }
    }
}
