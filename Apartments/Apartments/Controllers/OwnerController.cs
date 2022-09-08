using Antlr.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Apartments.Controllers
{
    public class OwnerController : Controller
    {
        ~OwnerController()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
        private readonly ModelContainer db = new ModelContainer();

        // GET: Owner
        public ActionResult OwnerIndex()
        {
            return View(db.Owners);
        }

        // GET: Owner/Details/5
        public ActionResult DetailsOwner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.SingleOrDefault(a => a.IDOwner == id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // GET: Owner/Create
        public ActionResult CreateOwner()
        {
            return View();
        }

        // POST: Owner/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName, LastName, ApartmentIDApartment")] Owner owner)
        {

            if (ModelState.IsValid)
            {
                db.Owners.Add(owner);
                db.SaveChanges();
            }
            return RedirectToAction("OwnerIndex");
        }

        // GET: Owner/Edit/5
        public ActionResult EditOwner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners
                .SingleOrDefault(a => a.IDOwner == id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owner/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed()
        {
            Owner apartmentToUpdate = db.Owners.FirstOrDefault();
            if (TryUpdateModel(apartmentToUpdate, "",
                new string[] { "FirstName", "LastName", "ApartmentIDApartment" }))
            {
                db.Entry(apartmentToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("OwnerIndex");
            }
            return View(apartmentToUpdate);
        }

        // GET: Owner/Delete/5
        public ActionResult DeleteOwner(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Owner owner = db.Owners.SingleOrDefault(a => a.IDOwner == id);
            if (owner == null)
            {
                return HttpNotFound();
            }
            return View(owner);
        }

        // POST: Owner/Delete/5
        [HttpPost]
        [ActionName("DeleteOwner")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            db.Owners.Remove(db.Owners.Find(id));
            db.SaveChanges();
            return RedirectToAction("OwnerIndex");
        }
    }
}
