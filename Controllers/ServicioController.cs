using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            ViewBag.fecha = System.DateTime.Now;
            ViewBag.clientes = listarClientes().Select(client => new SelectListItem(client.NroDoc+" "+client.Nombre+" "+client.Apellido, client.Id.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Registro(Ticket objTk)
        {
            ViewBag.fecha = System.DateTime.Now;
            ViewBag.clientes = listarClientes().Select(client => new SelectListItem(client.NroDoc+" "+client.Nombre+" "+client.Apellido, client.Id.ToString()));
            //aca se registra el nuevo servicio(ticket) a guardar en almacen
            //revisar si la boleta ya esta registrada
            var nroBoleta = objTk.TkNroBoleta;
            if(checkTicket(nroBoleta) || !ModelState.IsValid || objTk.AlmacenId==0)
            {
                // si se ingresa una boleta duplicada se activa este metodo y se da el mensaje de error
                if(checkTicket(nroBoleta))
                {
                    ModelState.AddModelError("TkNroBoleta","Esta boleta ya esta registrada");
                }
                if (objTk.AlmacenId==0)
                {
                    ModelState.AddModelError("AlmacenId","Escoja un almacen");
                }
                return View(objTk);
            }else
            {
                ViewBag.clientes = listarClientes();
                //se guarda la fecha de 
                objTk.TkFechaIngreso = System.DateTime.Now;
                //asigna por defecto estado "En espera"
                objTk.EstadoId = 1;
                _context.Add(objTk);
                //USAR PARA VER INFO DEL OBJ SIN GUARDAR A LA DB
                // return Json(objTk);
                _context.SaveChanges();
                return RedirectToAction("Listado",objTk);
            }
            
        }



        public IActionResult Detalle(int boleta)
        {
            //recibe los datos para mostrar info del ticket al que se le añaden los servicios
            var objTicket = _context.Tickets.Find(boleta);
            // var objServ = ;
            // return Json(objTicket);
            ViewBag.idboleta = objTicket.Id;
            ViewBag.ServiciosDisponibles = listarServDispo().Select(serv => new SelectListItem(serv.NomServ, serv.Id.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Detalle(Servicio detServ, int idboleta)
        {
            var objTicket = _context.Tickets.Find(idboleta);
            // ViewBag.idboleta = objTicket.Id;
            detServ.TicketId = idboleta;
            if(ModelState.IsValid) 
            {
                // return Json(detServ);
                _context.Add(detServ);
                _context.SaveChanges();
                return RedirectToAction("Listado",objTicket);
            }else{
                if (detServ.PrecServicio<=0)
                {
                    ModelState.AddModelError("PrecServicio","El precio debe ser mayor que 0");
                }
                return View(detServ);
            }
        }



        public IActionResult Listado(Ticket objTk)
        {
            //lista los servicios agregados al ticket
            //permite revisar agregar nuevos servicios o quitarlos del ticket a guardar
            ViewBag.NroBoleta = objTk.TkNroBoleta;
            ViewBag.idBol = objTk.Id;
            var ListaServicios = listarServicios().Where(x=>x.TicketId == objTk.Id).ToList();
            // return Json(ListaServicios);
            return View(ListaServicios);
        }
        public IActionResult Confirmacion(int boleta)
        {
            var objBoleta = _context.Tickets.Find(boleta);
            var suma = _context.Servicios.Include(x=>x.ticket).Where(y=>y.TicketId == boleta).Sum(z=>z.PrecServicio);
            objBoleta.TkPagoTotal = suma;
            // return Json(objBoleta);
            _context.Update(objBoleta);
            _context.SaveChanges();
            return View();
        }
        [HttpPost]
        public IActionResult Confirmacion(int boleta, Ticket objTicket)
        {
            var objBoleta = _context.Tickets.Find(boleta);
            objBoleta.TkFechaEntrega = objTicket.TkFechaEntrega;
            return Json(objBoleta);
            _context.Update(objBoleta);
            _context.SaveChanges();
            // return RedirectToAction("Index");
        }

// ----------------------------------------------------------METODOS------------------------------------------------------------------
        bool checkTicket(int n)
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
        List<Cliente> listarClientes()
        {
            var listaClientes = _context.Clientes.Include(c => c.Distrito)
                                                 .OrderBy(x => x.Id)
                                                 .ToList();
            return listaClientes;
        }
        List<Servicio> listarServicios()
        {
            var listaServicios = _context.Servicios.Include(x => x.ticket).Include(x => x.serviciosDisponibles)
                                                   .OrderBy(x => x.Id)
                                                   .ToList();
            return listaServicios;
        }
        List<ServiciosDisponibles> listarServDispo()
        {
            var listaServDispo = _context.ServiciosDisponibles.OrderBy(x => x.Id)
                                                              .ToList();
            return listaServDispo;
        }
    }
}