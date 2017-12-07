using Helper;
using Model;
using Portafolio.Areas.Admin.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace proyecto.Areas.Admin.Controllers
{
    [Autenticado]
    public class UsuarioController : Controller
    {
        private Usuario usuario = new Usuario();
        private TablaDato tabla = new TablaDato();
        // GET: Admin/Usuario
        public ActionResult Index()
        {
            ViewBag.Paises = tabla.Listar("pais");
            return View(usuario.Obtener(SessionHelper.GetUser()));
        }

        public JsonResult Guardar(Usuario model, HttpPostedFileBase Foto)
        {
            var rm = new ResponseModel();
            //Se retira validación del password
            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                rm = model.Guardar(Foto);
            }
            return Json(rm);
        }
    }
}