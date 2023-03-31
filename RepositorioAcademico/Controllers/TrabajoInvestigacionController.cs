using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class TrabajoInvestigacionController : Controller
    {
        // GET: TrabajoInvestigacion
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Actualizar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            var lista = db.TrabajoInvestigacion.OrderByDescending(x => x.fechaRegistro).ToList();
            List<object[]> tabla = new List<object[]>();
            int i = 1;
            string estado = "", btnEstado = "";
            foreach (var item in lista)
            {
                if (item.idEstado == 2)
                {
                    estado = "Deshabilitado";
                    btnEstado = "<button  type='button'class='btn btn-sm btn-link' onclick='Habilitar(" + item.id + ")'><i class='fas fa-check'></i></button></div>";
                }
                else
                {
                    estado = "Habilitado";
                    btnEstado = "<button  type='button'class='btn btn-sm btn-link' onclick='Deshabilitar(" + item.id + ")'><i class='fas fa-times'></i></button></div>";
                }
                object[] obj = { i, item.fechaRegistro.ToShortDateString(), item.CentroInvestigacion.nombre, item.titulo, estado, btnEstado };
                tabla.Add(obj);
                i++;
            }
            return Json(tabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(TrabajoInvestigacion trabajoInvestigacion)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            Status s = new Status();
            try
            {
                if (trabajoInvestigacion.id == 0)
                {
                    var existeTrabajoInvestigacion = db.TrabajoInvestigacion.SingleOrDefault(x => x.titulo == trabajoInvestigacion.titulo && x.idCentroInvestigacion == trabajoInvestigacion.idCentroInvestigacion);
                    if (existeTrabajoInvestigacion == null)
                    {
                        trabajoInvestigacion.idEstado = 1;
                        trabajoInvestigacion.fechaRegistro = DateTime.Now;
                        db.TrabajoInvestigacion.Add(trabajoInvestigacion);
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Trabajo de investigación registrado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El trabajo de investigación ya existe.";
                    }
                }
                else
                {
                    var existeTrabajoInvestigacion = db.TrabajoInvestigacion.SingleOrDefault(x => x.titulo == trabajoInvestigacion.titulo && x.idCentroInvestigacion == trabajoInvestigacion.idCentroInvestigacion && x.id != trabajoInvestigacion.id);
                    if (existeTrabajoInvestigacion == null)
                    {
                        var trabajo = db.TrabajoInvestigacion.Single(x => x.id == trabajoInvestigacion.id);
                        trabajo.añoPublicacion = trabajoInvestigacion.añoPublicacion;
                        trabajo.titulo = trabajoInvestigacion.titulo;
                        trabajo.descripcion = trabajoInvestigacion.descripcion;
                        trabajo.documento = trabajoInvestigacion.documento;
                        trabajo.idCentroInvestigacion = trabajoInvestigacion.idCentroInvestigacion;
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Trabajo de investigación modificado";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El trabajo de investigación ya existe.";
                    }
                }
            }
            catch (Exception)
            {
                s.Tipo = 3;
                s.Mensaje = "Se produjo un error comuníquese con el administrador.";
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult GuardarArchivo(HttpPostedFileBase archivo)
        {
            Status s = new Status();
            try
            {
                if (archivo != null && archivo.ContentLength > 0)
                {
                    string fechaHora = DateTime.Now.ToString("dd-MM-yyy hh-mm-ss");
                    string fileName = Path.GetFileName(archivo.FileName);
                    string filePath = Path.Combine(Server.MapPath("~/Documentos/TrabajosInvestigacion/"), fechaHora + " - " + fileName);
                    archivo.SaveAs(filePath);
                    s.Tipo = 1;
                    s.Mensaje = filePath;
                }
                else
                {
                    s.Tipo = 2;
                    s.Mensaje = "No se ha seleccionado ningún archivo.";
                }
            }
            catch (Exception)
            {
                s.Tipo = 3;
                s.Mensaje = "Se produjo un error al cargar el archivo, comuníquese con el administrador.";
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deshabilitar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                var c = db.TrabajoInvestigacion.SingleOrDefault(x => x.id == id);
                c.idEstado = 2;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Trabajo de investigación deshabilitado.";
            }
            catch
            {
                s.Tipo = 3;
                s.Mensaje = "Se produjo un error comuníquese con el administrador.";
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Habilitar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                var c = db.TrabajoInvestigacion.SingleOrDefault(x => x.id == id);
                c.idEstado = 1;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Trabajo de investigación habilitado.";
            }
            catch
            {
                s.Tipo = 3;
                s.Mensaje = "Se produjo un error comuníquese con el administrador.";
            }
            return Json(s, JsonRequestBehavior.AllowGet);
        }
    }
}