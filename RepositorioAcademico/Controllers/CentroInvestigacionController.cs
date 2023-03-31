using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using RepositorioAcademico.Models;

namespace RepositorioAcademico.Controllers
{
    public class CentroInvestigacionController : Controller
    {
        // GET: CentroInvestigacion
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
            var lista = db.CentroInvestigacion.ToList();
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
                btn = "<div style='float:left'><button  type='button'class='btn btn-sm btn-link' data-bs-toggle='modal' data-bs-target='#modalCentroInvestigacion' onclick='Editar(" + item.id + ")'><i class='fa fa-pen'></i></button>";
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
            foreach (var item in db.CentroInvestigacion.OrderBy(x => x.nombre).Where(x => x.estado == true))
            {
                object o = new { id = item.id, nombre = item.nombre };
                lista.Add(o);
            }
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Guardar(CentroInvestigacion centroInvestigacion)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                if (centroInvestigacion.id == 0)
                {
                    var existeCentroInvestigacion = db.CentroInvestigacion.SingleOrDefault(x => x.nombre == centroInvestigacion.nombre);
                    if (existeCentroInvestigacion == null)
                    {
                        centroInvestigacion.estado = true;
                        db.CentroInvestigacion.Add(centroInvestigacion);
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Centro de investigación registrado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "El centro de investigación ya existe.";
                    }
                }
                else
                {
                    var existeCentroInvestigacion = db.CentroInvestigacion.SingleOrDefault(x => x.nombre == centroInvestigacion.nombre && x.id != centroInvestigacion.id);
                    if (existeCentroInvestigacion == null)
                    {
                        var centro = db.CentroInvestigacion.SingleOrDefault(x => x.id == centroInvestigacion.id);
                        centro.nombre = centroInvestigacion.nombre;
                        db.SaveChanges();
                        s.Tipo = 1;
                        s.Mensaje = "Centro de investigación modificado.";
                    }
                    else
                    {
                        s.Tipo = 2;
                        s.Mensaje = "EL centro de investigación ya existe.";
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
            var c = db.CentroInvestigacion.SingleOrDefault(x => x.id == id);
            object o = new { id = c.id, nombre = c.nombre };
            return Json(o, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Deshabilitar(int id)
        {
            RepositorioAcademicoEntities db = new RepositorioAcademicoEntities();
            Status s = new Status();
            try
            {
                var c = db.CentroInvestigacion.SingleOrDefault(x => x.id == id);
                c.estado = false;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Centro de investigación deshabilitado.";
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
                var c = db.CentroInvestigacion.SingleOrDefault(x => x.id == id);
                c.estado = true;
                db.SaveChanges();
                s.Tipo = 1;
                s.Mensaje = "Centro de investigación habilitado.";
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