using System;
using System.ComponentModel.DataAnnotations;

namespace SwLavanderia.Models
{
    public class DetalleServicio
    {
        public int ID { get; set; }
        public Cliente Cliente {get; set; }
        public int IdCliente {get; set; }
        public Estado Estado {get; set; }
        public int IdEstado {get; set; }
        public Servicio Servicio {get; set; }
        public int IdServicio {get; set; }
        public string TipoServicio { get; set; }
        public string Descripcion { get; set; }
        public double? PrecioUnitario { get; set; }
        public double? PrecioTotal { get; set; }
        
       
        
    }
}