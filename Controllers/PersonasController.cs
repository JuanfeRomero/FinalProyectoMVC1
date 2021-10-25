using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                string registro = r.ToString().Substring(r.ToString().LastIndexOf(category)+category.Length);
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
            return View();
        }

        // POST: Personas/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "username,lastname,email,age,id_tipo_doc,nro_doc,gender,id")] Persona personas)
        {
            if (db.Personas.Find(personas.id) != null)
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
            return View(persona);
        }

        // POST: Personas/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "username,lastname,email,age, id_tipo_doc, nro_doc,gender, id")] Persona persona, int id)
        {
            if (ModelState.IsValid)
            {
                if (db.Personas.Find(id) != null)
                {
                    Persona registroABorrar = db.Personas.Find(id);
                    db.Personas.Remove(registroABorrar);
                    db.SaveChanges();
                    db.Personas.Add(persona);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                else
                {
                    db.Entry(persona).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
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
            db.Database.ExecuteSqlCommand("TRUNCATE TABLE RegistroUsuariosCodes");
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
