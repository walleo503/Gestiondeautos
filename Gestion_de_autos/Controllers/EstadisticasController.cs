using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;

namespace Gestion_de_autos.Controllers
{
    // Este controlador NO escribe datos: solo lee las 3 VIEWS de MySQL
    // (vista_ganancias_mensuales, vista_vehiculos_mas_vendidos, vista_ganancias_por_vendedor)
    // que ya vienen calculadas desde la base de datos.
    public class EstadisticasController : Controller
    {
        private readonly AppDbContext _context;
        public EstadisticasController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            ViewData["GananciasMensuales"] = await _context.GananciasMensuales
                .OrderByDescending(g => g.Anio).ThenByDescending(g => g.Mes).ToListAsync();

            ViewData["VehiculosMasVendidos"] = await _context.VehiculosMasVendidos
                .OrderByDescending(v => v.VecesVendido).ToListAsync();

            ViewData["GananciasPorVendedor"] = await _context.GananciasPorVendedor
                .OrderByDescending(g => g.GananciaTotal).ToListAsync();

            return View();
        }
    }
}
