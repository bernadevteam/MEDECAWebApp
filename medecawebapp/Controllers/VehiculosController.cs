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
using System.Data.Entity.Migrations;

namespace MEDECAWebApp.Controllers
{
    public class VehiculosController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public IEnumerable<object> GetVehiculos()
        {
            var vehiculos = db.Vehiculos.Include(v => v.Cliente).Include(v => v.Combustible).Include(v => v.Modelo);
            return vehiculos.AsEnumerable().Select(x => new Models.VehiculoModel
            {
                Anio = x.Anio,
                Color = x.Color,
                IdCliente = x.IdCliente,
                IdCombustible = x.IdCombustible,
                Cliente = new Cliente{
                    Direccion = x.Cliente.Direccion,
                    Nombre = x.Cliente.Nombre,
                    IDCliente = x.IdCliente
                },
                IdModelo = x.IdModelo,
                IdMarca = x.Modelo.IdMarca,
                IdVehiculo = x.IdVehiculo,
                Insumos = x.Insumos.Select(i => new Insumo { IdInsumo = i.IdInsumo, Nombre = i.Nombre, Activo = i.Activo}).ToList(),
                Kilometraje = x.Kilometraje,
                UnidadDistancia = x.UnidadDistancia,

                NoChasis = x.NoChasis,
                Placa = x.Placa,
                Info = Models.VehiculoModel.GetInfo(x)
            });
        }

        [HttpGet]
        public ValidarPlaca_Result ExistePlaca(string field, int id)
        {
            return db.ValidarPlaca(field, id).First();
        }
        // GET api/Vehiculos/5
        public Vehiculo GetVehiculo(int id)
        {
            Vehiculo vehiculo = db.Vehiculos.Find(id);
            if (vehiculo == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return vehiculo;
        }

        // PUT api/Vehiculos/5
        public HttpResponseMessage PutVehiculo( MEDECAWebApp.Models.VehiculoModel vehiculo)
        {
            if (ModelState.IsValid)
            {
                var insumos = vehiculo.Insumos.ToList();

                db.Vehiculos.AddOrUpdate(vehiculo);
                var vh = db.Vehiculos.Find(vehiculo.IdVehiculo);
                vh.Insumos.Clear();
                foreach(var insumo in db.Insumos){
                    if (insumos.Any(i => i.IdInsumo.Equals(insumo.IdInsumo))) { 
                        vh.Insumos.Add(insumo);
                    }
                }

                foreach (var insumo in insumos)
                {
                    if (insumo.IdInsumo.Equals(0))
                    {
                        vh.Insumos.Add(insumo);
                    }
                }

                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateConcurrencyException)
                {
                    return Request.CreateResponse(HttpStatusCode.NotFound);
                }
                Modelo selModelo = db.Modelos.Include(m => m.VehiculoMarca).Where(x => x.IdModelo.Equals(vehiculo.IdModelo)).AsEnumerable().Select(m => new Modelo
                {
                    Nombre = m.Nombre,
                    IdMarca = m.IdMarca,
                    VehiculoMarca = new VehiculoMarca() { 
                        IdMarca = m.IdMarca,
                        Nombre = m.VehiculoMarca.Nombre
                    }

                }).First();
                vehiculo.Insumos = vh.Insumos.Select(i => new Insumo
                {
                    IdInsumo = i.IdInsumo,
                    Nombre = i.Nombre,
                    Activo = i.Activo
                }).ToList();
                vehiculo.Info = string.Format("{0} {1} {2}", selModelo.VehiculoMarca.Nombre, selModelo.Nombre, vehiculo.Anio);
                vehiculo.Modelo = selModelo;
                //vehiculo.Combustible.Nombre = vh.Combustible.Nombre;

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.OK, vehiculo);

                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // POST api/Vehiculos
        public HttpResponseMessage PostVehiculo(Vehiculo vehiculo)
        {
            if (ModelState.IsValid)
            {
                var insumos = vehiculo.Insumos.ToList();
                vehiculo.Insumos.Clear();
                db.Vehiculos.Add(vehiculo);


                db.SaveChanges();
                var newVeh = db.Vehiculos
                    .Include(vehi => vehi.Combustible)
                    .First(veh => veh.IdVehiculo.Equals(vehiculo.IdVehiculo));

                foreach (var insumo in db.Insumos)
                {
                    if (insumos.Any(i => i.IdInsumo.Equals(insumo.IdInsumo)))
                    {
                        newVeh.Insumos.Add(insumo);
                    }
                }

                foreach (var insumo in insumos)
                {
                    if (insumo.IdInsumo.Equals(0))
                    {
                        newVeh.Insumos.Add(insumo);
                    }
                }

                db.SaveChanges();
                vehiculo.Insumos = insumos.Select(i => new Insumo
                {
                    IdInsumo = i.IdInsumo,
                    Nombre = i.Nombre
                }).ToList();

                Modelo selModelo = db.Modelos.Include(m => m.VehiculoMarca).Where(x => x.IdModelo.Equals(vehiculo.IdModelo)).AsEnumerable().Select(m => new Modelo
                {
                    Nombre = m.Nombre,
                    IdMarca = m.IdMarca,
                    VehiculoMarca = new VehiculoMarca()
                    {
                        IdMarca = m.IdMarca,
                        Nombre = m.VehiculoMarca.Nombre
                    }

                }).First();
                vehiculo.Modelo = selModelo;
                MEDECAWebApp.Models.VehiculoModel anew = new MEDECAWebApp.Models.VehiculoModel();
                anew.Info = string.Format("{0} {1} {2}", selModelo.VehiculoMarca.Nombre, selModelo.Nombre, vehiculo.Anio);
                anew.Anio = newVeh.Anio;
                anew.Color = newVeh.Color;
                anew.IdCliente = newVeh.IdCliente;
                anew.IdCombustible = newVeh.IdCombustible;
                anew.IdModelo = newVeh.IdModelo;
                anew.IdVehiculo = newVeh.IdVehiculo;
                anew.Insumos = insumos;
                anew.Kilometraje = newVeh.Kilometraje;
                anew.UnidadDistancia = newVeh.UnidadDistancia;
                anew.Modelo = selModelo;
                anew.NoChasis = newVeh.NoChasis;
                anew.Placa = newVeh.Placa;
                anew.CanDelete = true;
                anew.Combustible = new Combustible {
                    IdCombustible = newVeh.Combustible.IdCombustible,
                    Nombre = newVeh.Combustible.Nombre
                };

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, anew);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = vehiculo.IdVehiculo }));
                return response;

            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Vehiculos/5
        public HttpResponseMessage DeleteVehiculo(int id)
        {
            Vehiculo vehiculo = db.Vehiculos.Find(id);
            if (vehiculo == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            vehiculo.Insumos.Clear();

            db.Vehiculos.Remove(vehiculo);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, vehiculo);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}