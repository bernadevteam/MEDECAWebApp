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
    public class VehiculoMarcasController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET api/VehiculoMarcas
        public IEnumerable<VehiculoMarca> GetVehiculoMarcas()
        {
            return db.VehiculoMarcas.AsEnumerable().Select(x => new VehiculoMarca() { IdMarca = x.IdMarca, Nombre = x.Nombre});
        }


        // GET api/VehiculoMarcas/5
        public VehiculoMarca GetVehiculoMarca(int id)
        {
            VehiculoMarca vehiculomarca = db.VehiculoMarcas.Find(id);
            if (vehiculomarca == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vehiculomarca;
        }

        // PUT api/VehiculoMarcas/5
        public HttpResponseMessage PutVehiculoMarca(VehiculoMarca vehiculomarca)
        {
            if (ModelState.IsValid)
            {
                db.Entry(vehiculomarca).State = EntityState.Modified;

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

        // POST api/VehiculoMarcas
        public HttpResponseMessage PostVehiculoMarca(VehiculoMarca vehiculomarca)
        {
            if (ModelState.IsValid)
            {
                db.VehiculoMarcas.Add(vehiculomarca);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, vehiculomarca);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = vehiculomarca.IdMarca }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/VehiculoMarcas/5
        public HttpResponseMessage DeleteVehiculoMarca(int id)
        {
            VehiculoMarca vehiculomarca = db.VehiculoMarcas.Find(id);
            if (vehiculomarca == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.VehiculoMarcas.Remove(vehiculomarca);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, vehiculomarca);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}