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
    public class OrdenesTrabajosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        [System.Web.Mvc.OutputCache(NoStore = true, Duration = 0, VaryByParam = "None")]
        public ValidarPlaca_Result ExisteNoOrden(int field, int id)
        {
            return db.ValidarNoOrden(field, id).First();
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
                Reparaciones = ot.Reparaciones,
                Servicios = ot.Servicios.Select(s => new Servicio { 
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

        // PUT api/OrdenesTrabajos/5
        public HttpResponseMessage PutOrdenesTrabajo(OrdenesTrabajo ordenestrabajo)
        {
            if (ModelState.IsValid)
            {
                db.OrdenesTrabajos.AddOrUpdate(ordenestrabajo);

                var ot = db.OrdenesTrabajos.Find(ordenestrabajo.Id);
                ot.Servicios.Clear();
                ot.InsumosProveedores.Clear();

                foreach (var servicio in db.Servicios)
                {
                    if (ordenestrabajo.Servicios.Any(s => s.Id.Equals(servicio.Id)))
                    {
                        ot.Servicios.Add(servicio);
                    }
                }

                foreach (var insProv in ordenestrabajo.InsumosProveedores)
                {
                    var ins = db.Insumos.Find(insProv.IdInsumo);
                    var prov = db.Proveedores.Find(insProv.IdProveedor);

                    ot.InsumosProveedores.Add(new InsumosProveedore() { IdProveedor = insProv.IdProveedor, IdInsumo = insProv.IdInsumo, Precio = insProv.Precio});
                }
                
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }

                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/OrdenesTrabajos
        public HttpResponseMessage PostOrdenesTrabajo(OrdenesTrabajo ordenestrabajo)
        {
            if (ModelState.IsValid)
            {

                var selServs = new List<Servicio>(ordenestrabajo.Servicios);
                var selInsProv = new List<InsumosProveedore>(ordenestrabajo.InsumosProveedores);

                ordenestrabajo.Servicios.Clear();
                ordenestrabajo.InsumosProveedores.Clear();

                foreach (var servicio in db.Servicios)
                {
                    if (selServs.Any(s => s.Id.Equals(servicio.Id)))
                    {
                        ordenestrabajo.Servicios.Add(servicio);
                    }
                }

                foreach (var insProv in selInsProv)
                {
                    var ins = db.Insumos.Find(insProv.IdInsumo);
                    var prov = db.Proveedores.Find(insProv.IdProveedor);

                    ordenestrabajo.InsumosProveedores.Add(new InsumosProveedore() { IdProveedor = insProv.IdProveedor, IdInsumo = insProv.IdInsumo, Precio = insProv.Precio });
                }

                db.OrdenesTrabajos.Add(ordenestrabajo);
                db.SaveChanges();

                ordenestrabajo.Servicios = ordenestrabajo.Servicios.Select(s => new Servicio
                {
                    Nombre = s.Nombre,
                    Id = s.Id
                }).ToList();
                ordenestrabajo.InsumosProveedores = ordenestrabajo.InsumosProveedores.Select(ip => new InsumosProveedore
                        {
                            Precio = ip.Precio,
                            IdInsumo = ip.IdInsumo,
                            IdOrdenServicio = ip.IdOrdenServicio,
                            IdProveedor = ip.IdProveedor,
                            Insumo = new Insumo
                            {
                                Activo = ip.Insumo.Activo,
                                Nombre = ip.Insumo.Nombre,
                                IdInsumo = ip.IdInsumo
                            }
                        }).ToList();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, ordenestrabajo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = ordenestrabajo.Id }));
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

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}