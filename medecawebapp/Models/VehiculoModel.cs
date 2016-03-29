using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDECAWebApp.Models
{
    public class VehiculoModel : MEDECAWebApp.Vehiculo
    {
        public int IdMarca { get; set; }
        public string Info { get; set; }
        public bool CanDelete { get; set; }

        public static string GetInfo(Vehiculo model) {
            return string.Format("{0} {1} Año {2}", model.Modelo.VehiculoMarca.Nombre, model.Modelo.Nombre, model.Anio);
        }

    }
}