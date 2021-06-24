using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Arkiva.Models
{
    public class Inspektim
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Emri Institucionit Shteterore")]
        public string Emri { get; set; }

        [Required]
        [Display(Name = "Data e Kontrollit")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data{ get; set; }
        public int SubjektId { get; set; }
        public virtual Subjekt Subjekt { get; set; }
        public virtual ICollection<Dokument> Dokumente { get; set; }
    }
}