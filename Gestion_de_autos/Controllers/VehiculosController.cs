using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;
using Gestion_de_autos.Models;
using Gestion_de_autos.Filters;

namespace Gestion_de_autos.Controllers
{
    // Este controlador administra la tabla datos_auto (los vehiculos en venta)
    public class VehiculosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;
        public VehiculosController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        // GET /Vehiculos -> incluye el vendedor (Include = JOIN). ?estado=reservado filtra por estado
        public async Task<IActionResult> Index(string? estado)
        {
            var query = _context.DatosAuto.Include(d => d.Vendedor).Include(d => d.Fotos).AsQueryable();
            if (!string.IsNullOrEmpty(estado))
            {
                query = query.Where(d => d.Estado == estado);
            }
            ViewBag.EstadoActual = estado;
            return View(await query.OrderByDescending(d => d.Id).ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.Include(d => d.Vendedor).Include(d => d.Fotos).FirstOrDefaultAsync(d => d.Id == id);
            if (auto == null) return NotFound();
            return View(auto);
        }

        [SessionAuthorize]
        public IActionResult Create()
        {
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public async Task<IActionResult> Create(
            [Bind("Usuario,Marca,Modelo,CostoCompra,PrecioVenta,Descripcion,Danos,PiezasFaltantes,Estado")] DatosAuto auto,
            List<IFormFile> fotos)
        {
            if (ModelState.IsValid)
            {
                _context.Add(auto);
                await _context.SaveChangesAsync(); // primero se guarda el auto para tener su Id

                await GuardarFotosAsync(auto.Id, fotos);

                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        // Guarda cada foto en wwwroot/uploads/vehiculos/{id} y crea su registro en fotos_auto
        private async Task GuardarFotosAsync(int datosAutoId, List<IFormFile>? fotos)
        {
            if (fotos == null || fotos.Count == 0) return;

            var carpeta = Path.Combine(_env.WebRootPath, "uploads", "vehiculos", datosAutoId.ToString());
            Directory.CreateDirectory(carpeta);

            foreach (var foto in fotos)
            {
                if (foto.Length == 0) continue;

                var nombreArchivo = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
                var rutaFisica = Path.Combine(carpeta, nombreArchivo);

                using (var stream = new FileStream(rutaFisica, FileMode.Create))
                {
                    await foto.CopyToAsync(stream);
                }

                _context.FotosAuto.Add(new FotoAuto
                {
                    DatosAutoId = datosAutoId,
                    Ruta = $"/uploads/vehiculos/{datosAutoId}/{nombreArchivo}"
                });
            }
            await _context.SaveChangesAsync();
        }

        [SessionAuthorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.Include(d => d.Fotos).FirstOrDefaultAsync(d => d.Id == id);
            if (auto == null) return NotFound();
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Usuario,Marca,Modelo,CostoCompra,PrecioVenta,Descripcion,Danos,PiezasFaltantes,Estado")] DatosAuto auto, List<IFormFile> fotos)
        {
            if (id != auto.Id) return NotFound();
            if (ModelState.IsValid)
            {
                _context.Update(auto);
                await _context.SaveChangesAsync();
                await GuardarFotosAsync(auto.Id, fotos); // agrega las fotos nuevas, sin borrar las anteriores
                return RedirectToAction(nameof(Index));
            }
            ViewData["Usuario"] = new SelectList(_context.Usuarios, "Id", "Nombre", auto.Usuario);
            return View(auto);
        }

        // Elimina una foto individual (botoncito "x" en la vista de edicion)
        [SessionAuthorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EliminarFoto(int fotoId, int datosAutoId)
        {
            var foto = await _context.FotosAuto.FindAsync(fotoId);
            if (foto != null)
            {
                var rutaFisica = Path.Combine(_env.WebRootPath, foto.Ruta.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                if (System.IO.File.Exists(rutaFisica)) System.IO.File.Delete(rutaFisica);

                _context.FotosAuto.Remove(foto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Edit), new { id = datosAutoId });
        }

        [SessionAuthorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var auto = await _context.DatosAuto.Include(d => d.Vendedor).FirstOrDefaultAsync(d => d.Id == id);
            if (auto == null) return NotFound();
            return View(auto);
        }

        [SessionAuthorize]
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
