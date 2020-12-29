using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SwLavanderia.Data;
using SwLavanderia.Models;
using System;
using System.Threading.Tasks;
namespace SwLavanderia.Controllers
{
    public class AlmacenController : Controller
    {
        private readonly LavanderiaContext _context;
        
        public AlmacenController(LavanderiaContext c)
        {
            _context = c;
        }
        List<Ticket> tickets()
        {
            var lista = _context.Tickets.ToList();
            return lista;
        }
        


        public IActionResult almacenes()
        {
            

            double porcentaje1;
            double porcentaje2;
            
            double restante1;
            double restante2;
            
            double cantidadalmacen1 = tickets().Where(x => x.AlmacenId == 1 &&  x.EstadoId != 4 ).ToList().Count();
            double cantidadalmacen2 = tickets().Where(x => x.AlmacenId == 2 &&  x.EstadoId != 4 ).ToList().Count();

            porcentaje1 = Math.Round((cantidadalmacen1/78.0) * 100, 2) ;
            porcentaje2 = Math.Round((cantidadalmacen2/100.0) * 100, 2) ;
            restante1 = 78 - cantidadalmacen1;
            restante2 = 100 - cantidadalmacen2;
            int resto1 =Convert.ToInt32(restante1);
            int resto2 =Convert.ToInt32(restante2);
            
            int almacen1=Convert.ToInt32(cantidadalmacen1);
            int almacen2=Convert.ToInt32(cantidadalmacen2);

            ViewBag.cantidadalmacen1= almacen1;
            ViewBag.cantidadalmacen2= almacen2;
            ViewBag.almacenes1=porcentaje1;
            ViewBag.almacenes2=porcentaje2;
            ViewBag.restante1=resto1;
            ViewBag.restante2=resto2;
            
            if(almacen1==1){
                ViewBag.mensaje1="Almacen 1 lleno";
            }
            if(almacen1==100){
                ViewBag.mensaje2="Almacen 2 lleno";
            }
            
            // return Json(ListaServicios);
            return View();
        }
    }
}