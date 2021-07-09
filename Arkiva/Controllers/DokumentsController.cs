/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila eshte e perbere nga disa metoda te cilat sherbejne per kryerjen e procedurave te caktuara
* si Krijimin e nje file, Listimin e tyre dhe me pas Fshirjen.
* Gjithashtu ka dhe metoda ndihmese ne varesi te perdorimit i cili percakton dhe funksionalitetin me ane te qellimit qe ka metoda vet.
(c) Copyright by Soft & Solution (opsionale)
**/

using Arkiva.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Arkiva.Controllers
{
    /**
     * Data: Data e 10/06/2021
     * Programuesi: Arvin Xamo
     * Klasa: DokumentsController
     * Arsyeja: Me duhet te krijoj nje kontroller per perpunimin e te dhenave.
     * Pershkrimi: Kontrolleri do te bej te mundur perpunimin e te dhenave
     * me qellimin sic ka dhe emrin (per Dokumentat), qe te mund ti regjistroje ato,
     * fshij, listimin dhe Shikimin e tyre te tipit: pdf, png, jpeg dhe jpg.
     * Trashegon nga: Controller
     * Interfaces:
     * Constants:
     * Metodat:
     * Index(int InspektimId, string search), ActionResult
     * 
     * DownLoadFile(int id), FileResult
     * PreviewFilePDF(int id), ActionResult
     * PreviewFilePNG(int id), ActionResult
     * PreviewFileJPEG(int id), ActionResult
     * PreviewFileJPG(int id), ActionResult
     * 
     * Create(string InspektimId), ActionResult
     * Create([Bind(Include = "Id,Data,FileName,FileContent,InspektimId")] Dokument dokument, HttpPostedFileBase[] files), ActionResult
     * 
     * Delete(int? id), ActionResult
     * DeleteConfirmed(int id), ActionResult
     * Dispose(bool disposing), void
     **/
    [HandleError]
    public class DokumentsController : Controller
    {
        private ArkivaDBContext db = new ArkivaDBContext();

        // GET: Dokuments
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: Index
         * Arsyeja: Per shfaqjen e Listes se dokumentave.
         * Pershkrimi: Shfaq listen e dokumentave te cilat i perkasin nje inspektimi te caktuar
         * i cili dhe ky i fundit i perket nje subjekti te caktuar.
         * Para kushti: 
         * Post kushti: 
         * Parametrat:
         *          InspektimId: Id e inspektimit me Emrin e folderit specifik i krijuar ne nje date specifike.
         *                      Ne menyre te tille ne e dime se cilet dokument do te bejme display ne liste dhe cilet jo.
         *          search: Sherben si fjale kyce per gjetjen e dokumentit sipas emrit perkates te specifikuar ne kete string.
         * Return: Kthen nje View e cila brenda ka listen e dokumentave te caktuara sipas kushteve te percaktuara me poshte.
        **/

        public ActionResult Index(int LlojiDokumentitID, string search)
        {

            var llojiList = new List<string>();
            var llojiQy = from d in db.LlojiDokumentit orderby d.Id select d.Id.ToString();
            var dokument = from m in db.Dokument select m;

            llojiList.AddRange(llojiQy.Distinct());
            ViewBag.SubjektId = new SelectList(llojiList.Where(s => s.Contains(LlojiDokumentitID.ToString())));

            if (!String.IsNullOrEmpty(LlojiDokumentitID.ToString()))
            {
                dokument = dokument.Where(s => s.LlojiDokumentitId == LlojiDokumentitID);
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

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metoda: DownloadFile
         * Arsyeja: Per shkarkimin e dokumentave.
         * Pershkrimi: Shkarkon dokumentat te cilat shfaqen ne listen e permendur me siper me nje emer unik ne baze te dates
         * dhe ores aktuale.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          id: Id e dokumentit perkates i cili na duhet ta gjejme ne databaze dhe te kryejme veprimet te renditura si me poshte.
         * Return: Kthen nje File (dokument) nxitur nga FileResult e cila ben te mundur vendosjen ne formen e binary code si pergjigje. Kjo e fundit duke perdorur File Class na kthen
         * nje file ne baze te specifikimeve qe i kemi vendosur.
        **/

        [HttpGet]
        public FileResult DownLoadFile(int id)
        {
            Dokument dokument = db.Dokument.Find(id);
            string filename = Path.GetFileName(dokument.FileName).ToLower();
            string ext = Path.GetExtension(dokument.FileName).ToLower();

            string newFilename = filename + "_" + dokument.Data.ToString("yyyyMMddHHmmssffff") + ext;

            return File(dokument.FileContent, "application/" + ext, newFilename);
        }

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: PreviewFilePDF, PreviewFilePNG, PreviewFileJPG dhe PreviewFileJPEG
         * Arsyeja: Per Hapjen e dokumentit dhe parjen e pershkrimit te tij.
         * Pershkrimi: Bejne te mundur hapjen e dokumentit dhe jo shkarkimin e tije. Kjo vlen per te gjitha formatet e vendosura
         * ne pershkrimin e Detyres se Arkives. Formate si : PNG, JPG, JPEG dhe PDF.
         * dhe ores aktuale.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          id: Id e dokumentit perkates i cili na duhet ta gjejme ne databaze dhe te kryejme veprimet te renditura si me poshte.
         * Return: Kthen nje File (dokument) nxitur nga FileResult e cila ben te mundur vendosjen ne formen e binary code si pergjigje. Kjo e fundit duke perdorur File Class na kthen
         * nje file ne baze te specifikimeve qe i kemi vendosur.
        **/

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
        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Create
         * Arsyeja: Per krijimin e nje dokumenti te ri.
         * Pershkrimi: Metoda ben te mundur gjetjen e nje inspektimi te caktuar ne menyre qe ti atribuojme dokumentit inspektimin e sakte.
         * Pasi gjejme inspektimin e sakte dhe inspektimi ka te njejten id me subjektin pasi ky i fundit ja jep Id, e kalojme ne nje ViewBag qe te bejme selektimin.
         * Ne rastin tone ne shfaqim ate qe eshte vetem i sakti (pra vetem 1) ne menyre qe mos te kemi komplikacione.
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          InspektimId: Duke mar id e inspektimit e cila perkon me ate te subjektit jemi te gatshem qe te kryejme veprimet si me poshte. Eshte e tipit string pasi e marim nga url Id e sakte.
         * Return: Kthen View e metodes Create nxitur nga ActionResult.
        **/
        public ActionResult Create(string LlojiDokumentitId)
        {
            var llojiList = new List<string>();
            var llojiQy = from d in db.LlojiDokumentit orderby d.Id select d.Id.ToString();
            var dokument = from m in db.Dokument select m;
            
            llojiList.AddRange(llojiQy.Distinct());
            ViewBag.LlojiDokumentitId = new SelectList(llojiList.Where(s => s.Contains(LlojiDokumentitId)));
            return View();
        }

        // POST: Dokuments/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Data,FileName,FileContent,LlojiDokumentitId")] Dokument dokument, HttpPostedFileBase[] files)
        {
            if (ModelState.IsValid)
            {
                foreach (HttpPostedFileBase Files in files)
                {
                    Stream str = Files.InputStream;
                    BinaryReader Br = new BinaryReader(str);
                    Byte[] FileDet = Br.ReadBytes((Int32)str.Length);
                    
                    dokument.FileName = Files.FileName;
                    dokument.FileContent = FileDet;
                    dokument.Data = DateTime.Now;
                    db.Dokument.Add(dokument);
                    db.SaveChanges();
                    return RedirectToAction("Index", "Dokuments", new { dokument.LlojiDokumentitId });
                }   
            }
            ViewBag.LlojiDokumentitId = new SelectList(db.Inspektim, "Id", "Emri", dokument.LlojiDokumentitId);
            return View(dokument);
        }

        /**
         * Data: 10/06/2021
         * Programuesi: Arvin Xamo
         * Metodat: Delete
         * Arsyeja: Fshirje te dokumentit.
         * Pershkrimi: Metoda ben te mundur fshirjen e nje dokumenti i cili ndoshta nuk na duhet apo perdoruesit nuk i duhet me.
         * Paraprakisht kjo metode ben te mundur gjetjen dhe menaxhimin e Errorit nese nuk gjendet ky dokument me kete id. (Provo ti ndryshosh id ne link.)
         * Para kushti:
         * Post kushti:
         * Parametrat:
         *          id: Id e dokumentit perkates i cili na duhet ta gjejme ne databaze dhe te kryejme veprimet te renditura si me poshte.
         * Return: Kthen nje View me dokumentin perkates nese ai gjendet.
        **/

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
            return RedirectToAction("Index", "Dokuments", new { dokument.LlojiDokumentitId });
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
