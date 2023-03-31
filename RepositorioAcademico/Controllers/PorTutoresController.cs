using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorTutoresController : Controller
    {
        // GET: PorTutores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Proyectos(int idTutor)
        {
            SAADSTJEntities dbS = new SAADSTJEntities();
            var tutor = dbS.Persona.Single(x => x.Docente.Id == idTutor);
            ViewBag.idTutor = idTutor;
            ViewBag.Tutor = tutor.Nombre + " " + tutor.ApellidoPaterno + " " + tutor.ApellidoMaterno;
            return View();
        }

        public ActionResult Listar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            var tutores = db.TrabajoAcademico.Where(x => x.idEstado == 1 && x.idTutor != 0).GroupBy(x => x.idTutor).ToList();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de los tutores</li>";
            foreach (var item in tutores)
            {
                var persona = dbS.Persona.Single(x => x.Id == item.Key);
                string nombreTutor = persona.Nombre + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
                int proyectos = item.Count();
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorTutores", new { idTutor = item.Key }) + "'>" + nombreTutor + "</a><span> [ " + proyectos + " ]</span></li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarTutor(string tutor)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de los tutores</li>";
            var docentes = dbS.Docente.OrderBy(x=> x.Persona.Nombre).Where(x => (x.Persona.Nombre + " " + x.Persona.ApellidoPaterno + " " + x.Persona.ApellidoMaterno).ToUpper().Contains(tutor.ToUpper())).ToList();
            if (docentes.Count() > 0)
            {
                foreach (var item in docentes)
                {
                    var proyectos = db.TrabajoAcademico.Count(x => x.idTutor == item.Id && x.idEstado == 1);
                    if (proyectos > 0)
                    {
                        string nombreTutor = item.Persona.Nombre + " " + item.Persona.ApellidoPaterno + " " + item.Persona.ApellidoMaterno;
                        codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorTutores", new { idTutor = item.Id }) + "'>" + nombreTutor + "</a><span> [ " + proyectos + " ]</span></li>";
                    }
                }
            }
            else
            {
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'>No se encontraron tutores con ese nombre</li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarPorTutor(int idTutor)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderByDescending(x => x.fechaRegistro).Where(x => x.idEstado == 1 && x.idTutor == idTutor).ToList();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            string codigoHtml = detalle.TrabajosAcademicos(lista);
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarProyectoPorTituloTutor(int idTutor, string titulo)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderByDescending(x => x.titulo).Where(x => x.idEstado == 1 && x.idTutor == idTutor && x.titulo.ToUpper().Contains(titulo.ToUpper())).ToList();
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