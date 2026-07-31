using System.Diagnostics;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Data;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.Enums;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .ToListAsync();

            var viewModel = new DashboardViewModel
            {
                TotalOrdenes = ordenes.Count,
                OrdenesPendientes = ordenes.Count(o => o.Estado == EstadoOrden.Pendiente),
                OrdenesEnProceso = ordenes.Count(o => o.Estado == EstadoOrden.EnProceso),
                OrdenesCompletadas = ordenes.Count(o => o.Estado == EstadoOrden.Completada),
                TotalProcesos = await _context.ProcesosFabricacion.CountAsync()
            };

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
