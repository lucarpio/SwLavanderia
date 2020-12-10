using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SwLavanderia.Data;
using SwLavanderia.Models;

namespace SwLavanderia.Controllers
{
    public class ServicioController : Controller
    {
        private readonly LavanderiaContext _context;
        private readonly UtilsController _utilitario;
        public ServicioController(LavanderiaContext c, UtilsController u)
        {
            _context = c;
            _utilitario = u;
        }



        public IActionResult Registro()
        {
            //aca se ve el formulario de registro de almacen
            return View();
        }
        [HttpPost]
        public IActionResult Registro(Ticket objTk)
        {
            ViewBag.axcel = "axcel";
            //aca se registra el/ nuevo servicio(ticket) a guardar en almacen
            ViewBag.clientes = _utilitario.listarClientes();
            objTk.TkFechaIngreso = System.DateTime.Now;
            objTk.EstadoId = 1;
            _context.Add(objTk);
            _context.SaveChanges();
            return View("Detalle",objTk);
        }



        public IActionResult Detalle(Ticket objTk)
        {
            //recibe los datos para mostrar info del ticket al que se le añaden los servicios
            return View();
        }
        [HttpPost]
        public IActionResult Detalle(Servicio detServ)
        {
            _context.Add(detServ);
            _context.SaveChanges();
            return View();
        }



        public IActionResult Entrega(Ticket objTk)
        {
            //se añade la fecha de entrega al ticket
            _context.Update(objTk);
            _context.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult Entrega()
        {
            return View();
        }



        public IActionResult Confirmacion()
        {
            return View();
        }
    }
}