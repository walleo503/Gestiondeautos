using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;

namespace Gestion_de_autos.Controllers
{
    // Zona publica para compradores: no requiere sesion.
    // Solo muestra informacion de interes para quien compra (popularidad),
    // nunca datos financieros del vendedor (esos estan en EstadisticasController, protegido).
    public class PublicoController : Controller
    {
        private readonly AppDbContext _context;
        public PublicoController(AppDbContext context) => _context = context;

        public async Task<IActionResult> MasVendidos()
        {
            var lista = await _context.VehiculosMasVendidos
                .OrderByDescending(v => v.VecesVendido)
                .ToListAsync();
            return View(lista);
        }
    }
}
