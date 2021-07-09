using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Arkiva.Models;

namespace Arkiva.Controllers
{
    public class LlojiDokumentitsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: LlojiDokumentits
        public ActionResult Index(int InspektimId, string search)
        {
            var inspektimList = new List<string>();
            var inspektimQy = from d in db.Inspektim orderby d.Id select d.Id.ToString();
            var lloji = from m in db.LlojiDokumentit select m;

            inspektimList.AddRange(inspektimQy.Distinct());
            ViewBag.InspektimId = new SelectList(inspektimList.Where(s => s.Contains(InspektimId.ToString())));

            if (!String.IsNullOrEmpty(InspektimId.ToString()))
            {
                lloji = lloji.Where(s => s.InspektimId == InspektimId);
            }
            if (!String.IsNullOrEmpty(search))
            {
                lloji = lloji.Where(x => x.Emri.Contains(search));
                if (!lloji.Any())
                {
                    ViewBag.Message = "Nuk u gjend asnje Lloj Dokumenti!";
                }
            }
            return View(lloji.ToList());
        }

        // GET: LlojiDokumentits/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LlojiDokumentit llojiDokumentit = db.LlojiDokumentit.Find(id);
            if (llojiDokumentit == null)
            {
                return HttpNotFound();
            }
            return View(llojiDokumentit);
        }

        // GET: LlojiDokumentits/Create
        public ActionResult Create(string InspektimId)
        {
            var inspektimList = new List<string>();
            var inspektimQy = from d in db.Inspektim orderby d.Id select d.Id.ToString();
            var lloji = from m in db.LlojiDokumentit select m;

            inspektimList.AddRange(inspektimQy.Distinct());
            ViewBag.InspektimId = new SelectList(inspektimList.Where(s => s.Contains(InspektimId)));
            return View();
        }

        // POST: LlojiDokumentits/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Emri,Data,InspektimId")] LlojiDokumentit llojiDokumentit)
        {
            if (ModelState.IsValid)
            {
                db.LlojiDokumentit.Add(llojiDokumentit);
                db.SaveChanges();
                return RedirectToAction("Index", "LlojiDokumentits", new { llojiDokumentit.InspektimId });
            }

            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", llojiDokumentit.InspektimId);
            return View(llojiDokumentit);
        }

        // GET: LlojiDokumentits/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LlojiDokumentit llojiDokumentit = db.LlojiDokumentit.Find(id);
            if (llojiDokumentit == null)
            {
                return HttpNotFound();
            }
            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", llojiDokumentit.InspektimId);
            return View(llojiDokumentit);
        }

        // POST: LlojiDokumentits/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Emri,Data,InspektimId")] LlojiDokumentit llojiDokumentit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(llojiDokumentit).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.InspektimId = new SelectList(db.Inspektim, "Id", "Emri", llojiDokumentit.InspektimId);
            return View(llojiDokumentit);
        }

        // GET: LlojiDokumentits/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LlojiDokumentit llojiDokumentit = db.LlojiDokumentit.Find(id);
            if (llojiDokumentit == null)
            {
                return HttpNotFound();
            }
            return View(llojiDokumentit);
        }

        // POST: LlojiDokumentits/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            LlojiDokumentit llojiDokumentit = db.LlojiDokumentit.Find(id);

            db.LlojiDokumentit.Remove(llojiDokumentit);
            db.SaveChanges();
            return RedirectToAction("Index", "LlojiDokumentits", new { llojiDokumentit.InspektimId });
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
