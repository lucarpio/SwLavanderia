using SwLavanderia.Models;
using SwLavanderia.Data;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwLavanderia.Controllers
{
        public class ClienteController : Controller
    {
        private readonly LavanderiaContext _context;

        public ClienteController(LavanderiaContext context)
        {
            _context = context;
        }
        
        public IActionResult RegistrarCliente() 
        {
            return View();
        }


        [HttpPost]
        public IActionResult RegistrarCliente(Cliente objCliente)
        {
            if(ModelState.IsValid)
            {
                _context.Add(objCliente);
                _context.SaveChanges();
                ViewData["Message"] = "Success";
                RedirectToAction("ListarCliente");
                
            }
            
            return View();
            
        }


        public IActionResult ListarCliente() 
        {   
             var listClientes=_context.Clientes.OrderBy(s=>s.Id) .ToList();
            return View(listClientes);
        }

    }
    
}