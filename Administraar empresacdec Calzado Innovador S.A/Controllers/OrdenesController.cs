using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Data;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.Enums;
using Administraar_empresacdec_Calzado_Innovador_S.A_.Models.ViewModels;

namespace Administraar_empresacdec_Calzado_Innovador_S.A_.Controllers
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
                .OrderByDescending(o => o.FechaCreacion)
                .ToListAsync();

            return View(ordenes);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .ThenInclude(op => op.ProcesoFabricacion)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            var total = orden.OrdenProcesos.Count;
            var completados = orden.OrdenProcesos.Count(op => op.Completado);

            var viewModel = new OrdenDetalleViewModel
            {
                Orden = orden,
                TotalProcesos = total,
                ProcesosCompletados = completados,
                PorcentajeAvance = total == 0 ? 0 : (int)Math.Round(completados * 100.0 / total)
            };

            return View(viewModel);
        }

        public async Task<IActionResult> Create()
        {
            var viewModel = new OrdenFormViewModel
            {
                ProcesosDisponibles = await ObtenerProcesosDisponiblesAsync(new List<int>())
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrdenFormViewModel viewModel)
        {
            if (await _context.OrdenesProduccion.AnyAsync(o => o.NumeroOrden == viewModel.NumeroOrden))
            {
                ModelState.AddModelError(nameof(viewModel.NumeroOrden), "Ya existe una orden con ese número.");
            }

            if (viewModel.FechaEntregaEstimada < viewModel.FechaCreacion)
            {
                ModelState.AddModelError(nameof(viewModel.FechaEntregaEstimada), "La fecha de entrega no puede ser anterior a la fecha de creación.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.ProcesosDisponibles = await ObtenerProcesosDisponiblesAsync(viewModel.ProcesosSeleccionados);
                return View(viewModel);
            }

            var orden = new OrdenProduccion
            {
                NumeroOrden = viewModel.NumeroOrden,
                Producto = viewModel.Producto,
                CantidadAProducir = viewModel.CantidadAProducir,
                FechaCreacion = viewModel.FechaCreacion,
                FechaEntregaEstimada = viewModel.FechaEntregaEstimada,
                Estado = EstadoOrden.Pendiente,
                Observaciones = viewModel.Observaciones
            };

            var secuencia = 1;
            foreach (var procesoId in viewModel.ProcesosSeleccionados.Distinct())
            {
                orden.OrdenProcesos.Add(new OrdenProceso
                {
                    ProcesoFabricacionId = procesoId,
                    Secuencia = secuencia++
                });
            }

            _context.OrdenesProduccion.Add(orden);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Orden de producción creada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            var seleccionados = orden.OrdenProcesos.Select(op => op.ProcesoFabricacionId).ToList();

            var viewModel = new OrdenFormViewModel
            {
                Id = orden.Id,
                NumeroOrden = orden.NumeroOrden,
                Producto = orden.Producto,
                CantidadAProducir = orden.CantidadAProducir,
                FechaCreacion = orden.FechaCreacion,
                FechaEntregaEstimada = orden.FechaEntregaEstimada,
                Estado = orden.Estado,
                Observaciones = orden.Observaciones,
                ProcesosSeleccionados = seleccionados,
                ProcesosDisponibles = await ObtenerProcesosDisponiblesAsync(seleccionados)
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrdenFormViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return NotFound();
            }

            if (await _context.OrdenesProduccion.AnyAsync(o => o.NumeroOrden == viewModel.NumeroOrden && o.Id != id))
            {
                ModelState.AddModelError(nameof(viewModel.NumeroOrden), "Ya existe una orden con ese número.");
            }

            if (viewModel.FechaEntregaEstimada < viewModel.FechaCreacion)
            {
                ModelState.AddModelError(nameof(viewModel.FechaEntregaEstimada), "La fecha de entrega no puede ser anterior a la fecha de creación.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.ProcesosDisponibles = await ObtenerProcesosDisponiblesAsync(viewModel.ProcesosSeleccionados);
                return View(viewModel);
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            orden.NumeroOrden = viewModel.NumeroOrden;
            orden.Producto = viewModel.Producto;
            orden.CantidadAProducir = viewModel.CantidadAProducir;
            orden.FechaCreacion = viewModel.FechaCreacion;
            orden.FechaEntregaEstimada = viewModel.FechaEntregaEstimada;
            orden.Estado = viewModel.Estado;
            orden.Observaciones = viewModel.Observaciones;

            var seleccionadosNuevos = viewModel.ProcesosSeleccionados.Distinct().ToList();

            var aEliminar = orden.OrdenProcesos
                .Where(op => !seleccionadosNuevos.Contains(op.ProcesoFabricacionId))
                .ToList();
            foreach (var op in aEliminar)
            {
                orden.OrdenProcesos.Remove(op);
            }

            var existentes = orden.OrdenProcesos.Select(op => op.ProcesoFabricacionId).ToList();
            var secuencia = orden.OrdenProcesos.Any() ? orden.OrdenProcesos.Max(op => op.Secuencia) + 1 : 1;

            foreach (var procesoId in seleccionadosNuevos.Where(p => !existentes.Contains(p)))
            {
                orden.OrdenProcesos.Add(new OrdenProceso
                {
                    ProcesoFabricacionId = procesoId,
                    Secuencia = secuencia++
                });
            }

            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Orden de producción actualizada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .ThenInclude(op => op.ProcesoFabricacion)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (orden == null)
            {
                return NotFound();
            }

            return View(orden);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orden = await _context.OrdenesProduccion.FindAsync(id);
            if (orden == null)
            {
                return NotFound();
            }

            _context.OrdenesProduccion.Remove(orden);
            await _context.SaveChangesAsync();

            TempData["Mensaje"] = "Orden de producción eliminada correctamente.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AlternarProceso(int ordenId, int procesoId)
        {
            var ordenProceso = await _context.OrdenProcesos
                .FirstOrDefaultAsync(op => op.OrdenProduccionId == ordenId && op.ProcesoFabricacionId == procesoId);

            if (ordenProceso == null)
            {
                return NotFound();
            }

            ordenProceso.Completado = !ordenProceso.Completado;
            ordenProceso.FechaCompletado = ordenProceso.Completado ? DateTime.Now : null;

            var orden = await _context.OrdenesProduccion
                .Include(o => o.OrdenProcesos)
                .FirstOrDefaultAsync(o => o.Id == ordenId);

            if (orden != null && orden.Estado != EstadoOrden.Cancelada)
            {
                var total = orden.OrdenProcesos.Count;
                var completados = orden.OrdenProcesos.Count(op =>
                    op.ProcesoFabricacionId == procesoId ? ordenProceso.Completado : op.Completado);

                orden.Estado = completados == 0
                    ? EstadoOrden.Pendiente
                    : completados == total
                        ? EstadoOrden.Completada
                        : EstadoOrden.EnProceso;
            }

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = ordenId });
        }

        private async Task<List<ProcesoOpcionViewModel>> ObtenerProcesosDisponiblesAsync(List<int> seleccionados)
        {
            return await _context.ProcesosFabricacion
                .OrderBy(p => p.Nombre)
                .Select(p => new ProcesoOpcionViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Seleccionado = seleccionados.Contains(p.Id)
                })
                .ToListAsync();
        }
    }
}
