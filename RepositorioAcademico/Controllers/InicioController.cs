using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class InicioController : Controller
    {
        // GET: Inicio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Listar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            List<TrabajoAcademico> trabajos = db.TrabajoAcademico.OrderByDescending(x => x.fechaRegistro).Where(x => x.idEstado == 1).ToList();
            string codigoHtml = "";
            if (trabajos.Count() > 0)
            {
                codigoHtml = detalle.TrabajosAcademicos(trabajos);
            }
            else
            {
                codigoHtml = "<p>El repositorio no cuenta con trabajaros académicos por el momento.</p>";
            }
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }

        // metodo para realizar una busqueda profunda dentro del repositorio de trabajos academicos
        // buscara por titulo, descripcion, autor y tutor
        public ActionResult BuscarTodo(string palabas)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> proyectosEncontrados = db.TrabajoAcademico.Where(x => x.idEstado == 1 && x.titulo.ToUpper().Contains(palabas.ToUpper()) || x.descripcion.ToUpper().Contains(palabas.ToUpper())).ToList();
            var estudiantes = dbS.Estudiante.Where(x => (x.Persona.Nombre + " " + x.Persona.ApellidoPaterno + " " + x.Persona.ApellidoMaterno).ToUpper().Contains(palabas.ToUpper())).ToList();
            if (estudiantes.Count() > 0)
            {
                foreach (var item in estudiantes)
                {
                    var proyectosPorAutores = db.TrabajoAcademico.FirstOrDefault(x => x.idEstudiante == item.Id && x.idEstado == 1);
                    if (proyectosPorAutores != null)
                    {
                        proyectosEncontrados.Add(proyectosPorAutores);
                    }
                }
            }
            var tutores = dbS.Docente.Where(x => (x.Persona.Nombre + " " + x.Persona.ApellidoPaterno + " " + x.Persona.ApellidoMaterno).ToUpper().Contains(palabas.ToUpper())).ToList();
            if (tutores.Count() > 0)
            {
                foreach (var item in tutores)
                {
                    var proyectosPorTutores = db.TrabajoAcademico.FirstOrDefault(x => x.idTutor == item.Id && x.idEstado == 1);
                    if (proyectosPorTutores != null)
                    {
                        proyectosEncontrados.Add(proyectosPorTutores);
                    }
                }
            }
            string codigoHtml = "";
            if (proyectosEncontrados.Count() > 0)
            {
                DetalleDocumentoController detalle = new DetalleDocumentoController();
                List<TrabajoAcademico> proyectosFiltrados = proyectosEncontrados.Distinct().ToList();
                codigoHtml = detalle.TrabajosAcademicos(proyectosFiltrados);
            }
            else
            {
                codigoHtml = "<p>No se encontraron proyectos que coincidan con ese título, descripción, autor o tutor.</p>";
            }
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }
    }
}