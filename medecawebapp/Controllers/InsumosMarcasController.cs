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
    [Authorize]
    public class InsumosMarcasController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/InsumosMarcas
        public IEnumerable<MarcasInsumos> GetMarcasInsumos()
        {
            return db.MarcasInsumos.OrderBy(mi => mi.Nombre).AsEnumerable().Select(mi => new MarcasInsumos() { IdMarcaInsumo = mi.IdMarcaInsumo, Nombre = mi.Nombre});
        }

        // GET api/InsumosMarcas/5
        public MarcasInsumos GetMarcasInsumos(int id)
        {
            MarcasInsumos marcasinsumos = db.MarcasInsumos.Find(id);
            if (marcasinsumos == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return marcasinsumos;
        }

        // PUT api/InsumosMarcas/5
        public HttpResponseMessage PutMarcasInsumos(MarcasInsumos marcasinsumos)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            db.Entry(marcasinsumos).State = EntityState.Modified;

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

        // POST api/InsumosMarcas
        public HttpResponseMessage PostMarcasInsumos(MarcasInsumos marcasinsumos)
        {
            if (ModelState.IsValid)
            {
                db.MarcasInsumos.Add(marcasinsumos);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, marcasinsumos);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = marcasinsumos.IdMarcaInsumo }));
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }
        }

        // DELETE api/InsumosMarcas/5
        public HttpResponseMessage DeleteMarcasInsumos(int id)
        {
            MarcasInsumos marcasinsumos = db.MarcasInsumos.Find(id);
            if (marcasinsumos == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.MarcasInsumos.Remove(marcasinsumos);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }

            return Request.CreateResponse(HttpStatusCode.OK, marcasinsumos);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}