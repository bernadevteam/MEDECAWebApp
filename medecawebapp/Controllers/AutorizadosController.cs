using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using MEDECAWebApp;
using MEDECAWebApp.Models;

namespace MEDECAWebApp.Controllers
{
    public class AutorizadosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET: api/Autorizados
        public IQueryable<object> GetAutorizados()
        {
            var autorizados = db.Autorizados.Where(a => a.AppId.Equals(1)).Select(a => new { Id = a.Id, Correo = a.Correo, CanDelete = User.Identity.Name != a.Correo});
            return autorizados;
        }

        // GET: api/Autorizados/5
        [ResponseType(typeof(Autorizados))]
        public IHttpActionResult GetAutorizados(int id)
        {
            Autorizados autorizados = db.Autorizados.Find(id);
            if (autorizados == null)
            {
                return NotFound();
            }

            return Ok(autorizados);
        }

        // PUT: api/Autorizados/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAutorizados(int id, Autorizados autorizados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != autorizados.Id)
            {
                return BadRequest();
            }

            db.Entry(autorizados).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AutorizadosExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Autorizados
        [ResponseType(typeof(Autorizados))]
        public IHttpActionResult PostAutorizados(Autorizados autorizados)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            autorizados.AppId = 1;
            db.Autorizados.Add(autorizados);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = autorizados.Id }, new { Id = autorizados.Id, Correo = autorizados.Correo, CanDelete = true});
        }

        // DELETE: api/Autorizados/5
        [ResponseType(typeof(Autorizados))]
        public IHttpActionResult DeleteAutorizados(int id)
        {
            Autorizados autorizados = db.Autorizados.Find(id);
            if (autorizados == null)
            {
                return NotFound();
            }

            db.Autorizados.Remove(autorizados);
            db.SaveChanges();

            return Ok(autorizados);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AutorizadosExists(int id)
        {
            return db.Autorizados.Count(e => e.Id == id) > 0;
        }
    }
}