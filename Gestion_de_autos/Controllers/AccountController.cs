using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Gestion_de_autos.Data;
using Gestion_de_autos.Models;

namespace Gestion_de_autos.Controllers
{
    // Login simple usando Session (guarda el Id del vendedor logueado).
    // Nota: aqui la contrasena se compara en texto plano solo para que
    // el proyecto funcione rapido; en un sistema real debe guardarse
    // con un hash (por ejemplo BCrypt) y nunca en texto plano.
    public class AccountController : Controller
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context) => _context = context;

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string correo, string contrasena)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == correo && u.Contrasena == contrasena);

            if (usuario == null)
            {
                ViewBag.Error = "Correo o contrasena incorrectos.";
                return View();
            }

            // Guarda quien inicio sesion para usarlo en toda la app
            HttpContext.Session.SetInt32("UsuarioId", usuario.Id);
            HttpContext.Session.SetString("UsuarioNombre", usuario.Nombre);

            // Deja constancia en la tabla "login" (historial de accesos)
            _context.LoginLogs.Add(new LoginLog
            {
                Usuario = usuario.Id,
                Nombre = usuario.Nombre,
                Contrasena = usuario.Contrasena,
                FechaLogin = DateTime.Now
            });
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
