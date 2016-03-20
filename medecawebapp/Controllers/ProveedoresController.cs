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

namespace MEDECAWebApp.Controllers
{
    public class ProveedoresController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public IEnumerable<Proveedore> GetProveedores()
        {
            return db.Proveedores.AsEnumerable().Select(x => new Proveedore { 
                Activo = x.Activo,
                IdProveedor = x.IdProveedor,
                Nombre = x.Nombre,
                RNC = x.RNC,
                Telefono = x.Telefono
            });
        }

        [HttpGet]
        public IEnumerable<Proveedore> GetProveedoresActivos()
        {
            return db.Proveedores.AsEnumerable().Where(x => x.Activo).Select(x => new Proveedore
            {
                Activo = x.Activo,
                IdProveedor = x.IdProveedor,
                Nombre = x.Nombre,
                RNC = x.RNC,
                Telefono = x.Telefono
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
        public HttpResponseMessage PutProveedore( Proveedore proveedore)
        {
            if (ModelState.IsValid )
            {
                db.Entry(proveedore).State = EntityState.Modified;
                
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
                db.Proveedores.Add(proveedore);
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