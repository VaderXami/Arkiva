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
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Arkiva.Models;

namespace Arkiva.Controllers
{
    /**
    * Data: Data e 10/06/2021
    * Programuesi: Arvin Xamo
    * Klasa: SubjektsController
    * Arsyeja: Me duhet te krijoj nje kontroller per perpunimin e te dhenave.
    * Pershkrimi: Kontrolleri do te bej te mundur perpunimin e te dhenave
    * me qellimin sic ka dhe emrin (per Subjektet), qe te mund ti Krijoj ato,
    * Fshij, Editoj, si dhe te shoh Permbajtjen per cdo Subjekt specifik.
    * Trashegon nga: Controller
    * Interfaces:
    * Constants:
    * Metodat:
    *       Index(string search), ActionResult
    *       Details(int? id), ActionResult
    *       ActionResult Create(), ActionResult
    *       GetNewName(string filename), string
    *       DownloadZipFile(int id), ActionResult
    *       Edit(int? id), ActionResult
    *       Edit([Bind(Include = "Id,Emri,Data")] Subjekt subjekt), ActionResult
    *       Delete(int? id), ActionResult
    *       DeleteConfirmed(int id), ActionResult
    *       Dispose(bool disposing), void
    **/
    [HandleError]
    public class SubjektsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Subjekts
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Index
         * Arsyeja: Per listimin e Subjekteve te regjistruara.
         * Pershkrimi: Ben te mundur listimin e Subjekteve te regjistruara ne Databaze.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          search: Nje variabel e tipit string e cila na sherben per te kryer nje kerkim te subjekteve ne raste se kemi je list te gjat te tyre.
         * Return: Listen e Subjekteve ne View.
        **/
        public ActionResult Index(string search)
        {
            if (!String.IsNullOrWhiteSpace(search))
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
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Details
         * Arsyeja: Shfaq permbajtjen e Subjektit perkates.
         * Pershkrimi: Kjo metod na sherben per te ber te mundur Shfaqen e Permbajtjes se Subjekit te selektuar
         *             si dhe Shkarkimin e ketij te fundit ne formatin e Arkives (.zip).
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          Id: Per te identifikuar se permbajtjen e cilit subjekt duam te shohim.
         * Return: Kthen View se bashku me Subjektin e gjendur nepermjet Id.
        **/
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
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Create
         * Arsyeja: Per krijimin e nje Subjekti te ri.
         * Pershkrimi: Ben te mundur shfaqen e View perkatese e cila eshte e nderlidhur ngushte me metoden respektive
         *  te kontrollerit respektiv. Me posht verehim se bejm te mundur hedhjen e Databaze te te dhenave te cilat meren
         *  nga format nepermjet View.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         * Return: Kthen View e cila do te shfaqi Format e regjistrimit te subjektit.
        **/
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
                
            string newFileName = name +  "_" + DateTime.Now.ToString("yyyyMMddHHmmssffff") + ext;
            return newFileName;
        }

        /**
        * Data: 10/06/2021
        * Programuesi: Arvin Xamo
        * Metoda: DownloadZipFile
        * Arsyeja: Per arkivimin e dokumentave ne raste se perdoruesi ka deshire mos ti shkarkoj nje e nga nje.
        * Pershkrimi: Ben te mundur Krijimin e nje Arkive brenda se ciles kemi subjektin me te gjitha inspektimet perkates ku secili inspektim specifik ka
        * Dokumentat e tij.
        * Para kushti: 
        * Post kushti: 
        * Parametrat:
        *          Id: Identifikojme Subjektin perkates brenda se cilit ndodhen inspektimet ne menyre
        *          qe te shkarkojme ato qe i perkasin ketij Subjekti specifik. Shembull: Supermarket Europa -> Akshi -> 10 Dokumenta
        *                                                                                                      AKU -> 4 Dokumenta
        * Return: Kthen nje Arkive duke perdorur File class.
       **/
  /*      public ActionResult DownloadZipFile(int id)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, true))
                {
                    var inspektime = db.Subjekt.Find(id).Inspektime;
                    foreach (var inspektim in inspektime )
                    {
                        var dokumente = db.Inspektim.Find(inspektim.Id).LlojiDokumentit;
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
        }*/

        // GET: Subjekts/Edit/5
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
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Delete
         * Arsyeja: Fshirjen e Subjektit nese nuk na duhet me.
         * Pershkrimi: Ben te mundur fshirjen e nje Subjekti i cili mund te cilesohet si i padobishem
         * dhe identifikohet me ane te Id perkatese qe ka dhe duke e kaluar ate si parameter.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          Id: Gjen se cilin Subjekt duam te fshijm.
         * Return: Kthen nje View e cila brenda ka Subjektin qe ne duam te fshijm.
        **/
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
