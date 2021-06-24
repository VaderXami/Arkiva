using Microsoft.TeamFoundation.VersionControl.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Arkiva.Models
{
    public class Dokument
    {
        public int Id { get; set; }
        [Required]
        [Display(Name = "Data e Regjistrimit")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Data { get; set; }

        public String FileName { get; set; }
        public byte[] FileContent { get; set; }
        [NotMapped]
        public HttpPostedFileBase[] Files { get; set; }
        public int InspektimId { get; set; }
        public virtual Inspektim Inspektim { get; set; }

        public static implicit operator Dokument(FileContentResult v)
        {
            throw new NotImplementedException();
        }
    }
}