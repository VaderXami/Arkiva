using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Arkiva.Models;
using System.IO;
using System;
using System.Collections.Generic;

namespace Arkiva.Controllers
{
    public class DokumentsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Dokuments
        public ActionResult Index(int InspektimId, string search)
        {

            var inspektimList = new List<string>();
            var inspektimQy = from d in db.Inspektim orderby d.Id select d.Id.ToString();
            var dokument = from m in db.Dokument select m;

            inspektimList.AddRange(inspektimQy.Distinct());
            ViewBag.SubjektId = new SelectList(inspektimList.Where(s => s.Contains(InspektimId.ToString())));

            if (!String.IsNullOrEmpty(InspektimId.ToString()))
            {
                dokument = dokument.Where(s => s.InspektimId == InspektimId);
            }
            if (!String.IsNullOrEmpty(search))
            {
                dokument = dokument.Where(x => x.FileName.Contains(search));
            }
            if (!dokument.Any())
            {
                ViewBag.Message = "Nuk u gjend asnje Dokument!";
            }
            int nb = 0;
            foreach (var item in dokument)
            {
                nb += 1;
            }
            if (nb > 0)
            {
                ViewBag.No = "Nr total i dokumenteve: " + nb;
            }
            return View(dokument.ToList());
        }
        /*
        // GET: Dokuments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dokument dokument = db.Dokument.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            return View(dokument);
        }*/

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            Dokument dokument = db.Dokument.Find(id);
            string ext = Path.GetExtension(dokument.FileName).ToLower();

            return File(dokument.FileContent, "application/" + ext, dokument.FileName);

        }

        [HttpGet]
        public ActionResult PreviewFilePDF(int id)
        {
            var dokument = db.Dokument.Find(id);
            return File(dokument.FileContent, "application/pdf");
        }


        [HttpGet]
        public ActionResult PreviewFileJPG(int id)
        {
            var dokument = db.Dokument.Find(id);
            return File(dokument.FileContent, "image/jpg");
        }

        [HttpGet]
        public ActionResult PreviewFileJEPG(int id)
        {
            var dokument = db.Dokument.Find(id);
            return File(dokument.FileContent, "image/jpeg");
        }

        [HttpGet]
        public ActionResult PreviewFilePNG(int id)
        {
            var dokument = db.Dokument.Find(id);
            return File(dokument.FileContent, "image/png");
        }
        // GET: Dokuments/Create
        public ActionResult Create(string InspektimId)
        {
            var inspektimList = new List<string>();
            var inspektimQy = from d in db.Inspektim orderby d.Id select d.Id.ToString();
            var dokument = from m in db.Dokument select m;
            
            inspektimList.AddRange(inspektimQy.Distinct());
            ViewBag.InspektimId = new SelectList(inspektimList.Where(s => s.Contains(InspektimId)));
            return View();
        }

        // POST: Dokuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Data,FileName,FileContent,InspektimId")] Dokument dokument, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase Files in files)
                {
                    string FileExt = Path.GetExtension(Files.FileName).ToUpper();

                    if (FileExt == ".PDF" || FileExt == ".DOC" || FileExt == ".DOCX" || FileExt == ".PNG" || FileExt == ".JPG" || FileExt == ".JPEG")
                    {
                        Stream str = Files.InputStream;
                        BinaryReader Br = new BinaryReader(str);
                        Byte[] FileDet = Br.ReadBytes((Int32)str.Length);

                        dokument.FileName = Files.FileName;
                        dokument.FileContent = FileDet;
                        db.Dokument.Add(dokument);
                        db.SaveChanges();
                        return RedirectToAction("Index", "Dokuments", new { dokument.InspektimId });
                    }
                    else
                    {

                        ViewBag.FileStatus = "Invalid file format.";
                        return View();

                    }
                }
            }

            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", dokument.InspektimId);
            return View(dokument);
        }

       /* // GET: Dokuments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dokument dokument = db.Dokument.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", dokument.InspektimId);
            return View(dokument);
        }

        // POST: Dokuments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Data,InspektimId")] Dokument dokument)
        {
            if (ModelState.IsValid)
            {
                db.Entry(dokument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Dokuments", new { dokument.InspektimId });
            }
            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", dokument.InspektimId);
            return View(dokument);
        }*/

        // GET: Dokuments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Dokument dokument = db.Dokument.Find(id);
            if (dokument == null)
            {
                return HttpNotFound();
            }
            return View(dokument);
        }

        // POST: Dokuments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Dokument dokument = db.Dokument.Find(id);
            db.Dokument.Remove(dokument);
            db.SaveChanges();
            return RedirectToAction("Index", "Dokuments", new { dokument.InspektimId });
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
