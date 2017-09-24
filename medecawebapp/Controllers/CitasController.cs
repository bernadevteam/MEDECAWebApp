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
    public class CitasController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public IEnumerable<Cita> Citas()
        {
            var citas = db.Citas;
            return citas.AsEnumerable();
        }

        [HttpGet]
        public IEnumerable<ObtenerCitasdeHoy_Result> CitasdeHoy()
        {
            return db.ObtenerCitasdeHoy().AsEnumerable();
        }
        [HttpGet]
        public IEnumerable<MEDECAWebApp.Models.ClienteModel> Clientes()
        {
            return db.Clientes.AsEnumerable().Select(x => new MEDECAWebApp.Models.ClienteModel
            {
                IDCliente = x.IDCliente,
                Nombre = x.Nombre,
                Telefono = x.Telefono,
                Vehiculos = x.Vehiculos.Select(v => new Vehiculo
                {
                    Modelo = new Modelo
                    {
                        Nombre = v.Modelo.Nombre,
                        VehiculoMarca = new VehiculoMarca
                        {
                            Nombre = v.Modelo.VehiculoMarca.Nombre
                        }
                    },
                    Anio = v.Anio,
                    Motor = v.Motor,
                    Color = v.Color,
                    Combustible = new Combustible
                    {
                        Nombre = v.Combustible.Nombre
                    },
                    Kilometraje = v.Kilometraje,
                    NoChasis = v.NoChasis,
                    Placa = v.Placa,
                    Cliente = new Cliente
                    {
                        IDCliente = x.IDCliente,
                        Nombre = x.Nombre
                    },
                    IdVehiculo = v.IdVehiculo
                }).ToList()
            });
        }


        // GET api/Citas/5
        public Cita GetCita(int id)
        {
            Cita cita = db.Citas.Find(id);
            if (cita == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cita;
        }

        // PUT api/Citas/5
        public HttpResponseMessage PutCita(Cita cita)
        {
            if (ModelState.IsValid )
            {
                db.Entry(cita).State = EntityState.Modified;
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

        // POST api/Citas
        public HttpResponseMessage PostCita(Cita cita)
        {
            if (ModelState.IsValid)
            {
                db.Citas.Add(cita);
                try { 
                db.SaveChanges();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException ex) {
                    throw ex;
                }
                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cita);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cita.IdCita }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Citas/5
        public HttpResponseMessage DeleteCita(int id)
        {
            Cita cita = db.Citas.Find(id);
            if (cita == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Citas.Remove(cita);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cita);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}