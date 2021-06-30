using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Arkiva.Models
{
    public class Subjekt
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Vendosni emrin e Subjektit perkates!")]
        [Display(Name = "Emri Subjektit Perkates")]
        public string Emri { get; set; }

        [Required]
        [Display(Name = "Data e Regjistrimit")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }
        public virtual ICollection<Inspektim> Inspektime { get; set; }

    }
}