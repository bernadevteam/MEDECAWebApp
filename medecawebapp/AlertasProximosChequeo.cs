//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MEDECAWebApp
{
    using System;
    using System.Collections.Generic;
    
    public partial class AlertasProximosChequeo
    {
        public int IdAlerta { get; set; }
        public int IdOrden { get; set; }
        public int IdEstado { get; set; }
        public System.DateTime FechaActualizacion { get; set; }
    
        public virtual OrdenesTrabajo OrdenesTrabajo { get; set; }
    }
}
