namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Models
{
    public class OrdenProceso
    {
        public int OrdenProduccionId { get; set; }
        public OrdenProduccion OrdenProduccion { get; set; } = null!;

        public int ProcesoFabricacionId { get; set; }
        public ProcesoFabricacion ProcesoFabricacion { get; set; } = null!;

        public bool Completado { get; set; }
        public DateTime? FechaCompletado { get; set; }
        public int Secuencia { get; set; }
    }
}
