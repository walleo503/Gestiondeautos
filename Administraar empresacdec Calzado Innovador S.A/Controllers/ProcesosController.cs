using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Data;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Controllers
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
            if (await _context.ProcesosFabricacion.AnyAsync(p => p.Nombre == proceso.Nombre))
            {
                ModelState.AddModelError(nameof(proceso.Nombre), "Ya existe un proceso con ese nombre.");
            }

            if (!ModelState.IsValid)
            {
                return View(proceso);
            }

            _context.ProcesosFabricacion.Add(proceso);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Proceso creado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proceso = await _context.ProcesosFabricacion.FindAsync(id);
            if (proceso == null)
            {
                return NotFound();
            }

            return View(proceso);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProcesoFabricacion proceso)
        {
            if (id != proceso.Id)
            {
                return NotFound();
            }

            if (await _context.ProcesosFabricacion.AnyAsync(p => p.Nombre == proceso.Nombre && p.Id != id))
            {
                ModelState.AddModelError(nameof(proceso.Nombre), "Ya existe un proceso con ese nombre.");
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
                if (!await _context.ProcesosFabricacion.AnyAsync(p => p.Id == id))
                {
                    return NotFound();
                }
                throw;
            }

            TempData["Mensaje"] = "Proceso actualizado correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var proceso = await _context.ProcesosFabricacion
                .Include(p => p.OrdenProcesos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proceso == null)
            {
                return NotFound();
            }

            return View(proceso);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var proceso = await _context.ProcesosFabricacion
                .Include(p => p.OrdenProcesos)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proceso == null)
            {
                return NotFound();
            }

            if (proceso.OrdenProcesos.Any())
            {
                TempData["Error"] = "No se puede eliminar el proceso porque está asociado a una o más órdenes.";
                return RedirectToAction(nameof(Index));
            }

            _context.ProcesosFabricacion.Remove(proceso);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Proceso eliminado correctamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
