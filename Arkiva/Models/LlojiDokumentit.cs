using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Arkiva.Models
{
    public class LlojiDokumentit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Lloji i Dokumentit!")]
        [Display(Name = "Lloji i Dokumentit")]
        [MaxLength(35, ErrorMessage = "Emri i Llojit te Dokumentit duhet të jetë deri në 35 karaktere.")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Ju lutem, plotësoni fushën Data e Regjistrimit!")]
        [Display(Name = "Data e Regjistrimit")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public int InspektimId { get; set; }
        public virtual Inspektim Inspektim { get; set; }
        public virtual ICollection<Dokument> Dokumente { get; set; }
    }
}