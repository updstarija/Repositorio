using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorTrabajosAcademicosController : Controller
    {
        // GET: PorTrabajosAcademicos
        public ActionResult Index(int idTipoTrabajoAcademico)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            string tipoTrabajoAcademico = db.TipoTrabajoAcademico.Single(x => x.id == idTipoTrabajoAcademico).nombre;
            ViewBag.tipoTrabajoAcademico = tipoTrabajoAcademico;
            ViewBag.idTipoTrabajoAcademico = idTipoTrabajoAcademico;
            return View();
        }

        public ActionResult Listar(int idTipoTrabajoAcademico)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> lista = db.TrabajoAcademico.OrderBy(x => x.titulo).Where(x => x.idEstado == 1 && x.idTipoTrabajoAcademico == idTipoTrabajoAcademico).ToList();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            string codigoHtml = detalle.TrabajosAcademicos(lista);
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPorTitulo(string titulo, int idTipoTrabajoAcademico)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            List<TrabajoAcademico> proyectosEncontrados = db.TrabajoAcademico.OrderBy(x => x.titulo).Where(x => x.idEstado == 1 && x.idTipoTrabajoAcademico == idTipoTrabajoAcademico && (x.titulo.ToUpper().Contains(titulo.ToUpper()) || x.descripcion.ToUpper().Contains(titulo.ToUpper()))).ToList();
            string codigoHtml = "";
            if (proyectosEncontrados.Count() > 0)
            {
                DetalleDocumentoController detalle = new DetalleDocumentoController();
                codigoHtml = detalle.TrabajosAcademicos(proyectosEncontrados);
            }
            else
            {
                codigoHtml = "<p>No se encontraron proyectos que coincidan con ese título o resumen</p>";
            }
            return Json(codigoHtml, JsonRequestBehavior.AllowGet);
        }
    }
}