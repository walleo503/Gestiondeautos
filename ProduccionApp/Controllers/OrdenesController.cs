using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProduccionApp.Data;
using ProduccionApp.Models;
using ProduccionApp.Models.ViewModels;

namespace ProduccionApp.Controllers
{
    public class OrdenesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrdenesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var ordenes = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .OrderByDescending(o => o.OrdenProduccionId)
                .ToListAsync();

            return View(ordenes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                    .ThenInclude(op => op.ProcesoFabricacion)
                .FirstOrDefaultAsync(o => o.OrdenProduccionId == id);

            if (orden == null) return NotFound();

            return View(orden);
        }

        public async Task<IActionResult> Create()
        {
            var vm = new OrdenFormViewModel
            {
                ProcesosDisponibles = await _context.ProcesosFabricacion.OrderBy(p => p.Nombre).ToListAsync()
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenFormViewModel vm)
        {
            if (vm.ProcesosSeleccionados == null || vm.ProcesosSeleccionados.Count == 0)
            {
                ModelState.AddModelError(nameof(vm.ProcesosSeleccionados),
                    "La orden debe tener al menos un proceso de fabricación asociado.");
            }

            if (await _context.OrdenesProduccion.AnyAsync(o => o.Codigo == vm.Codigo))
            {
                ModelState.AddModelError(nameof(vm.Codigo), "Ya existe una orden con este código.");
            }

            if (vm.FechaEntregaEstimada < vm.FechaCreacion)
            {
                ModelState.AddModelError(nameof(vm.FechaEntregaEstimada),
                    "La fecha de entrega estimada no puede ser anterior a la fecha de creación.");
            }

            if (!ModelState.IsValid)
            {
                vm.ProcesosDisponibles = await _context.ProcesosFabricacion.OrderBy(p => p.Nombre).ToListAsync();
                return View(vm);
            }

            var orden = new OrdenProduccion
            {
                Codigo = vm.Codigo,
                Producto = vm.Producto,
                CantidadAProducir = vm.CantidadAProducir,
                FechaCreacion = vm.FechaCreacion,
                FechaEntregaEstimada = vm.FechaEntregaEstimada,
                Estado = vm.Estado,
                Observaciones = vm.Observaciones
            };

            int secuencia = 1;
            foreach (var procesoId in vm.ProcesosSeleccionados.Distinct())
            {
                orden.OrdenProcesos.Add(new OrdenProceso
                {
                    ProcesoFabricacionId = procesoId,
                    Secuencia = secuencia++,
                    Completado = false
                });
            }

            _context.OrdenesProduccion.Add(orden);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"La orden '{orden.Codigo}' fue creada exitosamente con {orden.OrdenProcesos.Count} proceso(s) asociado(s).";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.OrdenProduccionId == id);

            if (orden == null) return NotFound();

            var vm = new OrdenFormViewModel
            {
                OrdenProduccionId = orden.OrdenProduccionId,
                Codigo = orden.Codigo,
                Producto = orden.Producto,
                CantidadAProducir = orden.CantidadAProducir,
                FechaCreacion = orden.FechaCreacion,
                FechaEntregaEstimada = orden.FechaEntregaEstimada,
                Estado = orden.Estado,
                Observaciones = orden.Observaciones,
                ProcesosSeleccionados = orden.OrdenProcesos.Select(op => op.ProcesoFabricacionId).ToList(),
                ProcesosDisponibles = await _context.ProcesosFabricacion.OrderBy(p => p.Nombre).ToListAsync()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrdenFormViewModel vm)
        {
            if (id != vm.OrdenProduccionId) return NotFound();

            if (vm.ProcesosSeleccionados == null || vm.ProcesosSeleccionados.Count == 0)
            {
                ModelState.AddModelError(nameof(vm.ProcesosSeleccionados),
                    "La orden debe tener al menos un proceso de fabricación asociado.");
            }

            if (await _context.OrdenesProduccion.AnyAsync(o => o.Codigo == vm.Codigo && o.OrdenProduccionId != id))
            {
                ModelState.AddModelError(nameof(vm.Codigo), "Ya existe otra orden con este código.");
            }

            if (vm.FechaEntregaEstimada < vm.FechaCreacion)
            {
                ModelState.AddModelError(nameof(vm.FechaEntregaEstimada),
                    "La fecha de entrega estimada no puede ser anterior a la fecha de creación.");
            }

            if (!ModelState.IsValid)
            {
                vm.ProcesosDisponibles = await _context.ProcesosFabricacion.OrderBy(p => p.Nombre).ToListAsync();
                return View(vm);
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.OrdenProduccionId == id);

            if (orden == null) return NotFound();

            orden.Codigo = vm.Codigo;
            orden.Producto = vm.Producto;
            orden.CantidadAProducir = vm.CantidadAProducir;
            orden.FechaCreacion = vm.FechaCreacion;
            orden.FechaEntregaEstimada = vm.FechaEntregaEstimada;
            orden.Estado = vm.Estado;
            orden.Observaciones = vm.Observaciones;

            var seleccionados = vm.ProcesosSeleccionados.Distinct().ToList();

            var aEliminar = orden.OrdenProcesos
                .Where(op => !seleccionados.Contains(op.ProcesoFabricacionId))
                .ToList();
            foreach (var op in aEliminar)
            {
                _context.OrdenProcesos.Remove(op);
            }

            var existentes = orden.OrdenProcesos.Select(op => op.ProcesoFabricacionId).ToList();
            var aAgregar = seleccionados.Where(pid => !existentes.Contains(pid)).ToList();
            int siguienteSecuencia = orden.OrdenProcesos.Any() ? orden.OrdenProcesos.Max(op => op.Secuencia) + 1 : 1;
            foreach (var procesoId in aAgregar)
            {
                orden.OrdenProcesos.Add(new OrdenProceso
                {
                    ProcesoFabricacionId = procesoId,
                    Secuencia = siguienteSecuencia++,
                    Completado = false
                });
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = $"La orden '{orden.Codigo}' fue actualizada exitosamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                    .ThenInclude(op => op.ProcesoFabricacion)
                .FirstOrDefaultAsync(o => o.OrdenProduccionId == id);

            if (orden == null) return NotFound();

            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.OrdenProduccionId == id);

            if (orden != null)
            {
                _context.OrdenesProduccion.Remove(orden);
                await _context.SaveChangesAsync();
                TempData["Mensaje"] = $"La orden '{orden.Codigo}' fue eliminada exitosamente.";
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MarcarProceso(int ordenProcesoId, int ordenId)
        {
            var ordenProceso = await _context.OrdenProcesos.FindAsync(ordenProcesoId);
            if (ordenProceso == null) return NotFound();

            ordenProceso.Completado = !ordenProceso.Completado;
            ordenProceso.FechaCompletado = ordenProceso.Completado ? DateTime.Today : null;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = ordenId });
        }

        private bool OrdenExists(int id)
        {
            return _context.OrdenesProduccion.Any(e => e.OrdenProduccionId == id);
        }
    }
}
