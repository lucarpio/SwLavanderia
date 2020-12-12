using System;
using System.ComponentModel.DataAnnotations;

namespace SwLavanderia.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public ServiciosDisponibles serviciosDisponibles { get; set; }
        public int serviciosDisponiblesId { get; set; }
        public string DescServicio { get; set; }
        [RegularExpression(@"^\$?\d+(\.(\d{1,2}))?$", ErrorMessage ="El precio debe tener hasta 2 decimales")]
        public double PrecServicio { get; set; }
        public Ticket ticket { get; set; }
        public int TicketId { get; set; }
    }
}