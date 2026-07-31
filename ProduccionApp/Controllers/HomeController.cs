using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProduccionApp.Data;
using ProduccionApp.Models;

namespace ProduccionApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;

        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalOrdenes = await _context.OrdenesProduccion.CountAsync();
            ViewBag.TotalProcesos = await _context.ProcesosFabricacion.CountAsync();
            ViewBag.OrdenesPendientes = await _context.OrdenesProduccion.CountAsync(o => o.Estado == EstadoOrden.Pendiente);
            ViewBag.OrdenesEnProceso = await _context.OrdenesProduccion.CountAsync(o => o.Estado == EstadoOrden.EnProceso);
            ViewBag.OrdenesCompletadas = await _context.OrdenesProduccion.CountAsync(o => o.Estado == EstadoOrden.Completada);

            var ordenesRecientes = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .OrderByDescending(o => o.OrdenProduccionId)
                .Take(5)
                .ToListAsync();

            return View(ordenesRecientes);
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
