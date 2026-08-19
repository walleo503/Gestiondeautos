using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;
using Gestion_de_autos.Models;

namespace Gestion_de_autos.Controllers
{
    // Este controlador administra la tabla datos_auto (los vehiculos en venta)
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _context;
        public VehiculosController(AppDbContext context) => _context = context;

        // GET /Vehiculos -> incluye el vendedor (Include = JOIN)
        public async Task<IActionResult> Index()
        {
            var lista = _context.DatosAuto.Include(d => d.Vendedor);
            return View(await lista.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.Include(d => d.Vendedor).FirstOrDefaultAsync(d => d.Id == id);
            if (auto == null) return NotFound();
            return View(auto);
        }

        public IActionResult Create()
        {
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Usuario,Marca,Modelo,CostoCompra,PrecioVenta,Descripcion,Danos,PiezasFaltantes,Estado")] DatosAuto auto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.FindAsync(id);
            if (auto == null) return NotFound();
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Marca,Modelo,CostoCompra,PrecioVenta,Descripcion,Danos,PiezasFaltantes,Estado")] DatosAuto auto)
        {
            if (id != auto.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(auto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.Include(d => d.Vendedor).FirstOrDefaultAsync(d => d.Id == id);
            if (auto == null) return NotFound();
            return View(auto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auto = await _context.DatosAuto.FindAsync(id);
            if (auto != null)
            {
                _context.DatosAuto.Remove(auto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
