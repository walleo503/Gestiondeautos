using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosCortos.Controllers
{
    public class PromocionesController : Controller
    {
        private readonly IConfiguration _config;
        public PromocionesController(IConfiguration config)
        {
            _config = config;
        }

        [Route("Ofertas-Del-Dia")] //URL: /Ofertas-Del-Dia
       
        public IActionResult Ofertas() 
        {
            ViewBag.Tienda = _config["TiendaConfig:NombreTienda"];
            ViewBag.Desc = _config["TiendaConfig:DescuentoGlobal"];
            return View();
        }

        public ActionResult Index()
        {
            return View();
        }

        // GET: PromocionesController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: PromocionesController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PromocionesController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PromocionesController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: PromocionesController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: PromocionesController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: PromocionesController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
