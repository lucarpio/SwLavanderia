using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SwLavanderia.Helpers;
using SwLavanderia.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Security;
using SwLavanderia.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SwLavanderia.Controllers
{
    
    public class ContactController : Controller
    {
        private readonly LavanderiaContext _context;
        private IConfiguration configuration;
        private IWebHostEnvironment webHostEnviroment;

        public ContactController(IConfiguration _configuration, 
        IWebHostEnvironment _webHostEnviroment, LavanderiaContext context)
        {
            _context = context;
            configuration = _configuration;
            webHostEnviroment = _webHostEnviroment;
        }
        public IActionResult Index()
        {
            Contact objC = new Contact();
            var boletas = _context.Tickets.ToList();
            ViewBag.boletas = boletas.Select(d => new SelectListItem(d.TkNroBoleta.ToString(), d.Id.ToString()));
            return View(objC);
            // return View("Index", new Contact());
        }
        [HttpPost]
        public IActionResult Send(Contact contact, IFormFile[] attachments )
        {
            var boletas = _context.Tickets.ToList();
            ViewBag.boletas = boletas.Select(d => new SelectListItem(d.TkNroBoleta.ToString(), d.Id.ToString()));
    
            var body = "Lavanderia Burbujas V & J: <br>Nro de Boleta:" + contact.Phone + "<br>Estado: " + contact.Address +
            "<br>Estimado cliente, sobrepaso fecha de entrega";
            var mailHelper = new MailHelper (configuration);
            List<string> fileNames = null;
            if(attachments != null && attachments.Length > 0)
            {
                fileNames = new List<string>();
                foreach(IFormFile attachment in attachments)
                {
                    var path = Path.Combine(webHostEnviroment.WebRootPath, "uploads", attachment.FileName);
                    using (var stream = new FileStream (path, FileMode.Create))
                    {
                        attachment.CopyToAsync(stream);
                    } 
                    fileNames.Add(path);
                }
            }
            if(mailHelper.Send(configuration["Gmail:Username"], contact.Email, 
            contact. Subject,  body,  fileNames))
            {   
                ViewBag.msg = "Envío de notificación exitoso!";
            }
            else
            { 
                ViewBag.msg = "Envío de notificación exitoso!.";
            }
            return View("Index");
        }
        
    }
}