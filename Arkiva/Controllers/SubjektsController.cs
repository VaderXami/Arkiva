using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Arkiva.Models;

namespace Arkiva.Controllers
{
    public class SubjektsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Subjekts
        public ActionResult Index(string search)
        {
            if (!String.IsNullOrEmpty(search))
            {
                var subjekt = db.Subjekt.Where(s => s.Emri.Contains(search));
                if (subjekt.Any())
                {
                    ViewBag.Message = "";
                    return View(subjekt);
                }
                else
                {
                    ViewBag.Message = "Subjekti nuk u gjend!";

                    return View(subjekt);
                }
            }
            else
            {
                return View(db.Subjekt.ToList());
            }
        }

        // GET: Subjekts/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjekt subjekt = db.Subjekt.Find(id);
            if (subjekt == null)
            {
                return HttpNotFound();
            }
            return View(subjekt);
        }

        // GET: Subjekts/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Subjekts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Emri,Data")] Subjekt subjekt)
        {
            if (ModelState.IsValid)
            {
                db.Subjekt.Add(subjekt);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subjekt);
        }

        public string GetNewName(string filename)
        {
            string ext = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);
                
            string newFileName = name +  "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
            return newFileName;
        }

        public ActionResult DownloadZipFile(int id)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var inspektime = db.Subjekt.Find(id).Inspektime;
                    foreach (var inspektim in inspektime )
                    {
                        var dokumente = db.Inspektim.Find(inspektim.Id).Dokumente;
                        foreach (var dokument in dokumente)
                        {
                            var file = archive.CreateEntry(inspektim.Emri + "/" + GetNewName(dokument.FileName.ToString()));
                            using (var stream = file.Open())
                            {
                                stream.Write(dokument.FileContent, 0, dokument.FileContent.Length);
                            }
                        }
                    }
                }
                var subjekt = db.Subjekt.Find(id);
                return File(memoryStream.ToArray(), "application/zip", subjekt.Emri + ".zip");
            }
        }

        // GET: Subjekts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjekt subjekt = db.Subjekt.Find(id);
            if (subjekt == null)
            {
                return HttpNotFound();
            }
            return View(subjekt);
        }

        // POST: Subjekts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Emri,Data")] Subjekt subjekt)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subjekt).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subjekt);
        }

        // GET: Subjekts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Subjekt subjekt = db.Subjekt.Find(id);
            if (subjekt == null)
            {
                return HttpNotFound();
            }
            return View(subjekt);
        }

        // POST: Subjekts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Subjekt subjekt = db.Subjekt.Find(id);
            db.Subjekt.Remove(subjekt);
            db.SaveChanges();
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
