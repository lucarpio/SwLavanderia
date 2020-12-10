using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.fecha = System.DateTime.Now;
            var listaClientes = _context.Clientes.ToList().Select(client => new SelectListItem(client.NroDoc+" "+client.Nombre+" "+client.Apellido, client.Id.ToString()));

            // ViewBag.clientes = listaClientes;
            ViewBag.clientes = listarClientes().Select(client => new SelectListItem(client.NroDoc+" "+client.Nombre+" "+client.Apellido, client.Id.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Registro(Ticket objTk)
        {
            ViewBag.axcel = "axcel";
            //aca se registra el/ nuevo servicio(ticket) a guardar en almacen
            
            var nroBoleta = objTk.TkNroBoleta;
            //revisar si la boleta ya esta registrada
            if(checkTicket(nroBoleta))
            {
                // si se ingresa una boleta duplicada se activa este metodo
                ModelState.AddModelError("NroDoc","Ya existe un Documento con ese número");
                return View(objTk);
            }else
            {
                ViewBag.clientes = listarClientes();
                //se guarda la fecha de 
                objTk.TkFechaIngreso = System.DateTime.Now;
                //asigna por defecto estado "En espera"
                objTk.EstadoId = 1;
                _context.Add(objTk);
                _context.SaveChanges();
                return RedirectToAction("Detalle",objTk);
            }
            
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

        // -------------------------------------------------------METODOS------------------------------------------------------------------
        public bool checkTicket(int n)
        {
            var check = false;
            var listaTickets = _context.Tickets.OrderBy(x => x.Id).ToList();
            for (var i = 0; i < listaTickets.Count; i++)
            {
                if(n == listaTickets.ElementAt(i).TkNroBoleta)
                {
                    check = true;
                }
                
            }
            return check;
        }
        public List<Cliente> listarClientes()
        {
            var listaClientes = _context.Clientes.OrderBy(x => x.Id)
                                                .ToList();
            return listaClientes;
        }
    }
}