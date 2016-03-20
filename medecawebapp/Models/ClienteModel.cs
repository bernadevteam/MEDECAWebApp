using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MEDECAWebApp.Models
{
    public class ClienteModel:MEDECAWebApp.Cliente
    {
        public virtual ICollection<VehiculoModel> InfoVehiculos { get; set; }

    }
}