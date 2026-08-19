using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;
using Gestion_de_autos.Models;

namespace Gestion_de_autos.Controllers
{
    // Administra la tabla cotizacion_reparacion (presupuestos de reparacion por vehiculo)
    public class CotizacionesController : Controller
    {
        private readonly AppDbContext _context;
        public CotizacionesController(AppDbContext context) => _context = context;

        public async Task<IActionResult> Index()
        {
            var lista = _context.CotizacionesReparacion.Include(c => c.Vendedor).Include(c => c.Vehiculo);
            return View(await lista.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var cot = await _context.CotizacionesReparacion.Include(c => c.Vendedor).Include(c => c.Vehiculo)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cot == null) return NotFound();
            return View(cot);
        }

        public IActionResult Create()
        {
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            ViewData["DatosAutoId"] = new SelectList(_context.DatosAuto, "Id", "Marca");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario,DatosAutoId,Pieza,Modelo,Precio,Otro,ManoDeObra,Total")] CotizacionReparacion cot)
        {
            if (ModelState.IsValid)
            {
                // Calcula el total automaticamente = precio de la pieza + mano de obra
                cot.Total = cot.Precio + cot.ManoDeObra;
                _context.Add(cot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", cot.Usuario);
            ViewData["DatosAutoId"] = new SelectList(_context.DatosAuto, "Id", "Marca", cot.DatosAutoId);
            return View(cot);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var cot = await _context.CotizacionesReparacion.FindAsync(id);
            if (cot == null) return NotFound();
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", cot.Usuario);
            ViewData["DatosAutoId"] = new SelectList(_context.DatosAuto, "Id", "Marca", cot.DatosAutoId);
            return View(cot);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,DatosAutoId,Pieza,Modelo,Precio,Otro,ManoDeObra,Total")] CotizacionReparacion cot)
        {
            if (id != cot.Id) return NotFound();
            if (ModelState.IsValid)
            {
                cot.Total = cot.Precio + cot.ManoDeObra;
                _context.Update(cot);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", cot.Usuario);
            ViewData["DatosAutoId"] = new SelectList(_context.DatosAuto, "Id", "Marca", cot.DatosAutoId);
            return View(cot);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var cot = await _context.CotizacionesReparacion.Include(c => c.Vendedor).Include(c => c.Vehiculo)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (cot == null) return NotFound();
            return View(cot);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var cot = await _context.CotizacionesReparacion.FindAsync(id);
            if (cot != null)
            {
                _context.CotizacionesReparacion.Remove(cot);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
