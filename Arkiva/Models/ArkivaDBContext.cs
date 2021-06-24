using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Arkiva.Models
{
    public class ArkivaDBContext : DbContext
    {
        public DbSet<Subjekt> Subjekt { get; set; }
        public DbSet<Inspektim> Inspektim { get; set; }
        public DbSet<Dokument> Dokument { get; set; }
    }
}