
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
    
public partial class Servicio
{

    public Servicio()
    {

        this.OrdenesTrabajos = new HashSet<OrdenesTrabajo>();

    }


    public int Id { get; set; }

    public string Nombre { get; set; }

    public bool Activo { get; set; }

    public Nullable<int> IntervaloDias { get; set; }



    public virtual ICollection<OrdenesTrabajo> OrdenesTrabajos { get; set; }

}

}
