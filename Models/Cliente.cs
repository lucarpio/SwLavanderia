using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SwLavanderia.Models
{

    public class Cliente
    {
       //Se cambió el string Id por int id
        //[Column("id")]
        public int Id { get; set; }

        [Required(ErrorMessage="Debe colocar un nombre")]
        //[Column("nombre")]
        public string Nombre { get; set; }

        [Required(ErrorMessage="Debe colocar un apellido")]
        //[Column("apellido")]
        public string Apellido { get; set; }       
        
        [Required(ErrorMessage="Debe seleccionar uno")]
        //[Column("tipodoc")]
        public string TipoDoc { get; set; }

        [Required(ErrorMessage="Debe ingresar un número de documento")]
        [RegularExpression("(^[0-9]+$)", ErrorMessage = "Solo se permiten números")]
        //[Column("nrodoc")]
        public string NroDoc { get; set; }

        [Required(ErrorMessage="Debe ingresar una dirección")]
        //[Column("direccion")]
        public string Direccion { get; set; }
        
        [Required(ErrorMessage="Debe colocar un numero de telefono")]
        //[Column("tel")]
        public int Tel { get; set; }
        

        [EmailAddress]    
        //[Column("email")]
        public string Email { get; set; }

        public Distrito Distrito { get; set; }
        [Required(ErrorMessage="Debe seleccionar un distrito")]

        //[Column("distritoid")]
        public int DistritoId { get; set; }
        
        
        /*
        public class NroDocExistAttribute : ValidationAttribute{
            
            var listClientes=_context.Clientes.OrderBy(s=>s.Id) .ToList();
            
        }
        */


    }
}