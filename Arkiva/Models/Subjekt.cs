/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila sherben si Model per Subjektet si dhe Migrimin e ksaj te fundit ne databaze ne menyre qe te krijojme tabelen.
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
     * Metodat: Metodat jan ato te cilat ne baze te Attributeve(Data, Emri, Permbajtja etj) qe kemi per kete model ne Baze te Entitetit(ne rastin tone Subjekt) vendosim Cilesite (Supermarket Europa)
     * me ate te set; dhe i marim ato me get; nese na duhen per ti manipuluar.
     **/
    public class Subjekt
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Emri Subjektit Përkatës!", AllowEmptyStrings = false)]
        [Display(Name = "Emri Subjektit Përkatës")]
        [MaxLength(35, ErrorMessage = "Emri Subjektit duhet të jetë deri në 35 karaktere.")]
        [RegularExpression(@"^(?!^ +$)^.+$", ErrorMessage = "Ju lutem, plotësoni Emri Subjektit Përkatës!")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Data e Regjistrimit!")]
        [Display(Name = "Data e Regjistrimit")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Data { get; set; }
        public virtual ICollection<Inspektim> Inspektime { get; set; }
    }

}