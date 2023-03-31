using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorCarrerasController : Controller
    {
        // GET: PorCarreras
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Proyectos(int idCarrera)
        {
            SAADSTJEntities dbS = new SAADSTJEntities();
            string carrera = dbS.PlanEstudio.Single(x => x.Id == idCarrera).Nombre;
            ViewBag.idCarrera = idCarrera;
            ViewBag.Carrera = carrera;
            return View();
        }

        public ActionResult Listar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            var carreras = db.TrabajoAcademico.Where(x => x.idEstado == 1).GroupBy(x => x.idCarrera).ToList();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de las carreras</li>";
            foreach (var item in carreras)
            {
                var carrera = dbS.PlanEstudio.Single(x => x.Id == item.Key).Nombre;
                int proyectos = item.Count();
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorCarreras", new { idCarrera = item.Key }) + "'>" + carrera + "</a><span> [ " + proyectos + " ]</span></li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarCarrera(string carrera)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de las carreras</li>";
            var carreras = dbS.PlanEstudio.OrderBy(x=> x.Nombre).Where(x => x.Nombre.ToUpper().Contains(carrera.ToUpper())).ToList();
            if (carreras.Count() > 0)
            {
                foreach (var item in carreras)
                {
                    var proyectos = db.TrabajoAcademico.Count(x => x.idCarrera == item.Id && x.idEstado == 1);
                    if (proyectos > 0)
                    {
                        codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorCarreras", new { idCarrera = item.Id }) + "'>" + item.Nombre + "</a><span> [ " + proyectos + " ]</span></li>";
                    }
                }
            }
            else
            {
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'>No se encontraron carreras con ese nombre</li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarPorCarrera(int idCarrera)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderByDescending(x => x.fechaRegistro).Where(x => x.idEstado == 1 && x.idCarrera == idCarrera).ToList();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            string codigoHtml = detalle.TrabajosAcademicos(lista);
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarProyectoPorTituloCarrera(int idCarrera, string titulo)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderBy(x => x.titulo).Where(x => x.idEstado == 1 && x.idCarrera == idCarrera && x.titulo.ToUpper().Contains(titulo.ToUpper())).ToList();
            string codigoHtml = "";
            if (lista.Count() > 0)
            {
                DetalleDocumentoController detalle = new DetalleDocumentoController();
                codigoHtml = detalle.TrabajosAcademicos(lista);
            }
            else
            {
                codigoHtml = "<p>No se encontraron proyectos que coincidan con ese título</p>";
            }
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }
    }
}