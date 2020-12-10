using System;
using System.Collections.Generic;

namespace SwLavanderia.Models
{
    public class Servicio
    {
        public int ID { get; set; }

        public Almacen Almacen { get; set; }
        public int IdAlmacen { get; set; }
        public Estado Estado { get; set; }
        public int IdEstado { get; set; }
        public Cliente Cliente { get; set; }
        public int IdCliente { get; set; }
        public Boleta Boleta { get; set; }
        public int IdBoleta { get; set; }
        public string TipoServicio { get; set; }
        public string Descripcion { get; set; }

        public double? TarifaxKilo { get; set; }
        public double? TarifaxUnidad { get; set; }

        public DateTime FEntrega {get; set; }
        public List<DetalleServicio> DetalleServicio { get; set; }
    }

}