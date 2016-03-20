using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MEDECAWebApp.Controllers
{
    public class ReportesController : ApiController
    {
         private MEDECAEntities db = new MEDECAEntities();

         [HttpGet]
         public IEnumerable<HistoricoServicios> Historico() {
             return db.ObtenerHistoricoServicios().AsEnumerable();
         }

         [HttpGet]
         public IEnumerable<ObtenerHistoricoServiciosConteo_Result> HistoricoConteo()
         {
             return db.ObtenerHistoricoServiciosConteo().AsEnumerable();
         }

         [HttpGet]
         public IEnumerable<ObtenerHistoricoServiciosMesesConteo_Result> HistoricoMesesConteo()
         {
             return db.ObtenerHistoricoServiciosMesesConteo().AsEnumerable();
         }

    }
}
