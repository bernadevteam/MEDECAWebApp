using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Data.Entity.Migrations;

namespace MEDECAWebApp.Controllers
{
    [Authorize]
    public class OrdenesTrabajosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ValidarPlaca_Result ExisteNoOrden(int field, int id)
        {
            return db.ValidarNoOrden(field, id).First();
        }

        [HttpGet]
        public IEnumerable<object> OrdenesActivas()
        {
            return db.View_OrdenesActivas.ToList();
        }
        [HttpGet]
        public object OrdenActiva(int id)
        {
            return db.View_OrdenesActivas.FirstOrDefault(oa => oa.Id.Equals(id));
        }
        [HttpGet]
        public object ClienteOrden(int idOrden)
        {
            return db.OrdenesTrabajos
                .Include(ot => ot.Diagnosticos)
                .Include(ot => ot.InsumosCotizados)
                .Include(ot => ot.InsumosProveedores)
                .Include(ot => ot.Servicios)
                .Where(ot => ot.Id.Equals(idOrden))
                .AsEnumerable()
                .Select(ot => new
                {
                    Fallas = ot.Fallas,
                    Fecha = ot.Fecha,
                    FechaEntrega = ot.FechaEntrega,
                    FechaPrometida = ot.FechaPrometida,
                    Id = ot.Id,
                    Entregado = ot.Entregado,
                    NoOrden = ot.NoOrden,
                    DistanciaRecorrida = ot.DistanciaRecorrida,
                    Diagnosticos = ot.Diagnosticos.Select(dg => new Diagnostico
                    {
                        IdDiagnostico = dg.IdDiagnostico,
                        IdOrden = dg.IdOrden,
                        IdEstado = dg.IdEstado,
                        Descripcion = dg.Descripcion,
                        Ajustes = dg.Ajustes,
                        DiagnosticoEstado = new DiagnosticoEstado()
                        {
                            IdDiagnosticoEstado = dg.DiagnosticoEstado.IdDiagnosticoEstado,
                            Nombre = dg.DiagnosticoEstado.Nombre
                        }
                    }),
                    Reparaciones = ot.Reparaciones,
                    Diagnostico = ot.Diagnostico,
                    IdVehiculo = ot.Vehiculo.IdVehiculo,
                    InsumosCotizados = ot.InsumosCotizados.Select(ic => new
                    {
                        Aprobado = ic.Aprobado,
                        Cantidad = ic.Cantidad,
                        EsLocal = ic.EsLocal,
                        FechaAproxLlegada = ic.FechaAproxLlegada,
                        FechaCompra = ic.FechaCompra,
                        IdInsumo = ic.IdInsumo,
                        IdInsumoCotizado = ic.IdInsumoCotizado,
                        IdMarcaInsumo = ic.IdMarcaInsumo,
                        IdOrden = ic.IdOrden,
                        IdProveedor = ic.IdProveedor,
                        Precio = ic.Precio,
                        NombreInsumo = ic.Insumos.Nombre,
                        NombreProveedor = ic.Proveedores != null ? ic.Proveedores.Nombre : null,
                        NombreMarca = ic.MarcasInsumos != null ? ic.MarcasInsumos.Nombre : null
                    }),
                    InsumosProveedores = ot.InsumosProveedores.Select(ip => new InsumosProveedore
                    {
                        Precio = ip.Precio,
                        IdInsumo = ip.IdInsumo,
                        IdOrdenServicio = ip.IdOrdenServicio,
                        Proveedore = new Proveedore { Nombre = ip.Proveedore.Nombre },
                        IdProveedor = ip.IdProveedor,
                        Cantidad = ip.Cantidad,
                        Insumo = new Insumo
                        {
                            Activo = ip.Insumo.Activo,
                            Nombre = ip.Insumo.Nombre,
                            IdInsumo = ip.IdInsumo
                        }
                    }).ToList(),
                    Servicios = ot.Servicios.Select(s => new Servicio
                    {
                        Activo = s.Activo,
                        Id = s.Id,
                        Nombre = s.Nombre,
                        IntervaloDias = s.IntervaloDias
                    }).ToList(),
                    Vehiculo = new
                    {
                        Info = string.Format("{0} {1} {2}", ot.Vehiculo.Modelo.VehiculoMarca.Nombre, ot.Vehiculo.Modelo.Nombre, ot.Vehiculo.Anio),
                        Modelo = new Modelo
                        {
                            Nombre = ot.Vehiculo.Modelo.Nombre,
                            IdMarca = ot.Vehiculo.Modelo.IdMarca,
                            VehiculoMarca = new VehiculoMarca
                            {
                                Nombre = ot.Vehiculo.Modelo.VehiculoMarca.Nombre
                            }
                        },
                        Anio = ot.Vehiculo.Anio,
                        Color = ot.Vehiculo.Color,
                        Combustible = new Combustible
                        {
                            Nombre = ot.Vehiculo.Combustible.Nombre
                        },
                        Kilometraje = ot.Vehiculo.Kilometraje,
                        UnidadDistancia = ot.Vehiculo.UnidadDistancia,
                        Insumos = ot.Vehiculo.Insumos.OrderBy(i => i.Nombre).Where(i => i.Activo).Select(i => new Insumo
                        {
                            Nombre = i.Nombre,
                            IdInsumo = i.IdInsumo
                        }),
                        NoChasis = ot.Vehiculo.NoChasis,
                        Placa = ot.Vehiculo.Placa,
                        IdVehiculo = ot.Vehiculo.IdVehiculo,
                        Motor = ot.Vehiculo.Motor,
                        IdCliente = ot.Vehiculo.IdCliente,
                        IdModelo = ot.Vehiculo.IdModelo,
                        IdCombustible = ot.Vehiculo.IdCombustible,
                        Cliente = new
                        {
                            Direccion = ot.Vehiculo.Cliente.Direccion,
                            Email = ot.Vehiculo.Cliente.Email,
                            Favorito = ot.Vehiculo.Cliente.Favorito,
                            IDCliente = ot.Vehiculo.Cliente.IDCliente,
                            Identificacion = ot.Vehiculo.Cliente.Identificacion,
                            TipoIdentificacion = ot.Vehiculo.Cliente.TipoIdentificacion,
                            Nombre = ot.Vehiculo.Cliente.Nombre,
                            Telefono = ot.Vehiculo.Cliente.Telefono
                        }
                    }
                }).First();
        }

        [HttpGet]
        public IEnumerable<DiagnosticoEstado> ObtenerDiagnosticoEstados()
        {
            return db.DiagnosticoEstados.AsEnumerable().Select(dg => new DiagnosticoEstado()
            {
                IdDiagnosticoEstado = dg.IdDiagnosticoEstado,
                Nombre = dg.Nombre
            }).ToList();
        }
        // GET api/OrdenesTrabajos
        public IEnumerable<OrdenesTrabajo> GetOrdenesTrabajoes()
        {
            var ordenestrabajos = db.OrdenesTrabajos.Include(o => o.Vehiculo);
            return ordenestrabajos.AsEnumerable().Select(ot => new OrdenesTrabajo()
            {
                Id = ot.Id,
                Fallas = ot.Fallas,
                Fecha = ot.Fecha,
                IdVehiculo = ot.IdVehiculo,
                NoOrden = ot.NoOrden,
                Diagnostico = ot.Diagnostico,
                Reparaciones = ot.Reparaciones,
                Servicios = ot.Servicios.Select(s => new Servicio
                {
                    Nombre = s.Nombre,
                    Id = s.Id
                }).ToList()
            });
        }

        // GET api/OrdenesTrabajos/5
        public OrdenesTrabajo GetOrdenesTrabajo(int id)
        {
            OrdenesTrabajo ordenestrabajo = db.OrdenesTrabajos.Find(id);
            if (ordenestrabajo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return ordenestrabajo;
        }

        [HttpPut]
        // PUT api/OrdenesTrabajos/5
        public HttpResponseMessage OrdenesTrabajo(OrdenesTrabajo ordentrabajo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.OrdenesTrabajos.AddOrUpdate(ordentrabajo);

                    var ot = db.OrdenesTrabajos.Find(ordentrabajo.Id);
                    ot.Servicios.Clear();
                    ot.InsumosProveedores.Clear();
                    var dgIds = ot.Diagnosticos.Select(dg => dg.IdDiagnostico).ToArray();
                    var dgActual = db.Diagnosticos.Where(dg => dg.IdOrden.Equals(ot.Id));
                    var dgIdsActual = dgActual.Select(dg => dg.IdDiagnostico).ToArray();

                    foreach (var diag in dgIds)
                    {
                        if (!dgIdsActual.Contains(diag))
                        {
                            var dbDG = db.Diagnosticos.Find(diag);
                            db.Diagnosticos.Remove(dbDG);
                        }
                    }

                    var cotizados = db.InsumosCotizados.Where(c => c.IdOrden.Equals(ordentrabajo.Id));

                    foreach (var cotizado in cotizados)
                    {
                        if (!ordentrabajo.InsumosCotizados.Any(ic => ic.IdInsumoCotizado.Equals(cotizado.IdInsumoCotizado)))
                        {
                            db.InsumosCotizados.Remove(cotizado);
                        }
                    }

                    foreach (var dg in ordentrabajo.Diagnosticos)
                    {
                        dg.IdOrden = ordentrabajo.Id;
                        dg.DiagnosticoEstado = null;
                        db.Diagnosticos.AddOrUpdate(dg);
                    }

                    foreach (var cotizado in ordentrabajo.InsumosCotizados)
                    {
                        db.InsumosCotizados.AddOrUpdate(cotizado);
                    }
                    db.SaveChanges();


                    foreach (var servicio in db.Servicios)
                    {
                        if (ordentrabajo.Servicios.Any(s => s.Id.Equals(servicio.Id)))
                        {
                            ot.Servicios.Add(servicio);
                        }
                    }

                    foreach (var insProv in ordentrabajo.InsumosProveedores)
                    {
                        var ins = db.Insumos.Find(insProv.IdInsumo);
                        var prov = db.Proveedores.Find(insProv.IdProveedor);

                        ot.InsumosProveedores.Add(new InsumosProveedore() { IdProveedor = insProv.IdProveedor, IdInsumo = insProv.IdInsumo, Precio = insProv.Precio, Cantidad = insProv.Cantidad });
                    }

                    
                    db.SaveChanges();
                }
                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK, db.View_OrdenesInfo.First(ot => ordentrabajo.Id.Equals(ot.Id)));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/OrdenesTrabajos
        public HttpResponseMessage PostOrdenesTrabajo(OrdenesTrabajo ordentrabajo)
        {
            if (ModelState.IsValid)
            {

                var selServs = new List<Servicio>(ordentrabajo.Servicios);
                var selInsProv = new List<InsumosProveedore>(ordentrabajo.InsumosProveedores);
                var diagnosticos = new List<Diagnostico>(ordentrabajo.Diagnosticos);
                var cotizados = new List<InsumosCotizados>(ordentrabajo.InsumosCotizados);


                ordentrabajo.Servicios.Clear();
                ordentrabajo.InsumosProveedores.Clear();
                ordentrabajo.Diagnosticos.Clear();

                foreach (var servicio in db.Servicios)
                {
                    if (selServs.Any(s => s.Id.Equals(servicio.Id)))
                    {
                        ordentrabajo.Servicios.Add(servicio);
                    }
                }

                foreach (var insProv in selInsProv)
                {
                    var ins = db.Insumos.Find(insProv.IdInsumo);
                    var prov = db.Proveedores.Find(insProv.IdProveedor);

                    ordentrabajo.InsumosProveedores.Add(new InsumosProveedore() { IdProveedor = insProv.IdProveedor, IdInsumo = insProv.IdInsumo, Precio = insProv.Precio, Cantidad = insProv.Cantidad });
                }
                ordentrabajo.NoOrden = db.OrdenesTrabajos.Max(ot => ot.NoOrden).Value + 1;
                db.OrdenesTrabajos.Add(ordentrabajo);

                foreach (var dg in diagnosticos)
                {
                    dg.DiagnosticoEstado = null;
                    db.Diagnosticos.Add(dg);
                }

                db.SaveChanges();

                ordentrabajo.Servicios = ordentrabajo.Servicios.Select(s => new Servicio
                {
                    Nombre = s.Nombre,
                    Id = s.Id
                }).ToList();


                ordentrabajo.InsumosProveedores = ordentrabajo.InsumosProveedores.Select(ip => new InsumosProveedore
                {
                    Precio = ip.Precio,
                    IdInsumo = ip.IdInsumo,
                    IdOrdenServicio = ip.IdOrdenServicio,
                    IdProveedor = ip.IdProveedor,
                    Proveedore = new Proveedore
                    {
                        Nombre = ip.Proveedore.Nombre
                    },
                    Cantidad = ip.Cantidad,
                    Insumo = new Insumo
                    {
                        Activo = ip.Insumo.Activo,
                        Nombre = ip.Insumo.Nombre,
                        IdInsumo = ip.IdInsumo
                    }
                }).ToList();

                var insumosCotizados = new List<InsumosCotizados>(ordentrabajo.InsumosCotizados);

                ordentrabajo.InsumosCotizados.Clear();

                foreach (var cotizado in insumosCotizados)
                {
                    cotizado.OrdenesTrabajos = null;
                    cotizado.Proveedores = null;
                    cotizado.MarcasInsumos = null;
                    cotizado.Insumos = null;
                    cotizado.IdOrden = ordentrabajo.Id;
                    ordentrabajo.InsumosCotizados.Add(cotizado);
                }

                ordentrabajo.Diagnosticos = diagnosticos.Select(d => new Diagnostico() { IdDiagnostico = d.IdDiagnostico, IdEstado = d.IdEstado,
                    IdOrden = d.IdOrden, Descripcion = d.Descripcion,
                    Ajustes = d.Ajustes
                }).ToList();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, db.View_OrdenesInfo.First(ot => ot.Id.Equals(ordentrabajo.Id)));
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = ordentrabajo.Id }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/OrdenesTrabajos/5
        public HttpResponseMessage DeleteOrdenesTrabajo(int id)
        {
            OrdenesTrabajo ordenestrabajo = db.OrdenesTrabajos.Find(id);
            if (ordenestrabajo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.OrdenesTrabajos.Remove(ordenestrabajo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, ordenestrabajo);
        }

        [HttpPut]
        public HttpResponseMessage AprobarDiagnostico(Diagnostico dg)
        {

            try
            {
                dg.IdEstado = 2;
                db.Diagnosticos.AddOrUpdate(dg);
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, db.View_OrdenesActivas.First(ot => dg.IdOrden.Equals(ot.Id)));
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}