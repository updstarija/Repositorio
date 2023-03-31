using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class PorCentrosInvestigacionController : Controller
    {
        // GET: PorCentrosInvestigacion
        public ActionResult Index(int idCentroInvestigacion)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            string centroInvestigacion = db.CentroInvestigacion.Single(x => x.id == idCentroInvestigacion).nombre;
            ViewBag.centroInvestigacion = centroInvestigacion;
            ViewBag.idCentroInvestigacion = idCentroInvestigacion;
            return View();
        }

        public ActionResult Listar(int idCentroInvestigacion)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            List<TrabajoInvestigacion> lista = db.TrabajoInvestigacion.OrderBy(x => x.titulo).Where(x => x.idEstado == 1 && x.idCentroInvestigacion == idCentroInvestigacion).ToList();
            DetalleDocumentoController detalle = new DetalleDocumentoController();
            string codigoHtlm = detalle.TrabajosInvestigacion(lista);
            return Json(codigoHtlm, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarPorTitulo(string titulo, int idCentroInvestigacion)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            List<TrabajoInvestigacion> proyectosEncontrados = db.TrabajoInvestigacion.OrderBy(x => x.titulo).Where(x => x.idEstado == 1 && x.idCentroInvestigacion == idCentroInvestigacion && (x.titulo.ToUpper().Contains(titulo.ToUpper()) || x.descripcion.ToUpper().Contains(titulo.ToUpper()))).ToList();
            string codigoHtlm = "";
            if (proyectosEncontrados.Count() > 0)
            {
                DetalleDocumentoController detalle = new DetalleDocumentoController();
                codigoHtlm = detalle.TrabajosInvestigacion(proyectosEncontrados);
            }
            else
            {
                codigoHtlm = "<p>No se encontraron investigaciones que coincidan con ese título o resumen</p>";
            }
            return Json(codigoHtlm, JsonRequestBehavior.AllowGet);
        }
    }
}