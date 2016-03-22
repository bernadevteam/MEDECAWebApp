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
    public class InsumosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/Insumos
        public IEnumerable<object> GetInsumoes()
        {
            return db.Insumos.AsEnumerable().Select(x => new { Activo = x.Activo, Nombre = x.Nombre, IdInsumo = x.IdInsumo, CanDelete = x.Vehiculos.Count.Equals(0)});
        }

        // GET api/Insumos/5
        public Insumo GetInsumo(int id)
        {
            Insumo insumo = db.Insumos.Find(id);
            if (insumo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return insumo;
        }

        // PUT api/Insumos/5
        public HttpResponseMessage PutInsumo(Insumo insumo)
        {
            if (ModelState.IsValid)
            {
                db.Entry(insumo).State = EntityState.Modified;

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

        // POST api/Insumos
        public HttpResponseMessage PostInsumo(Insumo insumo)
        {
            if (ModelState.IsValid)
            {
                db.Insumos.Add(insumo);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, insumo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = insumo.IdInsumo }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Insumos/5
        public HttpResponseMessage DeleteInsumo(int id)
        {
            Insumo insumo = db.Insumos.Find(id);
            if (insumo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Insumos.Remove(insumo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, insumo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}