using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;

namespace Apartments.Controllers
{
    public class ApartmentsController : Controller
    {
        ~ApartmentsController()
        {
            if (db != null)
            {
                db.Dispose();
            }
        }
        private readonly ModelContainer db = new ModelContainer();
        // GET: Apartments
        public ActionResult Index()
        {
            return View(db.Apartments);
        }

        // GET: Apartments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments
                .Include(a => a.UploadedFiles)
                .SingleOrDefault(a => a.IDApartment == id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // GET: Apartments/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Apartments/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "Address, City, Contact")] Apartment apartment, 
            IEnumerable<HttpPostedFileBase> files)
        {
            if (ModelState.IsValid)
            {
                apartment.UploadedFiles = new List<UploadedFile>();
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var picture = new UploadedFile
                        {
                            Name = System.IO.Path.GetFileName(file.FileName),
                            ContentType = file.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            picture.Content = reader.ReadBytes(file.ContentLength);
                        }
                        apartment.UploadedFiles.Add(picture);
                    }
                }

                db.Apartments.Add(apartment);
                db.SaveChanges();
            }

            return RedirectToAction("Index");

        }

        // GET: Apartments/Edit/5
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments
                .Include(a => a.UploadedFiles)
                .SingleOrDefault(a => a.IDApartment == id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Edit/5
        [HttpPost]
        [ActionName("Edit")]
        public ActionResult EditConfirmed(int id, IEnumerable<HttpPostedFileBase> files)
        {
            Apartment apartmentToUpdate = db.Apartments.Find(id);
            if (TryUpdateModel(apartmentToUpdate, "", new string[] { "Address", "City", "Contact" }))
            {

                if (apartmentToUpdate.UploadedFiles == null)
                {
                    apartmentToUpdate.UploadedFiles = new List<UploadedFile>(); 
                }
                foreach (var file in files)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var picture = new UploadedFile
                        {
                            Name = System.IO.Path.GetFileName(file.FileName),
                            ContentType = file.ContentType
                        };
                        using (var reader = new System.IO.BinaryReader(file.InputStream))
                        {
                            picture.Content = reader.ReadBytes(file.ContentLength);
                        }
                        apartmentToUpdate.UploadedFiles.Add(picture);
                    }
                }




                db.Entry(apartmentToUpdate).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(apartmentToUpdate);
        }

        // GET: Apartments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            Apartment apartment = db.Apartments
                .Include(a => a.UploadedFiles)
                .SingleOrDefault(a => a.IDApartment == id);
            if (apartment == null)
            {
                return HttpNotFound();
            }
            return View(apartment);
        }

        // POST: Apartments/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, FormCollection collection)
        {
            db.UploadedFiles.RemoveRange(db.UploadedFiles.Where(f => f.ApartmentIDApartment == id));
            db.Apartments.Remove(db.Apartments.Find(id));
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
