using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class TrabajoAcademicoController : Controller
    {
        // GET: TrabajoAcademico
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Actualizar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            var lista = db.TrabajoAcademico.OrderByDescending(x => x.fechaRegistro).ToList();
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
                var p = dbS.Persona.Single(x => x.Id == item.idEstudiante);
                object[] obj = { i, item.fechaRegistro.ToShortDateString(), p.Nombre + " " + p.ApellidoPaterno + " " + p.ApellidoMaterno, item.TipoTrabajoAcademico.nombre, item.titulo, estado, btnEstado };
                tabla.Add(obj);
                i++;
            }
            return Json(tabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(TrabajoAcademico trabajoAcademico)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            SAADSTJEntities dbS = new SAADSTJEntities();
            Status s = new Status();
            try
            {
                if (trabajoAcademico.id == 0)
                {
                    var existeTrabajoAcademico = db.TrabajoAcademico.SingleOrDefault(x => x.titulo == trabajoAcademico.titulo && x.idEstudiante == trabajoAcademico.idEstudiante);
                    if (existeTrabajoAcademico == null)
                    {
                        trabajoAcademico.idEstado = 1;
                        trabajoAcademico.fechaRegistro = DateTime.Now;
                        db.TrabajoAcademico.Add(trabajoAcademico);
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Trabajo académico registrado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El trabajo académico ya existe.";
                    }
                }
                else
                {
                    var existeTrabajoAcademico = db.TrabajoAcademico.SingleOrDefault(x => x.titulo == trabajoAcademico.titulo && x.idEstudiante == trabajoAcademico.idEstudiante && x.id != trabajoAcademico.id);
                    if (existeTrabajoAcademico == null)
                    {
                        var trabajo = db.TrabajoAcademico.Single(x => x.id == trabajoAcademico.id);
                        trabajo.añoPublicacion = trabajoAcademico.añoPublicacion;
                        trabajo.titulo = trabajoAcademico.titulo;
                        trabajo.descripcion = trabajoAcademico.descripcion;
                        trabajo.documento = trabajoAcademico.documento;
                        trabajo.idEstudiante = trabajoAcademico.idEstudiante;
                        trabajo.idCarrera = trabajoAcademico.idCarrera;
                        trabajo.idTutor = trabajoAcademico.idTutor;
                        trabajo.idTipoTrabajoAcademico = trabajoAcademico.idTipoTrabajoAcademico;
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Trabajo académico modificado";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El trabajo académico ya existe.";
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
                    string filePath = Path.Combine(Server.MapPath("~/Documentos/TrabajosAcademicos/"), fechaHora + " - " + fileName);
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

        public ActionResult BuscarEstudiante(string ci)
        {
            SAADSTJEntities db = new SAADSTJEntities();
            List<object> lista = new List<object>();
            var estudiantes = db.Persona.SqlQuery("select top 5 p.* from Persona p, Estudiante e where p.Id = e.Id and p.DocumentoIdentidad like '%" + ci + "%'").ToList();
            foreach (var item in estudiantes)
            {
                string nombre = item.Nombre + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno;
                object o = new { id = item.Id, nombreCompleto = nombre };
                lista.Add(o);
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeleccionarEstudiante(int idPersona)
        {
            SAADSTJEntities db = new SAADSTJEntities();
            var p = db.Persona.Single(x => x.Id == idPersona);
            object o = new
            {
                nombre = p.Nombre,
                aPaterno = p.ApellidoPaterno,
                aMaterno = p.ApellidoMaterno,
                ci = p.DocumentoIdentidad,
                celular = p.Celular,
                correo = p.Estudiante.EmailOffice365
            };
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CarrerasEstudiante(int idPersona)
        {
            SAADSTJEntities db = new SAADSTJEntities();
            var p = db.InscripcionCarrera.Where(x => x.Estudiante.Id == idPersona);
            List<object> Carreras = new List<object>();
            foreach (var item in p)
            {
                object o = new
                {
                    id = item.PlanEstudio.Id,
                    nombre = item.PlanEstudio.Nombre
                };
                Carreras.Add(o);
            }
            return Json(Carreras, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BuscarTutor(string ci)
        {
            SAADSTJEntities db = new SAADSTJEntities();
            List<object> lista = new List<object>();
            var docentes = db.Persona.SqlQuery("select top 3 p.* from Persona p, Docente d where p.Id = d.Id and p.DocumentoIdentidad like '%" + ci + "%'").ToList();
            foreach (var item in docentes)
            {
                string nombre = item.Nombre + " " + item.ApellidoPaterno + " " + item.ApellidoMaterno;
                object o = new { id = item.Id, nombreCompleto = nombre };
                lista.Add(o);
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult SeleccionarTutor(int idTutor)
        {
            SAADSTJEntities db = new SAADSTJEntities();
            var tutor = db.Persona.Single(x => x.Id == idTutor);
            string nombre = tutor.Nombre + " " + tutor.ApellidoPaterno + " " + tutor.ApellidoMaterno;
            object o = new { id = tutor.Id, nombreCompleto = nombre };
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deshabilitar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                var c = db.TrabajoAcademico.SingleOrDefault(x => x.id == id);
                c.idEstado = 2;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Trabajo académico deshabilitado.";
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
                var c = db.TrabajoAcademico.SingleOrDefault(x => x.id == id);
                c.idEstado = 1;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Trabajo académico habilitado.";
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