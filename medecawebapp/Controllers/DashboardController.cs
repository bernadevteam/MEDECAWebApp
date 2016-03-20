using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MEDECAWebApp.Controllers
{
    public class DashboardController : ApiController
    {
        private MEDECAEntities db = new MEDECAEntities();

        [HttpGet]
        public IEnumerable<Dasboard> GetDashboard() {
            return db.prc_Dashboard().AsEnumerable().Select(d => new Dasboard { 
                Nombre = d.Nombre,
                Valor = d.Valor
            });
        }
        [HttpGet]
        public IEnumerable<ObtenerVehiculosCambioAceite_Result> ServiciosContinuos()
        {
            return db.ObtenerVehiculosCambioAceite().AsEnumerable();
        }


        [HttpGet]
        public IEnumerable<ObtenerVehiculosCambioAceiteProximo_Result> ProximosCambiosAceite()
        {
            return db.ObtenerVehiculosCambioAceiteProximo().AsEnumerable();
        }
    }
}
