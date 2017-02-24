using System.ComponentModel.DataAnnotations;

namespace GO
{
 /// <summary>
 /// Zwischenklasse, weil EF Core bisher kein N:M unterstützt
 /// </summary>
 public class Buchung
 {
  //public int BuchungID { get; set; } // das wäre der automatische PK!
  // --- Primärschlüssel
  //[Key] zusammengesetzer Key geht nicht per Annotation
  public int FlugNr { get; set; }
  //[Key] zusammengesetzer Key geht nicht per Annotation
  public int PassagierID { get; set; }
  // --- Navigationseigenschaften 
  public Flug Flug { get; set; }
  public Passagier Passagier { get; set; }
 }
}
