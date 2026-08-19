using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;
using Gestion_de_autos.Filters;
using Gestion_de_autos.Models;

namespace Gestion_de_autos.Controllers
{
    // Administra la tabla historial_vendidos (registro de ventas).
    // El trigger "after_venta_insert" en MySQL se encarga de marcar
    // el vehiculo como 'vendido' automaticamente al insertar aqui.
    [SessionAuthorize]
    public class VentasController : Controller
    {
        private readonly AppDbContext _context;
        public VentasController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var lista = _context.HistorialVendidos.Include(v => v.Vendedor).Include(v => v.Vehiculo)
                .OrderByDescending(v => v.FechaVenta);
            return View(await lista.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var venta = await _context.HistorialVendidos.Include(v => v.Vendedor).Include(v => v.Vehiculo)
                .FirstOrDefaultAsync(v => v.Id == id);
            if (venta == null) return NotFound();
            return View(venta);
        }

        // GET /Ventas/Create -> solo muestra autos que siguen 'disponible'
        public IActionResult Create()
        {
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["DatosAutoId"] = new SelectList(
                _context.DatosAuto.Where(a => a.Estado == "disponible"), "Id", "Marca");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario,DatosAutoId,CompradorNombre,CompradorTelefono,PrecioFinal,FechaVenta")] HistorialVendido venta)
        {
            if (ModelState.IsValid)
            {
                _context.Add(venta);
                await _context.SaveChangesAsync();
                // No hace falta actualizar el estado del auto manualmente:
                // el trigger de la base de datos lo pone en 'vendido' automaticamente.
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", venta.Usuario);
            ViewData["DatosAutoId"] = new SelectList(
                _context.DatosAuto.Where(a => a.Estado == "disponible"), "Id", "Marca", venta.DatosAutoId);
            return View(venta);
        }
    }
}
