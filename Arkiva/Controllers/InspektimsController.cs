/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila eshte e perbere nga disa metoda te cilat sherbejne per kryerjen e procedurave te caktuara
* si Krijimin e nje Inspektimi, Listimin e tyre dhe me pas Fshirjen.
* Gjithashtu ka dhe metoda ndihmese ne varesi te perdorimit i cili percakton dhe funksionalitetin me ane te qellimit qe ka metoda vet.
(c) Copyright by Soft & Solution (opsionale)
**/

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
    /**
     * Data: Data e 10/06/2021
     * Programuesi: Arvin Xamo
     * Klasa: InspektimsController
     * Arsyeja: Kontrolleri me duhet per perpunimin e te dhenave dhe me pas shfaqjen e tyre.
     * Pershkrimi: Kontrolleri do te bej te mundur perpunimin e te dhenave
     * me qellimin sic ka dhe emrin (per Inspektimet), qe te mund ti Krijoj ato,
     * Fshij, Editoj, si dhe te shoh Permbajtjen per cdo Inspektim specifik.
     * Trashegon nga: Controller
     * Interfaces:
     * Constants:
     * Metodat:
     *      Index(int SubjektId, string search), ActionResult
     *      Details(int? id), ActionResult
     *      GetNewName(string filename), string
     *      DownloadZipFile(int id), ActionResult
     *      Create(int SubjektId), ActionResult
     *      Create([Bind(Include = "Id,Emri,Data,SubjektId")] Inspektim inspektim), ActionResult
     *      Edit(int? id), ActionResult
     *      Edit([Bind(Include = "Id,Emri,Data,SubjektId")] Inspektim inspektim), ActionResult
     *      Delete(int? id), ActionResult
     *      DeleteConfirmed(int id), ActionResult
     *      Dispose(bool disposing), void
     **/

    [HandleError]
    public class InspektimsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Inspektims
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Index
         * Arsyeja: Per shfaqjen e Listes se Inspektimeve.
         * Pershkrimi: Shfaq listen e inspektimeve te cilat i perkasin nje Subjekti te caktuar duke identifikuar kete
         * te fundit me ane te Id perkatese qe ka dhe duke e kaluar ate si parameter.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          SubjektId: Id e inspektimit me Emrin e folderit specifik i krijuar ne nje date specifike.
         *                      Ne menyre te tille ne e dime se cilet dokument do te bejme display ne liste dhe cilet jo.
         *          search: Sherben si fjale kyce per gjetjen e dokumentit sipas emrit perkates te specifikuar ne kete string.
         * Return: Kthen nje View e cila brenda ka listen e inspektimeve te caktuara sipas kushteve te percaktuara me poshte.
        **/
        public ActionResult Index(DateTime? start, int SubjektId, string search)
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
            if (!String.IsNullOrWhiteSpace(start.ToString()) && String.IsNullOrWhiteSpace(search))
            {
                var inspektim2 = db.Inspektim.Where(i => i.SubjektId == SubjektId);
                var list = inspektim2.Where(e => e.Data == start);
                return View(list);
            }
            else if (!String.IsNullOrWhiteSpace(search))
            {
                if (search.All(char.IsDigit))
                {
                    inspektim = inspektim.Where(d => d.NrInspektimit.ToString().Contains(search));
                } else
                {
                    inspektim = inspektim.Where(x => x.Emri.Contains(search));
                }
                if (search.Trim().Contains(";"))
                {
                    List<Inspektim> listInspektime = new List<Inspektim>();
                    string[] emrat = search.Split(';');
                    foreach (string tmp in emrat)
                    {
                        var ins = db.Inspektim.Where(s => s.SubjektId == SubjektId);
                        var tempList = ins.Where(x => x.Emri.Contains(tmp.Trim()));
                        listInspektime.AddRange(tempList);
                        listInspektime = listInspektime.Distinct().ToList();
                    }
                    if (listInspektime.Any())
                    {
                        ViewBag.Message = "";
                        return View(listInspektime);
                    }
                    else
                    {
                        return View(listInspektime);
                    }
                }
            }
            return View(inspektim.ToList());
        }

        // GET: Inspektims/Details/5
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Details
         * Arsyeja: Per shfaqjen e Listes se Inspektimeve.
         * Pershkrimi: Shfaq listen e inspektimeve te cilat i perkasin nje Subjekti te caktuar duke identifikuar kete
         * te fundit me ane te Id perkatese qe ka dhe duke e kaluar ate si parameter.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          id : Id e inspektimit me Emrin e folderit specifik i krijuar ne nje date specifike.
         *                      Ne menyre te tille ne e dime se te dhenat e kujt Inspektim do te shfaqim.
         * Return: Kthen nje View e cila brenda ka vetem nje Inspektim specifik i cili na sherben per shaqjen e te dhenave (listen e dokumentave qe permban brenda).
        **/
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

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: GetNewName
         * Arsyeja: Per vendosjen e nje emri unik te Dokumentit.
         * Pershkrimi: Ben vedosjen e nje emri unik te nje Dokumenti te caktuar i cili shfaqet ne permbajtjen e Inspektimeve.
         * Para kushti: 
         * Post kushti: 
         * Parametrat: 
         *            filename: eshte nje fjale kyce e tipit string qe ben te mundur ruajtjen e emrit te
         *            Dokumentit.
         * Return: Nje emer dokumenti te ri te tipit string.
        **/
        public string GetNewName(string filename)
        {
            string ext = Path.GetExtension(filename);
            string name = Path.GetFileNameWithoutExtension(filename);

            string newFileName = name + "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
            return newFileName;
        }

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: DownloadZipFile
         * Arsyeja: Per arkivimin e dokumentave ne raste se perdoruesi ka deshire mos ti shkarkoj nje e nga nje.
         * Pershkrimi: Ben te mundur Krijimin e nje Arkive brenda se ciles kemi dokumentat perkates persa i perket ktij
         * inspektimi specifik.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          Id: Identifikojme Inspektimin perkates brenda se cilit ndodhen dokumentat ne menyre
         *          qe te shkarkojme ato qe i perkasin ketij Inspektimi specifik. Shembull: Akshi -> 10 Dokumenta, AKU -> 4 Dokumenta
         * Return: Kthen nje Arkive duke perdorur File class.
        **/
        public ActionResult DownloadZipFile(int id)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var llojDokumentit = db.Inspektim.Find(id).LlojiDokumentit;
                    foreach (var lloj in llojDokumentit)
                    {
                        var dokumente = db.LlojiDokumentit.Find(lloj.Id).Dokumente;
                        foreach (var dokument in dokumente)
                        {
                            var file = archive.CreateEntry(lloj.Emri + "/" + GetNewName(dokument.FileName.ToString()));
                            using (var stream = file.Open())
                            {
                                stream.Write(dokument.FileContent, 0, dokument.FileContent.Length);
                            }
                        }
                    }
                }
                var inspektim = db.Inspektim.Find(id);
                return File(memoryStream.ToArray(), "application/zip", inspektim.Emri + ".zip");
            }
        }

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Create
         * Arsyeja: Te krijojme nje Inspektim te ri.
         * Pershkrimi: Ben te mundur krijimin e nje Inspektimi te ri si dhe caktimin e vlerave nga UI persa i perket prototipit te dyte te Create (metoda e dyte e Create)
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          SubjektId: Kjo e fundit na sherben si ure lidhese per identifikimin se cilit Subjekt duhet tja servirim apo shoqerojme me kete te fundit.
         * Return: Kthen nje View e ku shfaqet UI i nderlidhur me metoden Create.
        **/
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
        public ActionResult Create([Bind(Include = "Id,Emri,Data,NrInspektimit,SubjektId")] Inspektim inspektim)
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

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Edit
         * Arsyeja: Per ndryshim te Emrin te Inspektimit dhe Dates se regjistrimit ne rast gabimi njerezor.
         * Pershkrimi: Me ane te id qe e mer si parameter ben te mundur identifikimin e Inspektimit dhe me pas shfaq keto te dhena
         * me qellim ndryshimin e tyre nese deshirojme.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *            Id: Sherben per te identifikuar se cilin Inspektim kemi deshire te editojme.
         * Return: Kthen nje View me Inspektimin perkates te cilin duam ta editojme.
        **/
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
        public ActionResult Edit([Bind(Include = "Id,Emri,Data,NrInspektimit,SubjektId")] Inspektim inspektim)
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
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Delete
         * Arsyeja: Fshirjen e Inspektimit nese nuk na duhet me.
         * Pershkrimi: Ben te mundur fshirjen e nje inspektimi i cili mund te cilesohet si i padobishem
         * dhe identifikohet me ane te Id perkatese qe ka dhe duke e kaluar ate si parameter.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          Id: Gjen se cilin Inspektim duam te fshijm.
         * Return: Kthen nje View e cila brenda ka Inspektimin qe ne duam te fshijm.
        **/
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

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Dispose
         * Arsyeja: Per clirimin e webit nga burimet te cilat nuk i perdorim.
         * Pershkrimi: Bejne te mundur cilirimin e databases nga burime e cilat nuk perdoren apo ndryshe burime te pa menaxhuara dhe liron vende per te rejat.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          disposing: Nje vler booleane e cila ben te mundur check-im nese duhet ti bej dispose apo ti cliroj keto te dhena apo jo
         * Return:
        **/
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
