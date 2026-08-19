using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Gestion_de_autos.Filters
{
    // Filtro casero: si no hay un vendedor logueado (Session["UsuarioId"]),
    // lo manda a la pantalla de Login en vez de dejarlo continuar.
    // Se usa poniendo [SessionAuthorize] arriba de un controlador o una accion.
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var usuarioId = context.HttpContext.Session.GetInt32("UsuarioId");
            if (usuarioId == null)
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
            base.OnActionExecuting(context);
        }
    }
}
