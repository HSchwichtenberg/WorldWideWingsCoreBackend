using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GO
{
 [Table("Flug")]
 public partial class Flug
 {
  // Parameterloser Konstruktor ist immer notwendig für EF!
  public Flug() {  }

  public Flug(string abflugort, string zielort)
  {
   this.Abflugort = abflugort;
   this.Zielort = zielort;
  }

  // --- Primärschlüssel 
  [Key]
  [DatabaseGenerated(DatabaseGeneratedOption.None)]
  public int FlugNr { get; set; }

  // --- Einfache Eigenschaften
  [StringLength(50)]
  public string Abflugort { get; set; }
  [StringLength(50)]
  public string Zielort { get; set; }


  public System.DateTime Datum { get; set; }
  public bool NichtRaucherFlug { get; set; }
  public short? Plaetze { get; set; }
  public short? FreiePlaetze { get; set; }
  public decimal Preis { get; set; }

  // Explizites Property
  private string memo;
  public string Memo
  {
   get { return this.memo; }
   set { this.memo = value; }
  }

  // Navigationseigenschaften
  public Pilot Pilot { get; set; }
  public Pilot Copilot { get; set; }
  public ICollection<Buchung> BuchungSet { get; set; } = new List<Buchung>();

  // Explizite Fremdschlüsseleigenschaften zu den Navigationseigenschaften
  public int PilotId { get; set; }
  public int CopilotId { get; set; }

  // Methode (ohne Bedeutung für ORM)
  public override string ToString()
  {
   return String.Format($"Flug #{this.FlugNr}: von {this.Abflugort} nach {this.Zielort} Freie Plätze: {this.FreiePlaetze}");
  }
 }
}