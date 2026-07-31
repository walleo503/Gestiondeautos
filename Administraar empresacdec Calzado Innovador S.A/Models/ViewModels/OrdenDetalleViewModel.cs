namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Models.ViewModels
{
    public class OrdenDetalleViewModel
    {
        public OrdenProduccion Orden { get; set; } = null!;
        public int TotalProcesos { get; set; }
        public int ProcesosCompletados { get; set; }
        public int PorcentajeAvance { get; set; }
    }
}
