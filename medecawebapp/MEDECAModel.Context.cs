﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MEDECAEntities : DbContext
    {
        public MEDECAEntities()
            : base("name=MEDECAEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Cliente> Clientes { get; set; }
        public virtual DbSet<Combustible> Combustibles { get; set; }
        public virtual DbSet<Insumo> Insumos { get; set; }
        public virtual DbSet<Modelo> Modelos { get; set; }
        public virtual DbSet<sysdiagram> sysdiagrams { get; set; }
        public virtual DbSet<VehiculoMarca> VehiculoMarcas { get; set; }
        public virtual DbSet<Vehiculo> Vehiculos { get; set; }
        public virtual DbSet<Servicio> Servicios { get; set; }
        public virtual DbSet<OrdenesTrabajo> OrdenesTrabajos { get; set; }
        public virtual DbSet<InsumosProveedore> InsumosProveedores { get; set; }
        public virtual DbSet<Proveedore> Proveedores { get; set; }
        public virtual DbSet<DiagnosticoEstado> DiagnosticoEstados { get; set; }
        public virtual DbSet<Cita> Citas { get; set; }
        public virtual DbSet<Actividades> Actividades { get; set; }
        public virtual DbSet<MarcasInsumos> MarcasInsumos { get; set; }
        public virtual DbSet<InsumosCotizados> InsumosCotizados { get; set; }
        public virtual DbSet<Autorizados> Autorizados { get; set; }
        public virtual DbSet<Diagnostico> Diagnosticos { get; set; }
        public virtual DbSet<AlertasProximosChequeo> AlertasProximosChequeos { get; set; }
        public virtual DbSet<View_ClientesProximasRevisiones> View_ClientesProximasRevisiones { get; set; }
        public virtual DbSet<View_OrdenesActivas> View_OrdenesActivas { get; set; }
        public virtual DbSet<View_OrdenesInfo> View_OrdenesInfo { get; set; }
    
        public virtual ObjectResult<Dasboard> prc_Dashboard()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<Dasboard>("prc_Dashboard");
        }
    
        public virtual ObjectResult<ObtenerVehiculosCambioAceite_Result> ObtenerVehiculosCambioAceite()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerVehiculosCambioAceite_Result>("ObtenerVehiculosCambioAceite");
        }
    
        public virtual ObjectResult<HistoricoServicios> ObtenerHistoricoServicios()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<HistoricoServicios>("ObtenerHistoricoServicios");
        }
    
        public virtual ObjectResult<ObtenerHistoricoServiciosConteo_Result> ObtenerHistoricoServiciosConteo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerHistoricoServiciosConteo_Result>("ObtenerHistoricoServiciosConteo");
        }
    
        public virtual ObjectResult<ValidarCedORNC_Result> ValidarCedORNC(string identificacion, Nullable<int> id)
        {
            var identificacionParameter = identificacion != null ?
                new ObjectParameter("Identificacion", identificacion) :
                new ObjectParameter("Identificacion", typeof(string));
    
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidarCedORNC_Result>("ValidarCedORNC", identificacionParameter, idParameter);
        }
    
        public virtual ObjectResult<ValidarPlaca_Result> ValidarPlaca(string placa, Nullable<int> id)
        {
            var placaParameter = placa != null ?
                new ObjectParameter("Placa", placa) :
                new ObjectParameter("Placa", typeof(string));
    
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidarPlaca_Result>("ValidarPlaca", placaParameter, idParameter);
        }
    
        public virtual ObjectResult<ObtenerCitasdeHoy_Result> ObtenerCitasdeHoy()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerCitasdeHoy_Result>("ObtenerCitasdeHoy");
        }
    
        public virtual ObjectResult<ValidarPlaca_Result> ValidarNoOrden(Nullable<int> noOrden, Nullable<int> id)
        {
            var noOrdenParameter = noOrden.HasValue ?
                new ObjectParameter("NoOrden", noOrden) :
                new ObjectParameter("NoOrden", typeof(int));
    
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidarPlaca_Result>("ValidarNoOrden", noOrdenParameter, idParameter);
        }
    
        public virtual ObjectResult<ValidarProveedorRNC_Result> ValidarProveedorRNC(string rNC, Nullable<int> id)
        {
            var rNCParameter = rNC != null ?
                new ObjectParameter("RNC", rNC) :
                new ObjectParameter("RNC", typeof(string));
    
            var idParameter = id.HasValue ?
                new ObjectParameter("Id", id) :
                new ObjectParameter("Id", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ValidarProveedorRNC_Result>("ValidarProveedorRNC", rNCParameter, idParameter);
        }
    
        public virtual ObjectResult<ObtenerHistoricoServiciosMesesConteo_Result> ObtenerHistoricoServiciosMesesConteo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerHistoricoServiciosMesesConteo_Result>("ObtenerHistoricoServiciosMesesConteo");
        }
    
        public virtual ObjectResult<ObtenerVehiculosCambioAceiteProximo_Result> ObtenerVehiculosCambioAceiteProximo()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<ObtenerVehiculosCambioAceiteProximo_Result>("ObtenerVehiculosCambioAceiteProximo");
        }
    
        public virtual int ActualizarEstadoAlerta(Nullable<int> idAlerta, Nullable<int> idEstado)
        {
            var idAlertaParameter = idAlerta.HasValue ?
                new ObjectParameter("IdAlerta", idAlerta) :
                new ObjectParameter("IdAlerta", typeof(int));
    
            var idEstadoParameter = idEstado.HasValue ?
                new ObjectParameter("IdEstado", idEstado) :
                new ObjectParameter("IdEstado", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ActualizarEstadoAlerta", idAlertaParameter, idEstadoParameter);
        }
    }
}
