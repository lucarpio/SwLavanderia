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
            var distritos = _context.Distritos.ToList();
            ViewBag.Distrito = distritos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegistrarCliente(Cliente objCliente)
        {
            
            if (_context.Clientes.Any(a => a.NroDoc == objCliente.NroDoc))
            {
                ModelState.AddModelError("NroDoc","Ya existe un Documento con ese número");
            }else{
                if(ModelState.IsValid)
                {
                    _context.Add(objCliente);
                    _context.SaveChanges();
                    ViewBag.Message = "Registro del cliente exitoso";
                    return View();    
                }
            }

            var distritos = _context.Distritos.ToList();
            ViewBag.Distrito = distritos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
            return View();
            
        }


        public IActionResult ListarCliente() 
        {   
            var distritos = _context.Distritos.ToList();
            ViewBag.Distrito = distritos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
            var listClientes=_context.Clientes.OrderBy(s=>s.Id) .ToList();
            return View(listClientes);
        }


        public IActionResult EditarCliente(int? id)
        {
            if(id == null){
                return NotFound();
            }
            var cliente = _context.Clientes.Find(id);
            if(cliente == null){
                return NotFound();
            }
            var distritos = _context.Distritos.ToList();
            ViewBag.Distrito = distritos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
            return View(cliente);
        }
        

        [HttpPost]
        public IActionResult EditarCliente(int id, Cliente objCliente)
        {

            if (ModelState.IsValid)
            {
                _context.Update(objCliente);
                _context.SaveChanges();
                return RedirectToAction("ListarCliente");   
            }
            var distritos = _context.Distritos.ToList();
            ViewBag.Distrito = distritos.Select(d => new SelectListItem(d.Nombre, d.Id.ToString()));
            return View(objCliente);
        }



        public IActionResult BorrarCliente(int? id)
        {
            var empleado = _context.Clientes.Find(id);
            _context.Clientes.Remove(empleado);
            _context.SaveChanges();
            return RedirectToAction(nameof(ListarCliente));
        }


        [HttpPost]
        public IActionResult Filtrar(string idfiltro, string filtro)
        {
            var listClientes = _context.Clientes.OrderBy(s => s.Id).ToList();
            if(idfiltro == "Nombre"){
                listClientes=_context.Clientes.Where(c => c.Nombre.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.Id) .ToList();
            }else{
                listClientes=_context.Clientes.Where(c => c.NroDoc.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.Id) .ToList();
            }
            // var listClientes=_context.Clientes.Where(c => c.Nombre.ToUpper().Contains(filtro.ToUpper())).OrderBy(s=>s.Id) .ToList();
            return View("ListarCliente", listClientes);
        }


    }
    
}