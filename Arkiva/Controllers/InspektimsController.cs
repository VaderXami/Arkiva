using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Arkiva.Models;

namespace Arkiva.Controllers
{
    [HandleError]
    public class InspektimsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Inspektims
        public ActionResult Index(int SubjektId, string search)
        {
            var subjektList = new List<string>();
            var subjektQy = from d in db.Subjekt orderby d.Id select d.Id.ToString();
            var inspektim = from m in db.Inspektim select m;

            subjektList.AddRange(subjektQy.Distinct());
            ViewBag.SubjektId = new SelectList(subjektList.Where(s => s.Contains(SubjektId.ToString())));

            if (!String.IsNullOrEmpty(SubjektId.ToString()))
            {
                inspektim = inspektim.Where(s => s.SubjektId == SubjektId);
            }
            if (!String.IsNullOrEmpty(search))
            {
                inspektim = inspektim.Where(x => x.Emri.Contains(search));
                if (!inspektim.Any())
                {
                    ViewBag.Message = "Nuk u gjend asnje Inspektim!";
                }
            }
            return View(inspektim.ToList());
        }

        // GET: Inspektims/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspektim inspektim = db.Inspektim.Find(id);
            if (inspektim == null)
            {
                return HttpNotFound();
            }
            return View(inspektim);
        }
        public string GetNewName(string filename)
        {
            string ext = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);

            string newFileName = name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
            return newFileName;
        }

        public ActionResult DownloadZipFile(int id)
        {
            var dokument = db.Dokument.Where(i => i.InspektimId == id).ToList();

            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    foreach (var doc in dokument)
                    {
                        var file = archive.CreateEntry(doc.Inspektim.Subjekt.Emri +"/"+ doc.Inspektim.Emri + "/" + GetNewName(doc.FileName.ToString()));
                        using (var stream = file.Open())
                        {
                            stream.Write(doc.FileContent, 0, doc.FileContent.Length);
                        }
                    }
                }
                var inspektim = db.Inspektim.Find(id);
                return File(memoryStream.ToArray(), "application/zip", inspektim.Emri + ".zip");
            }
        }


        // GET: Inspektims/Create
        public ActionResult Create(int SubjektId)
        {
            ViewBag.SId = SubjektId;
            ViewBag.SubjektId = new SelectList(db.Subjekt, "Id", "Emri");
            return View();
        }

        // POST: Inspektims/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Emri,Data,SubjektId")] Inspektim inspektim)
        {
            if (ModelState.IsValid)
            {
                db.Inspektim.Add(inspektim);
                db.SaveChanges();
                return RedirectToAction("Index", "Inspektims", new { inspektim.SubjektId });
            }

            ViewBag.SubjektId = new SelectList(db.Subjekt, "Id", "Emri", inspektim.SubjektId);
            return View(inspektim);
        }

        // GET: Inspektims/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspektim inspektim = db.Inspektim.Find(id);
            if (inspektim == null)
            {
                return HttpNotFound();
            }
            ViewBag.SubjektId = new SelectList(db.Subjekt, "Id", "Emri", inspektim.SubjektId);
            return View(inspektim);
        }

        // POST: Inspektims/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Emri,Data,SubjektId")] Inspektim inspektim)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inspektim).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Inspektims", new { inspektim.SubjektId });
            }
            ViewBag.SubjektId = new SelectList(db.Subjekt, "Id", "Emri", inspektim.SubjektId);
            return View(inspektim);
        }

        // GET: Inspektims/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Inspektim inspektim = db.Inspektim.Find(id);
            if (inspektim == null)
            {
                return HttpNotFound();
            }
            return View(inspektim);
        }

        // POST: Inspektims/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Inspektim inspektim = db.Inspektim.Find(id);
            db.Inspektim.Remove(inspektim);
            db.SaveChanges();
            return RedirectToAction("Index", "Inspektims", new { inspektim.SubjektId });
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
