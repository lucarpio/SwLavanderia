using System;
using System.ComponentModel.DataAnnotations;

namespace SwLavanderia.Models
{
    public class Servicio
    {
        public int Id { get; set; }
        public string NomServ { get; set; }
        public string DescServicio { get; set; }
        public double PrecServicio { get; set; }
    }
}