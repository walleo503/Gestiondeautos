using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProduccionApp.Data;
using ProduccionApp.Models;

namespace ProduccionApp.Controllers
{
    public class ProcesosController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProcesosController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var procesos = await _context.ProcesosFabricacion
                .Include(p => p.OrdenProcesos)
                .OrderBy(p => p.Nombre)
                .ToListAsync();

            return View(procesos);
        }

        public IActionResult Create()
        {
            return View(new ProcesoFabricacion());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProcesoFabricacion proceso)
        {
            if (await _context.ProcesosFabricacion.AnyAsync(p => p.Nombre.ToLower() == proceso.Nombre.ToLower()))
            {
                ModelState.AddModelError(nameof(proceso.Nombre), "Ya existe un proceso de fabricación con este nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(proceso);
            }

            _context.ProcesosFabricacion.Add(proceso);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"El proceso '{proceso.Nombre}' fue creado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var proceso = await _context.ProcesosFabricacion.FindAsync(id);
            if (proceso == null) return NotFound();

            return View(proceso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProcesoFabricacion proceso)
        {
            if (id != proceso.ProcesoFabricacionId) return NotFound();

            if (await _context.ProcesosFabricacion.AnyAsync(p =>
                    p.Nombre.ToLower() == proceso.Nombre.ToLower() && p.ProcesoFabricacionId != id))
            {
                ModelState.AddModelError(nameof(proceso.Nombre), "Ya existe otro proceso de fabricación con este nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(proceso);
            }

            try
            {
                _context.Update(proceso);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProcesoExists(proceso.ProcesoFabricacionId)) return NotFound();
                throw;
            }

            TempData["Mensaje"] = $"El proceso '{proceso.Nombre}' fue actualizado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var proceso = await _context.ProcesosFabricacion
                .Include(p => p.OrdenProcesos)
                .FirstOrDefaultAsync(p => p.ProcesoFabricacionId == id);

            if (proceso == null) return NotFound();

            return View(proceso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proceso = await _context.ProcesosFabricacion
                .Include(p => p.OrdenProcesos)
                .FirstOrDefaultAsync(p => p.ProcesoFabricacionId == id);

            if (proceso == null) return RedirectToAction(nameof(Index));

            if (proceso.OrdenProcesos.Any())
            {
                TempData["Error"] = $"No se puede eliminar el proceso '{proceso.Nombre}' porque está asociado a {proceso.OrdenProcesos.Count} orden(es) de producción.";
                return RedirectToAction(nameof(Index));
            }

            _context.ProcesosFabricacion.Remove(proceso);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"El proceso '{proceso.Nombre}' fue eliminado exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        private bool ProcesoExists(int id)
        {
            return _context.ProcesosFabricacion.Any(e => e.ProcesoFabricacionId == id);
        }
    }
}
