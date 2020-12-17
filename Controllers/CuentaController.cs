using System.Threading.Tasks;
using SwLavanderia.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


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
        public IActionResult Registro(string nombre, string apellido, string tipdoc, string nrodoc, string pwd, string pwdconfirm )
        {
            if(pwd==pwdconfirm){
                var username=nombre.Substring(0,3)+apellido.Substring(0,3) + nrodoc;
                var IdentityUser = new IdentityUser(username);
                
                    var result = _um.CreateAsync(IdentityUser,pwd).Result;
                
                if(result.Succeeded)
                {
                    return RedirectToAction("index","home");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("usuario", error.Description);
                }
            }else{
                ModelState.AddModelError("pwd","Las contraseñas no coinciden");
            }

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