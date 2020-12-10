using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using SwLavanderia.Data;
using SwLavanderia.Models;

namespace SwLavanderia.Controllers
{
    public class UtilsController : Controller
    {
        private readonly LavanderiaContext _context;
        public UtilsController(LavanderiaContext c)
        {
            _context = c;
        }
        public List<Cliente> listarClientes()
        {
            var listaClientes = _context.Clientes.OrderBy(x => x.Id)
                                                .ToList();
            return listaClientes;
        }
    }
}