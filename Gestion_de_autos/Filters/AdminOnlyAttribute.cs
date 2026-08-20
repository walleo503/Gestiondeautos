using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gestion_de_autos.Filters
{
    // Como SessionAuthorize, pero ademas exige que el rol guardado en la
    // sesion sea "administrador". Se usa en las secciones que un empleado
    // (rol "vendedor") NO debe poder ver: Empleados y Estadisticas.
    public class AdminOnlyAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");
            var rol = context.HttpContext.Session.GetString("UsuarioRol");

            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            else if (rol != "administrador")
            {
                // Esta logueado pero no es administrador: no tiene permiso, lo mandamos al catalogo
                context.Result = new RedirectToActionResult("Index", "Home", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
