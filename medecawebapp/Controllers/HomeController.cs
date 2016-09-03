using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MEDECAWebApp.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult VehiculosMarcas() {
            return View();
        }

        public ActionResult Actividades()
        {
            return View();
        }

        public ActionResult InsumosMarcas()
        {
            return View();
        }

        public ActionResult Modelos()
        {
            return View();
        }

        public ActionResult Insumos()
        {
            return View();
        }

        public ActionResult Clientes()
        {
            return View();
        }

        public ActionResult Vehiculos()
        {
            return View();
        }

        public ActionResult Citas()
        {
            return View();
        }

        public ActionResult OrdenesTrabajos()
        {
            return View();
        }

        public ActionResult Reportes()
        {
            return View();
        }

        public ActionResult Proveedores()
        {
            return View();
        }

        public ActionResult Servicios()
        {
            return View();
        }
    }
}
