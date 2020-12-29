using System.ComponentModel.DataAnnotations;

namespace SwLavanderia.Models
{
    public class Usuario
    {
        //[RegularExpression("^[0-9]*$", ErrorMessage = "* Solo se permiten números.")]
        public string nombre{get; set;}
        public string apellido{get; set;}
        public string tipdoc{get; set;}
        public string nrodoc{get; set;}


        [Required(ErrorMessage="Debe colocar una contraseña")]
        public string pwd{get; set;}

        [Required(ErrorMessage="Debe colocar una contraseña")]
        [System.ComponentModel.DataAnnotations.Compare("pwd", ErrorMessage="Las contraseñas no coinciden")]
        public string pwdconfirm{get; set;}
    }
}