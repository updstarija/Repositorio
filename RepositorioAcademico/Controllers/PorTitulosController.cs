using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorTitulosController : Controller
    {
        // GET: PorTitulos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderBy(x => x.titulo).Where(x => x.idEstado == 1).ToList();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            string codigoHtml = detalle.TrabajosAcademicos(lista);
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPorTitulo(string titulo)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> proyectosEncontrados = db.TrabajoAcademico.OrderBy(x=> x.titulo).Where(x => x.idEstado == 1 && x.titulo.ToUpper().Contains(titulo.ToUpper())).ToList();
            string codigoHtml = "";
            if (proyectosEncontrados.Count() > 0)
            {
                DetalleDocumentoController detalle = new DetalleDocumentoController();
                codigoHtml = detalle.TrabajosAcademicos(proyectosEncontrados);
            }
            else
            {
                codigoHtml = "<p>No se encontraron proyectos que coincidan con ese título</p>";
            }
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }
    }
}