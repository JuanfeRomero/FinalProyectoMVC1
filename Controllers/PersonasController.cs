using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mime;
using System.Web;
using System.Web.Mvc;
using RegistroPersonas;

namespace RegistroPersonas.Controllers
{
    public class PersonasController : Controller
    {
        private UsuariosCodesEntities db = new UsuariosCodesEntities();

        // GET: Personas
        public ActionResult Index()
        {
            return View(db.Personas.ToList());
        }

        [HttpPost]
        public ActionResult Index(string search, string category)
        {
            var result = db.Personas.AsEnumerable().Where(r =>
            {
                string registro = r.ToString().Substring(r.ToString().IndexOf(category) + category.Length);
                registro = category == "genero" ? registro : registro.Substring(0, registro.IndexOf(","));
                return registro.Contains(search.ToLower());
            });

            return View(result.ToList());
        }


        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // GET: Personas/Create
        public ActionResult Create()
        {
            List<SelectListItem> tipos = new List<SelectListItem>();
            db.TiposDocumento.ToList().ForEach(t => tipos.Add(new SelectListItem { Text = $"{t.nombre}", Value = $"{t.id_tipo_doc}" }));
            ViewData["tipos"] = tipos;
            int nuevoId = 0;
            do
            {
                nuevoId = new Random().Next();
            } while (db.Personas.Find(nuevoId) != null);

            ViewData["randomId"] = nuevoId;
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "age, email, gender, id, id_tipo_doc, lastname, nro_doc, username")] Persona personas)
        {
            personas.TipoDocumento = db.TiposDocumento.Find(personas.id_tipo_doc);

            if (db.Personas.Count(p => p.nro_doc == personas.nro_doc) > 0)
            {
                ModelState.Clear();
                ViewData["Message"] =
                    $"<p class=\"alert alert-warning text-center\" role=\"alert\">El DNI ingresado ya fue ingresado en la base de datos!</p>";
                return View();

            }
            else if (ModelState.IsValid)
            {
                db.Personas.Add(personas);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(personas);
        }

        // GET: Personas/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }

            List<SelectListItem> tipos = new List<SelectListItem>();
            db.TiposDocumento.ToList().ForEach(t => tipos.Add(new SelectListItem { Text = $"{t.nombre}", Value = $"{t.id_tipo_doc}" }));
            ViewData["tipos"] = tipos;
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,lastname,email,age, id_tipo_doc, nro_doc,gender, id")] Persona persona)
        {
            if (ModelState.IsValid)
            {

                db.Entry(persona).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            return View(persona);
        }

        // GET: Personas/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Persona persona = db.Personas.Find(id);
            if (persona == null)
            {
                return HttpNotFound();
            }
            return View(persona);
        }

        // POST: Personas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Persona persona = db.Personas.Find(id);
            db.Personas.Remove(persona);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpDelete]
        public ActionResult DeleteAll()
        {
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE Persona");
            db.SaveChanges();
            ViewData["Message"] =
                $"<p class=\"alert alert-warning text-center\" role=\"alert\">Elementos Borrados exitosamente!</p>";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
