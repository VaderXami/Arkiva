using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Arkiva.Models
{
    public class LlojiDokumentit
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Vendosni emrin e Institucionit Inspektues!")]
        [Display(Name = "Lloji i Dokumentit")]
        public string Emri { get; set; }

        [Required(ErrorMessage = "Vendosni daten perkatese te regjistrimit!")]
        [Display(Name = "Data e Regjistrimit")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public int InspektimId { get; set; }
        public virtual Inspektim Inspektim { get; set; }
        public virtual ICollection<Dokument> Dokumente { get; set; }
    }
}