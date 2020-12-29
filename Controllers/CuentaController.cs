using System.Threading.Tasks;
using SwLavanderia.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SwLavanderia.Models;

namespace SwLavanderia.Controllers
{
    public class CuentaController : Controller
    {
        private UserManager<IdentityUser> _um;
        private SignInManager<IdentityUser> _sm;
        public CuentaController(UserManager<IdentityUser> um, SignInManager<IdentityUser> sm)
        {
            
            _um = um;
            _sm = sm;
        }
        public IActionResult Registro()
        {
            return View();
        }

        
        [HttpPost]
        public IActionResult Registro(Usuario usuario )
        {
            if(ModelState.IsValid){
                var username=usuario.nombre.Substring(0,3)+usuario.apellido.Substring(0,3) + usuario.nrodoc;
                var IdentityUser = new IdentityUser(username);
                
                    var result = _um.CreateAsync(IdentityUser,usuario.pwd).Result;
                
                if(result.Succeeded)
                {
                    return RedirectToAction("Acceso","cuenta");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("usuario", error.Description);
                }
            }
                
             //form formulario = new form(nombre, apellido, tipdoc, nrodoc, pwd, pwdconfirm );

             //ModelState.AddModelError("pwd","Las contraseñas no coinciden");
            return View();
        }

        public IActionResult Acceso()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Acceso(string username, string pwd)
        {
            var result = _sm.PasswordSignInAsync(username,pwd,false,false).Result;
            if(result.Succeeded)
            {
                return RedirectToAction("index","home");
            }
            ModelState.AddModelError("usuario", "Datos incorrectos");
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _sm.SignOutAsync();
            return RedirectToAction("index","home");
        }
    }
}