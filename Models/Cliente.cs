using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SwLavanderia.Models
{
    public class Cliente
    {
       //Se cambió el string Id por int id
        public int ID { get; set; }

        [Required(ErrorMessage="Debe colocar un nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage="Debe colocar un apellido")]
        public string Apellido { get; set; }       
        
        [Required(ErrorMessage="Debe seleccionar uno")]
        public string TipoDoc { get; set; }

        [Required(ErrorMessage="Debe ingresar un numero de documento")]
        public string NroDoc { get; set; }

        [Required(ErrorMessage="Debe ingresar una direccion")]
        public string Direccion { get; set; }
        
        [Required(ErrorMessage="Debe colocar un numero de telefono")]
        public int Tel { get; set; }
        [EmailAddress]    
        public string Email { get; set; }        
    }
}