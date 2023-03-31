using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class DetalleDocumentoController : Controller
    {
        // GET: DetalleDocumento
        public ActionResult Index(int tipoDocumento, int idDocumento)
        {
            ViewBag.tipoDocumento = tipoDocumento;
            ViewBag.idDocumento = idDocumento;
            return View();
        }

        // variable url para enviar datos o refireccionar para produccion y desarrollo
        //Desarrollo
        //string url = "https://localhost:44347/";
        //Produccion
        string url = "https://tarija.upds.edu.bo/RepositorioAcademico/";

        public string TrabajosAcademicos(List<TrabajoAcademico> trabajos)
        {
            SAADSTJEntities dbS = new SAADSTJEntities();
            string codigoHtml = "";
            foreach (var item in trabajos)
            {
                var persona = dbS.Persona.Single(x => x.Id == item.idEstudiante);
                string autor = persona.Nombre + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
                string descripcionRecortada = item.descripcion;
                if (item.descripcion.Length > 220)
                {
                    descripcionRecortada = item.descripcion.Substring(0, 220) + "...";
                }
                codigoHtml += "<div class='card mb-3' style='max-width: 100%;'>" +
                    "<div class='row g-0'>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-2 col-xl-2 col-xxl-2 contenedor-portada'>" +
                            "<img src='" + url + "Content/img/portadaProyectos.jpg' alt='portada' style='width: 100%;' />" +
                        "</div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-10 col-xl-10 col-xxl-10'>" +
                            "<div class='franja'></div>" +
                            "<div class='card-body'>" +
                                "<a href='" + url + "DetalleDocumento?tipoDocumento=1&idDocumento=" + item.id + "' class='card-title' style='font-size: 17px;'>" + item.titulo.ToUpper() + "</a>" +
                                "<p class='card-text' style='text-align:justify'>" +
                                    "<small class='text-muted'>" + autor + "</small><br/>" +
                                    "<small>" + descripcionRecortada + "</small>" +
                                "</p>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
            }
            return codigoHtml;
        }

        public string TrabajosInvestigacion(List<TrabajoInvestigacion> trabajos)
        {
            string codigoHtml = "";
            foreach (var item in trabajos)
            {
                string descripcionRecortada = item.descripcion;
                if (item.descripcion.Length > 220)
                {
                    descripcionRecortada = item.descripcion.Substring(0, 220) + "...";
                }
                codigoHtml += "<div class='card mb-3' style='max-width: 100%;'>" +
                    "<div class='row g-0'>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-2 col-xl-2 col-xxl-2 contenedor-portada'>" +
                            "<img src='" + url + "Content/img/portadaProyectos.jpg' alt='portada' style='width: 100%;' />" +
                        "</div>" +
                        "<div class='col-12 col-sm-12 col-md-12 col-lg-10 col-xl-10 col-xxl-10'>" +
                            "<div class='franja'></div>" +
                            "<div class='card-body'>" +
                                "<a href='" + url + "DetalleDocumento?tipoDocumento=2&idDocumento=" + item.id + "' class='card-title' style='font-size: 17px;'>" + item.titulo.ToUpper() + "</a>" +
                                "<p class='card-text' style='text-align:justify'>" +
                                    "<small class='text-muted'>" + item.CentroInvestigacion.nombre + "</small><br/>" +
                                    "<small>" + descripcionRecortada + "</small>" +
                                "</p>" +
                            "</div>" +
                        "</div>" +
                    "</div>" +
                "</div>";
            }
            return codigoHtml;
        }

        public ActionResult DetalleTrabajoAcademico(int idDocumento)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            var documento = db.TrabajoAcademico.Single(x => x.id == idDocumento);
            string nombreArchivo = Path.GetFileName(documento.documento);
            var persona = dbS.Estudiante.Single(x => x.Id == documento.idEstudiante).Persona;
            string estudiante = persona.Nombre + " " + persona.ApellidoPaterno + " " + persona.ApellidoMaterno;
            string carrera = dbS.PlanEstudio.Single(x => x.Id == documento.idCarrera).Nombre;
            string docente = "";
            if (documento.idTutor > 0)
            {
                var personaDocente = dbS.Docente.Single(x => x.Id == documento.idTutor).Persona;
                docente = "<p><span class='fw-bold'>Tutor:</span> " + personaDocente.Nombre + " " + personaDocente.ApellidoPaterno + " " + personaDocente.ApellidoMaterno + "</p>";
            }
            string titulo = "<div class='col-12 mt-2'>"+
                          "<h5 class='text-center'>" + documento.titulo.ToUpper() + "</h5>"+
                          "<hr/>"+
                       "</div>";
            string informacion = "<div class='col-12 col-sm-12 col-md-3 col-lg-3 col-xl-3 col-xxl-3 mt-2 img-docuento-detalle'>" +
                                    "<img src='" + url + "Content/img/portadaProyectos.jpg' class='shadow-sm rounded'/>" +
                                 "</div>"+
                                 "<div class='col-12 col-sm-12 col-md-9 col-lg-9 col-xl-9 col-xxl-9 mt-2'>" +
                                    "<p><span class='fw-bold'>Tipo de proyecto académico:</span> " + documento.TipoTrabajoAcademico.nombre + "</p>"+
                                    "<p><span class='fw-bold'>Autor:</span> " + estudiante + "</p>" +
                                    "<p><span class='fw-bold'>Carrera:</span> " + carrera + "</p>" +
                                    docente +
                                    "<p><span class='fw-bold'>Año de publicación:</span> " + documento.añoPublicacion + "</p>" +
                                    "<div class='row'>" +
                                        "<div class='col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6 mt-1'>" +
                                            "<div class='d-grid gap-2'>" +
                                                "<button class='btn btn-primary' type='button' data-bs-toggle='modal' data-bs-target='#modalDocumento' onclick='MostrarDocumento(\"" + nombreArchivo + "\", 1)'><i class='fas fa-eye'></i> Visualizar Documento</button>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class='col-12 col-sm-12 col-md-6 col-lg-6 col-xl-6 col-xxl-6 mt-1'>" +
                                            "<div class='d-grid gap-2'>" +
                                                "<a class='btn btn-primary' href=''><i class='fas fa-download'></i> Descargar Documento</a>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>";
            string resumen = "<div class='col-12 mt-2'>" +
                                "<hr />" +
                                "<p style='text-align:justify'>" +
                                "<span class='fw-bold'>Resumen:</span><br />" +
                                documento.descripcion +
                                "</p>" +
                            "</div>";
            string codHTML = titulo + informacion + resumen;
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DetalleTrabajoInvestigacion(int idDocumento)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            var documento = db.TrabajoInvestigacion.Single(x => x.id == idDocumento);
            string nombreArchivo = Path.GetFileName(documento.documento);
            string titulo = "<div class='col-sm-12 mt-2'>" +
                          "<h5 class='text-center'>" + documento.titulo.ToUpper() + "</h5>" +
                          "<hr/>" +
                       "</div>";
            string informacion = "<div class='col-sm-3 mt-2'>" +
                                    "<img src='" + url + "Content/img/portadaProyectos.jpg' class='shadow-sm rounded' style='width:100%; height:240px' />" +
                                 "</div>" +
                                 "<div class='col-sm-9 mt-2'>" +
                                    "<p><span class='fw-bold'>Centro de investigación:</span> " + documento.CentroInvestigacion.nombre + "</p>" +
                                    "<p>" +
                                        "<span class='fw-bold'>Año de publicación:</span> " + documento.añoPublicacion +
                                     "</p>" +
                                     "<p style='text-align:justify'>" +
                                        "<span class='fw-bold'>Resumen:</span><br />" + documento.descripcion +
                                    "</p>" +
                                    "<div class='row'>" +
                                        "<div class='col-sm-6 mt-1'>" +
                                            "<div class='d-grid gap-2'>" +
                                                "<button class='btn btn-primary' type='button' data-bs-toggle='modal' data-bs-target='#modalDocumento' onclick='MostrarDocumento(\"" + nombreArchivo + "\", 2)'><i class='fas fa-eye'></i> Visualizar Documento</button>" +
                                            "</div>" +
                                        "</div>" +
                                        "<div class='col-sm-6 mt-1'>" +
                                            "<div class='d-grid gap-2'>" +
                                                "<a class='btn btn-primary' href=''><i class='fas fa-download'></i> Descargar Documento</a>" +
                                            "</div>" +
                                        "</div>" +
                                    "</div>" +
                                "</div>";
            string codHTML = titulo + informacion;
            return Json(codHTML, JsonRequestBehavior.AllowGet);
        }

        public ActionResult MostrarDocumento(string nombreArchivo, int tipo)
        {
            string rutaArchivo = "";
            if (tipo == 1)
            {
                rutaArchivo = Server.MapPath("~/Documentos/TrabajosAcademicos/" + nombreArchivo);
            }
            else
            {
                rutaArchivo = Server.MapPath("~/Documentos/TrabajosInvestigacion/" + nombreArchivo);
            }
            // Crea un objeto FileStream para leer el archivo PDF
            FileStream archivoStream = new FileStream(rutaArchivo, FileMode.Open, FileAccess.Read);

            // Devuelve el archivo PDF
            return File(archivoStream, "application/pdf");
        }
    }
}