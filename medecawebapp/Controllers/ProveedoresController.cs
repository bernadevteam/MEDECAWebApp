using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace MEDECAWebApp.Controllers
{
    [Authorize]
    public class ProveedoresController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public IEnumerable<object> GetProveedores()
        {
            return db.Proveedores.OrderBy(p => p.Nombre).AsEnumerable().Select(x => new
            {
                Activo = x.Activo,
                IdProveedor = x.IdProveedor,
                Nombre = x.Nombre,
                RNC = x.RNC,
                Telefono = x.Telefono,
                Correo = x.Correo,
                Actividades = x.Actividades.Select(a => new Actividades() { IdActividad = a.IdActividad, Nombre = a.Nombre}),
                CanDelete = x.InsumosProveedores.Count.Equals(0)
            });
        }

        [HttpGet]
        public IEnumerable<Proveedore> GetProveedoresActivos()
        {
            return db.Proveedores.OrderBy(p => p.Nombre).AsEnumerable().Where(x => x.Activo).Select(x => new Proveedore
            {
                Activo = x.Activo,
                IdProveedor = x.IdProveedor,
                Nombre = x.Nombre,
                RNC = x.RNC,
                Telefono = x.Telefono,
                Actividades = x.Actividades.Select(a => new Actividades() { IdActividad = a.IdActividad, Nombre = a.Nombre }).ToList()
            });
        }

        [HttpGet]
        public ValidarProveedorRNC_Result ExisteRNC(string field, int id)
        {
            return db.ValidarProveedorRNC(field, id).First();
        }

        // GET api/Proveedores/5
        public Proveedore GetProveedore(int id)
        {
            Proveedore proveedore = db.Proveedores.Find(id);
            if (proveedore == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return proveedore;
        }

        // PUT api/Proveedores/5
        public HttpResponseMessage PutProveedore(Proveedore proveedore)
        {
            if (ModelState.IsValid)
            {
                //db.Entry(proveedore).State = EntityState.Modified;
                var prov = db.Proveedores.Find(proveedore.IdProveedor);
                db.Proveedores.AddOrUpdate(proveedore);
                var actividades = proveedore.Actividades.ToList();
                prov.Actividades.Clear();

                foreach (var act in db.Actividades)
                {
                    if (actividades.Any(i => i.IdActividad.Equals(act.IdActividad)))
                    {
                        prov.Actividades.Add(act);

                    }
                }

                foreach (var act in actividades)
                {
                    if (act.IdActividad.Equals(0))
                    {
                        prov.Actividades.Add(act);
                    }
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

        // POST api/Proveedores
        public HttpResponseMessage PostProveedore(Proveedore proveedore)
        {
            if (ModelState.IsValid)
            {
                var actividades = proveedore.Actividades;
                db.Proveedores.Add(proveedore);
                db.SaveChanges();
                var prov = db.Proveedores.Find(proveedore.IdProveedor);
                foreach (var act in db.Actividades)
                {
                    if (actividades.Any(i => i.IdActividad.Equals(act.IdActividad)))
                    {
                        prov.Actividades.Add(act);

                    }
                }

                foreach (var act in actividades)
                {
                    if (act.IdActividad.Equals(0))
                    {
                        prov.Actividades.Add(act);
                    }

                }

                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, proveedore);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = proveedore.IdProveedor }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Proveedores/5
        public HttpResponseMessage DeleteProveedore(int id)
        {
            Proveedore proveedore = db.Proveedores.Find(id);
            if (proveedore == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Proveedores.Remove(proveedore);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, proveedore);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}