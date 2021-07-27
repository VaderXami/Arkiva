﻿/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila sherben si Model per Inspektimet si dhe Migrimin e ksaj te fundit ne databaze ne menyre qe te krijojme tabelen.
(c) Copyright by Soft & Solution (opsionale)
**/


using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
     * Metodat: Metodat jan ato te cilat ne baze te Attributeve(Data, Emri etj) qe kemi per kete model ne Baze te Entitetit(ne rastin tone Inspektim) vendosim Cilesite (AKSHI)
     * me ate te set; dhe i marim ato me get; nese na duhen per ti manipuluar.
     **/
    public class Inspektim
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Emri Institucionit Shtetëror!", AllowEmptyStrings = false)]
        [Display(Name = "Emri Institucionit Shtetëror")]
        [MaxLength(35, ErrorMessage = "Emri Inspektimit duhet të jetë deri në 35 karaktere.")]
        [RegularExpression(@"^(?![\s.]+$)[a-zA-Z\s.]*$", ErrorMessage = "Ju lutem, plotësoni fushën Emri Institucionit Shtetëror!")]
        public string Emri { get; set; }


        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Nr. Inspektimit!")]
        [Display(Name = "Nr. Inspektimit")]
        [RegularExpression(@"[0-9]*$", ErrorMessage = "Ju lutem, plotësoni fushën Nr. Inspektimit me një numër të saktë!")]
        public int NrInspektimit { get; set; }

        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Data e Kontrollit!")]
        [Display(Name = "Data e Kontrollit")] 
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data{ get; set; }
        public int SubjektId { get; set; }
        public virtual Subjekt Subjekt { get; set; }
        public virtual ICollection<LlojiDokumentit> LlojiDokumentit { get; set; }
    }
}