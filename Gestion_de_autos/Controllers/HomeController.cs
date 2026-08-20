using System.Diagnostics;
using Gestion_de_autos.Models;
using Gestion_de_autos.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Gestion_de_autos.Controllers
{
    // Pagina de inicio: catalogo publico de vehiculos disponibles.
    // No requiere sesion: cualquier comprador que entre a la pagina lo ve.
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var disponibles = await _context.DatosAuto
                .Include(a => a.Fotos)
                .Where(a => a.Estado == "disponible")
                .OrderByDescending(a => a.Id)
                .ToListAsync();
            return View(disponibles);
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
