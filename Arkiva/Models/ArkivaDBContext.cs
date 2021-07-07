/**
* Versioni: V 1.0.0
* Data: 10/06/2021
* Programuesi: Arvin Xamo
* Pershkrimi: File permban 1 Klas e cila sherben per vendosjen e te dhenave ne databaze ne baze te Emrit te Modelit qe kemi krijuar.
(c) Copyright by Soft & Solution (opsionale)
**/

using System.Data.Entity;

namespace Arkiva.Models
{
    /**
     * Data: Data e 10/06/2021
     * Programuesi: Arvin Xamo
     * Klasa: DokumentsController
     * Arsyeja: Aksesimin e tabelave ne datavaze duke mar parasysh Modelet.
     * Pershkrimi: Nevoja per te aksesuar tabelat te cilat na duhen per hedhjen e te dhenave sipas emrit qe kan. Subjektet, Inspektimet dhe Dokumentet.
     * Krijimin e kam kryer duke ber te mundur plotesimin e Modeleve fillimisht dhe me pas duke migruar tabelat pa te dhena paraprake.
     * Trashegon nga: DbContext
     * Interfaces:
     * Constants:
     * Metodat: Subjekt { get; -> Kthen tabelen e Subjekteve (e akseson); set; -> Vendos te dhena ne tabel }, DbSet<Subjekt>
     *          Inspektim { get; -> Kthen tabelen e Inspektimeve (e akseson); set; -> Vendos te dhena ne tabel }, DbSet<Inspektim>
     *          Dokument { get; -> Kthen tabelen e Dokumenteve (e akseson); set; -> Vendos te dhena ne tabel }, DbSet<Dokument>
     **/
    public class ArkivaDBContext : DbContext
    {
        public DbSet<Subjekt> Subjekt { get; set; }
        public DbSet<Inspektim> Inspektim { get; set; }
        public DbSet<Dokument> Dokument { get; set; }
    }
}