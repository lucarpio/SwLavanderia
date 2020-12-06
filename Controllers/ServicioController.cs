using Microsoft.AspNetCore.Mvc;
using SwLavanderia.Data;
using SwLavanderia.Models;

namespace SwLavanderia.Controllers
{
    public class ServicioController : Controller
    {
        private readonly LavanderiaContext _context;
        public ServicioController(LavanderiaContext c)
        {
            _context = c;
        }

        public IActionResult Registro()
        {
            //aca se ve el formulario de registro de almacen
            return View();
        }

        [HttpPost]
        public IActionResult Registro(Servicio serv)
        {
            ViewBag.axcel = "axcel";
            //aca se registra el nuevo servicio(ticket) a guardar en almacen
            return View("Detalle",serv);
        }

        public IActionResult Detalle(Servicio detServ)
        {
            //aca se añaden los servicios al ticket
            //se debe poder reutilizar varias veces
            return View();
        }

        [HttpPost]
        public IActionResult Detallep(Servicio detServ)
        {
            return View();
        }

        public IActionResult Entrega(Servicio detServ)
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Entregap(Servicio detServ)
        {
            return View();
        }

        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}