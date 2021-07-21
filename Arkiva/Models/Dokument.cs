/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila sherben si Model per Dokumentat si dhe Migrimin e ksaj te fundit ne databaze ne menyre qe te krijojme tabelen.
(c) Copyright by Soft & Solution (opsionale)
**/

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.Web.Mvc;
using Xunit;
using Xunit.Sdk;

namespace Arkiva.Models
{
    /**
     * Data: Data e 10/06/2021
     * Programuesi: Arvin Xamo
     * Klasa: Dokument
     * Arsyeja: Krijimin e tabelave ne databaze si dhe ruajtjen perkoheshisht te te dhenave (ndryshe e njohim si medium transmetimi te perkohshem) nepermjet
     * atributeve te ndryshme qe mund ti vendosim ne baze te qellimit te saj.
     * Pershkrimi: Modeli permban Attributet(Emri) kryesore te cilat nje Entitet(Njeriu) si qellim ruajtjen e Cilesive(Profesor SHLLAJFI) te ktij te fundit.
     * Trashegon nga:
     * Interfaces:
     * Constants:
     * Metodat: Metodat jan ato te cilat ne baze te Attributeve(Data, Emri, Permbajtja etj) qe kemi per kete model ne Baze te Entitetit(ne rastin tone Dokumenti) vendosim Cilesite (dokument.pdf)
     * me ate te set; dhe i marim ato me get; nese na duhen per ti manipuluar.
     **/

    public class Dokument
    {
        public int Id { get; set; }

        [Display(Name = "Emri Subjektit")]
        public string EmriSubjektit { get; set; }
        [Display(Name = "Emri Inspektuesit")]
        public string EmriInspektuesit { get; set; }
        [Display(Name = "Nr. Inspektimit")]
        public int NrInspektuesit { get; set; }
        [Display(Name = "Lloji Dokumentit")]
        public string EmriLlojitDokumentit { get; set; }

        [Display(Name = "Data e Regjistrimit")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy HH:mm:ss}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        [Display(Name = "Emri Dokumentit")]

        public String FileName { get; set; }
        public byte[] FileContent { get; set; }

        [NotMapped]
        [Display(Name = "Ngarko Dokument")]
        public HttpPostedFileBase[] Files { get; set; }

        [Display(Name = "Zyra e Dokumentit")]
        [MaxLength(35, ErrorMessage = "Emri Zyrës duhet të jetë deri në 35 karaktere.")]
        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Zyra e Dokumentit!", AllowEmptyStrings = false)]
        [RegularExpression(@"^(?!^ +$)^.+$", ErrorMessage = "Ju lutem, plotësoni fushën Zyra e Dokumentit")]
        public string Zyra { get; set; }

        [Display(Name = "Nr. Kutisë")]
        [Range(0, int.MaxValue, ErrorMessage = "Kapaciteti maksimal është 10 shifra!")]
        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Nr. Kutisë!")]
        public int NrKutis { get; set; }

        [Display(Name = "Rafti Përkatës")]
        [Range(0, int.MaxValue, ErrorMessage = "Kapaciteti maksimal është 10 shifra!")]
        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Rafti Përkatës!")]
        public int Rafti { get; set; }

        [Display(Name = "Fusha e Indeksimit")]
        [Range(0, int.MaxValue, ErrorMessage = "Kapaciteti maksimal është 10 shifra!")]
        [Required(ErrorMessage = "Ju lutem, plotësoni fushën e Indeksimit!")]
        public int Indeksimi { get; set; }
        public int LlojiDokumentitId { get; set; }
        public virtual LlojiDokumentit LlojiDokumentit { get; set; }

        public static implicit operator Dokument(FileContentResult v)
        {
            throw new NotImplementedException();
        }
    }
}