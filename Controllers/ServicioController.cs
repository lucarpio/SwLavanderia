using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public IActionResult servicios()
        {
            var listServicios = _context.servicios.ToList();
            return View(listServicios);
        }
        public IActionResult DetalleServicio(int nroservicio)
        {
            var detalledeservicio = _context.detalleServicios.Include(q => q.Servicio).Include(q => q.Cliente).Include(q => q.Estado).Where(x => x.IdServicio.Equals(nroservicio)).ToList();
            var datosservicio = _context.servicios.Find(nroservicio);
            ViewBag.numero = datosservicio.ID;
            return View(detalledeservicio);
        }

        [HttpPost]
        public IActionResult servicios(string filtro,string data)
        {
            var servicios = _context.servicios.Include(x => x.Almacen).Include(y => y.Cliente).Include(z => z.Boleta).ToList();

                 if (filtro.Equals("Dni"))
                {
                    servicios = servicios.Where(x => x.Cliente.TipoDoc.Equals("Dni") && x.Cliente.NroDoc.Equals(data)).ToList();
                }
                else if(filtro.Equals("Almacen"))
                {
                    servicios = servicios.Where(x => x.Almacen.nombre.Equals(data)).ToList();
                }
                else if(filtro.Equals("Cliente"))
                {
                    servicios = servicios.Where(x => x.Cliente.Apellido.Equals(data)).ToList();
                }

            return View(servicios);
        }

        public IActionResult serviciosPendientes()
        {
            var servicios = _context.servicios.Include(x => x.Estado).ToList();
            servicios = servicios.Where(x => x.Estado.nombre.Equals("Pendiente")).ToList();
            return View(servicios);
        }

    }    
}