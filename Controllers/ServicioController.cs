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
            ViewBag.nroboleta = boleta;
            var objBoleta = _context.Tickets.Find(boleta);
            // return Json(objBoleta);
            return View();
        }
        [HttpPost]
        public IActionResult Confirmacion(int boleta, Ticket objTicket)
        {
            var objBoleta = _context.Tickets.Find(boleta);
            var suma = _context.Servicios.Include(x=>x.ticket).Where(y=>y.TicketId == boleta).Sum(z=>z.PrecServicio);
            objBoleta.TkPagoTotal = suma;
            objBoleta.TkFechaEntrega = objTicket.TkFechaEntrega;
            // return Json(objBoleta);
            _context.Update(objBoleta);
            _context.SaveChanges();
            return RedirectToAction("Index","Home");
        }

        public IActionResult ModificarEstadoServicio(Ticket objTicket)
        {
            var Estado = true;
            ViewBag.Peticion = Estado;
            if(Estado == true){

            }

            if(_context.Tickets.Find(objTicket.Id) == null) //REVISA SI EXISTE LA BOLETA
            {
                return View();
            }else{
                ViewBag.Peticion = false;
                ViewBag.NUMEROBOLETA = objTicket.TkNroBoleta;
                ViewBag.CLIENTE = objTicket.Cliente.Nombre+" "+objTicket.Cliente.Apellido+" "+objTicket.Cliente.NroDoc;
                ViewBag.ESTADOS = _context.ServiciosDisponibles.ToList().Select(x=> new SelectListItem(x.NomServ,x.Id.ToString()));
                return View(objTicket);
            }
        }

        [HttpPost]
        public IActionResult ModificarEstadoServicio(int nroboleta, Ticket objBoleta)
        {
            ViewBag.NUMEROBOLETA = objBoleta.TkNroBoleta;
                ViewBag.CLIENTE = objBoleta.Cliente.Nombre+" "+objBoleta.Cliente.Apellido+" "+objBoleta.Cliente.NroDoc;
                ViewBag.ESTADOS = _context.ServiciosDisponibles.ToList().Select(x=> new SelectListItem(x.NomServ,x.Id.ToString()));
            return Json(objBoleta);
            // return RedirectToAction("ModificarEstadoServicio");
            // ViewBag.ServiciosDisponibles = listarServDispo().Select(serv => new SelectListItem(serv.NomServ, serv.Id.ToString()));
            // var check = _context.Tickets.Find(nroboleta);
            // if(check != null)
            // {
                
            //     return RedirectToAction("InfoEstadoServicio", check);
            // }else
            // {
            //     ModelState.AddModelError("TkNroBoleta","Boleta no encontrada");
            //     return View();
            // }
        }

        // public IActionResult InfoEstadoServicio(Ticket objBoleta)
        // {
        //     ViewBag.ServiciosDisponibles = listarServDispo().Select(serv => new SelectListItem(serv.NomServ, serv.Id.ToString()));
        //     return View(objBoleta);
        // }
        // [HttpPost]
        // public IActionResult InfoEstadoServicio(Ticket objBoleta, int nro)
        // {
        //     ViewBag.ServiciosDisponibles = listarServDispo().Select(serv => new SelectListItem(serv.NomServ, serv.Id.ToString()));
        //     return View(objBoleta);
        // }
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

        public IActionResult servicios()
        {
            var listServicios = _context.Servicios.Include(q => q.ticket).ToList();
            return View(listServicios);
        }
        public IActionResult DetalleServicio(int nroservicio)
        {
            var detalledeservicio = _context.Servicios.Include(q => q.serviciosDisponibles).Include(q => q.ticket.Cliente).Include(q => q.ticket.Servicios).Include(q => q.ticket.Estado).Where(x => x.Id.Equals(nroservicio)).ToList();
            var datosservicio = _context.Servicios.Find(nroservicio);
            ViewBag.numero = datosservicio.Id;
            return View(detalledeservicio);
        }

        [HttpPost]
        public IActionResult servicios(string filtro,string data)
        {
            var servicios = _context.Servicios.Include(x => x.ticket.Cliente).Include(x => x.ticket.Almacen).ToList();

                 if (filtro.Equals("Dni"))
                {
                    servicios = servicios.Where(x => x.ticket.Cliente.TipoDoc.Equals("Dni") && x.ticket.Cliente.NroDoc.Equals(data)).ToList();
                }
                else if(filtro.Equals("Almacen"))
                {
                    servicios = servicios.Where(x => x.ticket.Almacen.NomAlm.Equals(data)).ToList();
                }
                else if(filtro.Equals("Cliente"))
                {
                    servicios = servicios.Where(x => x.ticket.Cliente.Apellido.Equals(data)).ToList();
                }
                

            return View(servicios);
        }

        public IActionResult serviciosPendientes()
        {
            var servicios = _context.Servicios.Include(x => x.ticket.Estado).ToList();
            servicios = servicios.Where(x => x.ticket.Estado.NombreEstado.Equals("En espera")).ToList();
            return View(servicios);
        }

        public IActionResult serviciosRealizados()
        {
            var listServicios = _context.Servicios.Include(q => q.ticket).Include(q => q.serviciosDisponibles).ToList();
            return View(listServicios);
        }

        [HttpPost]
        public IActionResult serviciosRealizados(DateTime fechainicial, DateTime fechafinal)
        {
            var servicios = _context.Servicios.Include(q => q.ticket).Include(q => q.serviciosDisponibles).ToList();
            
            int result = DateTime.Compare(fechainicial, fechafinal);
            
            if(fechainicial <= fechafinal && fechafinal >= fechainicial)
            {
                servicios = _context.Servicios.Where(x => x.ticket.TkFechaIngreso.Date.Equals(fechainicial.Date) && x.ticket.TkFechaEntrega.Equals(fechafinal)).ToList();        
            }
            else if (fechainicial >= fechafinal && fechafinal <= fechainicial){
                return View("Ingresar lapso de tiempo válido");
            }

            return View(servicios);
            
        }

        public IActionResult serviciosFiltrados()
        {
             var detalledeservicio = _context.Servicios.Include(q => q.serviciosDisponibles).Include(q => q.ticket.Servicios).ToList();

            

           return View();
           
        }


    }
}