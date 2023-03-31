using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorAutoresController : Controller
    {
        // GET: PorAutores
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Proyectos(int idEstudiante)
        {
            SAADSTJEntities dbS = new SAADSTJEntities();
            var estudiante = dbS.Persona.Single(x => x.Estudiante.Id == idEstudiante);
            ViewBag.idEstudiante = idEstudiante;
            ViewBag.Estudiante = estudiante.Nombre + " " + estudiante.ApellidoPaterno + " " + estudiante.ApellidoMaterno;
            return View();
        }

        public ActionResult Listar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            var autores = db.TrabajoAcademico.Where(x =>  x.idEstado == 1).GroupBy(x=> x.idEstudiante).ToList();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de los autores</li>";
            foreach (var item in autores)
            {
                var persona = dbS.Persona.Single(x => x.Id == item.Key);
                string nombreAutor = persona.Nombre + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
                int proyectos = item.Count();
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorAutores", new { idEstudiante = item.Key }) + "'>" + nombreAutor + "</a><span> [ " + proyectos + " ]</span></li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        } 

        public ActionResult BuscarAutor(string autor)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            string codHTML = "<li class='list-group-item list-group-item-action list-group-item-primary'>Nombre de los autores</li>";
            var estudiantes = dbS.Estudiante.OrderBy(x=> x.Persona.Nombre).Where(x => (x.Persona.Nombre + " " + x.Persona.ApellidoPaterno + " " + x.Persona.ApellidoMaterno).ToUpper().Contains(autor.ToUpper())).ToList();
            if (estudiantes.Count() > 0)
            {
                foreach (var item in estudiantes)
                {
                    var proyectos = db.TrabajoAcademico.Count(x => x.idEstudiante == item.Id && x.idEstado == 1);
                    if (proyectos > 0)
                    {
                        string nombreAutor = item.Persona.Nombre + " " + item.Persona.ApellidoPaterno + " " + item.Persona.ApellidoMaterno;
                        codHTML += "<li class='list-group-item list-group-item-action list-busqueda'><a href='" + Url.Action("Proyectos", "PorAutores", new { idEstudiante = item.Id }) + "'>" + nombreAutor + "</a><span> [ " + proyectos + " ]</span></li>";
                    }
                }
            }
            else
            {
                codHTML += "<li class='list-group-item list-group-item-action list-busqueda'>No se encontraron autores con ese nombre</li>";
            }
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarPorAutor(int idEstudiante)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderByDescending(x => x.fechaRegistro).Where(x => x.idEstado == 1 && x.idEstudiante == idEstudiante).ToList();
            string codigoHtml = detalle.TrabajosAcademicos(lista);
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }
    }
}