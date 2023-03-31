using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class TiposTrabajoAcademicoController : Controller
    {
        // GET: TiposTrabajoAcademico
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Actualizar()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            var btn = "";
            var btnEstado = "";
            string estado = "";
            var lista = db.TipoTrabajoAcademico.ToList();
            List<object[]> tabla = new List<object[]>();
            int i = 1;
            foreach (var item in lista)
            {
                if (item.estado == false)
                {
                    estado = "Deshabilitado";
                    btnEstado = "<button  type='button'class='btn btn-sm btn-link' onclick='Habilitar(" + item.id + ")'><i class='fas fa-check'></i></button></div>";
                }
                else
                {
                    estado = "Habilitado";
                    btnEstado = "<button  type='button'class='btn btn-sm btn-link' onclick='Deshabilitar(" + item.id + ")'><i class='fas fa-times'></i></button></div>";
                }
                btn = "<div style='float:left'><button  type='button'class='btn btn-sm btn-link' data-bs-toggle='modal' data-bs-target='#modalTipos' onclick='Editar(" + item.id + ")'><i class='fa fa-pen'></i></button>";
                btn += btnEstado;
                object[] obj = { i, item.nombre, estado, btn };
                tabla.Add(obj);
                i++;
            }
            return Json(tabla, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListarSelect()
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            List<object> lista = new List<object>();
            foreach (var item in db.TipoTrabajoAcademico.OrderBy(x => x.nombre).Where(x => x.estado == true))
            {
                object o = new { id = item.id, nombre = item.nombre };
                lista.Add(o);
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(TipoTrabajoAcademico tipoTrabajoAcademico)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                if (tipoTrabajoAcademico.id == 0)
                {
                    var existeTipoTrabajoAcademico = db.TipoTrabajoAcademico.SingleOrDefault(x => x.nombre == tipoTrabajoAcademico.nombre);
                    if (existeTipoTrabajoAcademico == null)
                    {
                        tipoTrabajoAcademico.estado = true;
                        db.TipoTrabajoAcademico.Add(tipoTrabajoAcademico);
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Tipo de trabajo académico registrado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El tipo de trabajo académico ya existe.";
                    }
                }
                else
                {
                    var existeTipoTrabajoAcademico = db.TipoTrabajoAcademico.SingleOrDefault(x => x.nombre == tipoTrabajoAcademico.nombre && x.id != tipoTrabajoAcademico.id);
                    if (existeTipoTrabajoAcademico == null)
                    {
                        var tipoTrabajo = db.TipoTrabajoAcademico.SingleOrDefault(x => x.id == tipoTrabajoAcademico.id);
                        tipoTrabajo.nombre = tipoTrabajoAcademico.nombre;
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Tipo de trabajo académico modificado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El tipo de trabajo académico ya existe.";
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

        public ActionResult Editar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            var c = db.TipoTrabajoAcademico.SingleOrDefault(x => x.id == id);
            object o = new { id = c.id, nombre = c.nombre };
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deshabilitar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                var c = db.TipoTrabajoAcademico.SingleOrDefault(x => x.id == id);
                c.estado = false;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Tipo de trabajo académico deshabilitado.";
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
                var c = db.TipoTrabajoAcademico.SingleOrDefault(x => x.id == id);
                c.estado = true;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Tipo de trabajo académico Habilitado.";
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