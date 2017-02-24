using System;
using System.Collections.Generic;

namespace GO
{
 public enum Fluglizenztyp
 {
 CPL, PPL, GPL, SPL, LAPL 
 }

 public  partial class Pilot : Person
 {
  // Primärschlüssel wird geerbt!

  // --- Weitere Eigenschaften
  public System.DateTime FlugscheinSeit { get; set; }
  public Nullable<int> Flugstunden { get; set; }
  public Fluglizenztyp Fluglizenztyp { get; set; }
  public string Flugschule { get; set; }

  // --- Navigationseigenschaften deklariert auf Schnittstellen mit expliziter Mengentypinstanziierung 
  public ICollection<Flug> FluegeAlsPilot { get; set; } = new List<Flug>();
  // --- Navigationseigenschaften deklariert auf Mengentyp 
  public List<Flug> FluegeAlsCopilot { get; set; } 
 }
}
