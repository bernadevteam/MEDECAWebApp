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
    public class ActividadesController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/Actividades
        public IEnumerable<Actividades> GetActividades()
        {
            return db.Actividades.AsEnumerable();
        }

        // GET api/Actividades/5
        public Actividades GetActividades(int id)
        {
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return actividades;
        }

        // PUT api/Actividades/5
        public HttpResponseMessage PutActividades(Actividades actividades)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            db.Entry(actividades).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }

        // POST api/Actividades
        public HttpResponseMessage PostActividades(Actividades actividades)
        {
            if (ModelState.IsValid)
            {
                db.Actividades.Add(actividades);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, actividades);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = actividades.IdActividad }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/Actividades/5
        public HttpResponseMessage DeleteActividades(int id)
        {
            Actividades actividades = db.Actividades.Find(id);
            if (actividades == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Actividades.Remove(actividades);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, actividades);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}