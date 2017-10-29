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

namespace MEDECAWebApp.Controllers
{
    public class AlertasProximosChequeosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        // GET: api/AlertasProximosChequeos
        public IQueryable<AlertasProximosChequeo> GetAlertasProximosChequeos()
        {
            return db.AlertasProximosChequeos;
        }

        // GET: api/AlertasProximosChequeos/5
        [ResponseType(typeof(AlertasProximosChequeo))]
        public IHttpActionResult GetAlertasProximosChequeo(int id)
        {
            AlertasProximosChequeo alertasProximosChequeo = db.AlertasProximosChequeos.Find(id);
            if (alertasProximosChequeo == null)
            {
                return NotFound();
            }

            return Ok(alertasProximosChequeo);
        }

        // PUT: api/AlertasProximosChequeos/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAlertasProximosChequeo(int id, int idEstado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                db.ActualizarEstadoAlerta(id, idEstado);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AlertasProximosChequeoExists(id))
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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AlertasProximosChequeoExists(int id)
        {
            return db.AlertasProximosChequeos.Count(e => e.IdAlerta == id) > 0;
        }
    }
}