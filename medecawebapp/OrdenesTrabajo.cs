//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MEDECAWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class OrdenesTrabajo
    {
        public OrdenesTrabajo()
        {
            this.Servicios = new HashSet<Servicio>();
            this.InsumosProveedores = new HashSet<InsumosProveedore>();
        }
    
        public int Id { get; set; }
        public Nullable<int> NoOrden { get; set; }
        public string Fallas { get; set; }
        public string Reparaciones { get; set; }
        public System.DateTime Fecha { get; set; }
        public int IdVehiculo { get; set; }
        public bool Entregado { get; set; }
        public string Diagnostico { get; set; }
        public Nullable<System.DateTime> FechaEntrega { get; set; }
        public Nullable<System.DateTime> FechaPrometida { get; set; }
        public Nullable<double> DistanciaRecorrida { get; set; }
    
        public virtual Vehiculo Vehiculo { get; set; }
        public virtual ICollection<Servicio> Servicios { get; set; }
        public virtual ICollection<InsumosProveedore> InsumosProveedores { get; set; }
    }
}
