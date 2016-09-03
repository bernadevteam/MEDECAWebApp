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
    public class ModelosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/Modelos
        public IEnumerable<Modelo> GetModeloes()
        {
            var modelos = db.Modelos.Include(m => m.VehiculoMarca);
            return modelos.AsEnumerable().OrderBy(m => m.Nombre).ThenBy(m => m.VehiculoMarca.Nombre).Select(x => new Modelo()
            {
                IdMarca = x.IdMarca,
                Nombre = x.Nombre,
                IdModelo = x.IdModelo,
                VehiculoMarca = new VehiculoMarca {
                    IdMarca = x.VehiculoMarca.IdMarca,
                    Nombre = x.VehiculoMarca.Nombre
                }
            });
        }

        // GET api/Modelos/5
        public Modelo GetModelo(int id)
        {
            Modelo modelo = db.Modelos.Find(id);
            if (modelo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return modelo;
        }

        // PUT api/Modelos/5
        public HttpResponseMessage PutModelo(Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.VehiculoMarca = null;

                db.Entry(modelo).State = EntityState.Modified;

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

        // POST api/Modelos
        public HttpResponseMessage PostModelo(Modelo modelo)
        {
            if (ModelState.IsValid)
            {
                modelo.VehiculoMarca = null;
                db.Modelos.Add(modelo);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, modelo);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = modelo.IdModelo }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Modelos/5
        public HttpResponseMessage DeleteModelo(int id)
        {
            Modelo modelo = db.Modelos.Find(id);
            if (modelo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Modelos.Remove(modelo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, modelo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}