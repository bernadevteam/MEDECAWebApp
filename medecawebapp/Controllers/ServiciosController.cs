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
    public class ServiciosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/Servicios
        public IEnumerable<Servicio> GetServicios()
        {
            return db.Servicios.AsEnumerable().Select( s => new Servicio(){
                Id = s.Id,
                Nombre = s.Nombre
            });
        }

        // GET api/Servicios/5
        public Servicio GetServicio(int id)
        {
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return servicio;
        }

        // PUT api/Servicios/5
        public HttpResponseMessage PutServicio(int id, Servicio servicio)
        {
            if (ModelState.IsValid && id == servicio.Id)
            {
                db.Entry(servicio).State = EntityState.Modified;

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

        // POST api/Servicios
        public HttpResponseMessage PostServicios(Servicio[] servicios)
        {
            if (ModelState.IsValid)
            {
                foreach (Servicio serv in servicios) {
                    db.Servicios.AddOrUpdate(serv);
                }
                db.SaveChanges();
                var serviciosRes = db.Servicios.Select(s => new Servicio { Id = s.Id, Nombre = s.Nombre});
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, serviciosRes);
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Servicios/5
        public HttpResponseMessage DeleteServicio(int id)
        {
            Servicio servicio = db.Servicios.Find(id);
            if (servicio == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Servicios.Remove(servicio);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, servicio);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}