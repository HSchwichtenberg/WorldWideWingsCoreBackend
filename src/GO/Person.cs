using System;
using System.ComponentModel.DataAnnotations.Schema;

using System.ComponentModel.DataAnnotations;

namespace GO
{
 public partial class Person
 {
  // --- Primärschlüssel
  public int ID { get; set; }

  // --- Einfache Eigenschaften
  [StringLength(50)]
  public string Name { get; set; }
  [StringLength(50)]
  public string Vorname { get; set; }
  public Nullable<System.DateTime> Geburtsdatum { get; set; }
  public string Strasse { get; set; }
  public Byte[] Foto { get; set; }
  public string EMail { get; set; }
  public string Stadt { get; set; }
  public string Land { get; set; }

  public virtual string Memo { get; set; }

  // Berechnete Eigenschaft (wird nicht persistiert!)
  public string GanzerName { get { return this.Vorname + " " + this.Name; } }
  // Methode (ohne Bedeutung für ORM)
  public override string ToString()
  {
   return "#" + this.ID + ": " + this.GanzerName;
  }
 }
}
