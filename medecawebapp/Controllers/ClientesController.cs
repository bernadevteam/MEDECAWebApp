﻿using System;
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
    public class ClientesController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public ValidarCedORNC_Result ExisteCliente(string field, int id)
        {
            return db.ValidarCedORNC(field, id).First();
        }

        [HttpGet]
        public IEnumerable<object> Clientes()
        {
            return db.Clientes.AsEnumerable().Select(x => new  { Direccion = x.Direccion, Email = x.Email, IDCliente = x.IDCliente, 
                Identificacion = x.Identificacion , TipoIdentificacion = x.TipoIdentificacion,
            Nombre = x.Nombre, Telefono = x.Telefono, Favorito = x.Favorito, CanDelete = x.Vehiculos.Count.Equals(0)});
        }

        [HttpGet]
        public IEnumerable<object> ClientesVehiculos()
        {
            return db.Clientes.AsEnumerable().Select(x => new 
            {
                Direccion = x.Direccion,
                Email = x.Email,
                IDCliente = x.IDCliente,
                Identificacion = x.Identificacion,
                TipoIdentificacion = x.TipoIdentificacion,
                Nombre = x.Nombre,
                Telefono = x.Telefono,
                Favorito = x.Favorito,
                Vehiculos = x.Vehiculos.Select(v => new 
                {
                    Info = string.Format("{0} {1} {2}", v.Modelo.VehiculoMarca.Nombre, v.Modelo.Nombre, v.Anio),
                    Modelo = new Modelo
                    {
                        Nombre = v.Modelo.Nombre,
                        IdMarca = v.Modelo.IdMarca,
                        VehiculoMarca = new VehiculoMarca
                        {
                            Nombre = v.Modelo.VehiculoMarca.Nombre
                        }
                    },
                    Anio = v.Anio,
                    Color = v.Color,
                    Combustible = new Combustible
                    {
                        Nombre = v.Combustible.Nombre
                    },
                    Kilometraje = v.Kilometraje,
                    UnidadDistancia = v.UnidadDistancia,
                    Insumos = v.Insumos.OrderBy(i => i.Nombre).Where( i => i.Activo).Select( i => new Insumo{
                        Nombre = i.Nombre,
                        IdInsumo = i.IdInsumo
                    }),
                    NoChasis = v.NoChasis,
                    Placa = v.Placa,
                    Cliente = new Cliente { 
                    Nombre = x.Nombre},
                    IdVehiculo = v.IdVehiculo,
                    Motor = v.Motor,
                    IdCliente = v.IdCliente,
                    IdModelo = v.IdModelo,
                    IdCombustible = v.IdCombustible,
                    CanDelete = v.OrdenesTrabajos.Count.Equals(0)
                }).ToList()
            });
        }

        [HttpGet]
        public IEnumerable<object> ProximasRevisiones()
        {
            return db.View_ClientesProximasRevisiones.Where( cp =>cp.IdEstado.Equals(1)).ToList();
        }

        [HttpGet]
        public object MantenerSesion()
        {
            return new { Active = true};
        }

        [HttpGet]
        public IEnumerable<object> ClientesOrdenes()
        {
            return db.Clientes.AsEnumerable().OrderBy(c => c.Nombre).Select(x => new
            {
                Direccion = x.Direccion,
                Email = x.Email,
                Favorito = x.Favorito,
                IDCliente = x.IDCliente,
                Identificacion = x.Identificacion,
                TipoIdentificacion = x.TipoIdentificacion,
                Nombre = x.Nombre,
                Telefono = x.Telefono,
                Vehiculos = x.Vehiculos.Select(v => new
                {
                    Info = string.Format("{0} {1} {2}", v.Modelo.VehiculoMarca.Nombre, v.Modelo.Nombre, v.Anio),
                    Modelo = new Modelo
                    {
                        Nombre = v.Modelo.Nombre,
                        IdMarca = v.Modelo.IdMarca,
                        VehiculoMarca = new VehiculoMarca
                        {
                            Nombre = v.Modelo.VehiculoMarca.Nombre
                        }
                    },
                    Anio = v.Anio,
                    Color = v.Color,
                    Combustible = new Combustible
                    {
                        Nombre = v.Combustible.Nombre
                    },
                    Kilometraje = v.Kilometraje,
                    UnidadDistancia = v.UnidadDistancia,
                    Insumos = v.Insumos.OrderBy(i => i.Nombre).Where(i => i.Activo).Select(i => new Insumo
                    {
                        Nombre = i.Nombre,
                        IdInsumo = i.IdInsumo
                    }),
                    NoChasis = v.NoChasis,
                    Placa = v.Placa,
                    IdVehiculo = v.IdVehiculo,
                    Motor = v.Motor,
                    IdCliente = v.IdCliente,
                    IdModelo = v.IdModelo,
                    IdCombustible = v.IdCombustible,
                    OrdenesTrabajos = v.OrdenesTrabajos.Select(ot => new
                    {
                        Id = ot.Id,
                        NoOrden = ot.NoOrden,
                        Fecha = ot.Fecha
                    }).ToList()
                }).ToList()
            });
        }
        // GET api/Clientes/5
        public Cliente GetCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                throw new HttpResponseException(Request.CreateResponse(HttpStatusCode.NotFound));
            }

            return cliente;
        }

        // PUT api/Clientes/5
        public HttpResponseMessage PutCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Entry(cliente).State = EntityState.Modified;

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

        // POST api/Clientes
        public HttpResponseMessage PostCliente(Cliente cliente)
        {
            if (ModelState.IsValid)
            {
                db.Clientes.Add(cliente);
                db.SaveChanges();

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created, cliente);
                response.Headers.Location = new Uri(Url.Link("DefaultApi", new { id = cliente.IDCliente }));
                return response;
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        // DELETE api/Clientes/5
        public HttpResponseMessage DeleteCliente(int id)
        {
            Cliente cliente = db.Clientes.Find(id);
            if (cliente == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            db.Clientes.Remove(cliente);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            return Request.CreateResponse(HttpStatusCode.OK, cliente);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}