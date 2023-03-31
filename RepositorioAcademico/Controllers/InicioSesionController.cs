using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RepositorioAcademico.Controllers
{
    public class InicioSesionController : Controller
    {
        // GET: InicioSesion
        public ActionResult Index()
        {
            return View();
        }
    }
}