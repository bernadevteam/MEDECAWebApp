using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDECAWebApp.Models
{
    public class CitaModel : Cita
    {
        public Cliente Cliente { get; set; }

        public int IdCliente { get; set; }
    }
}