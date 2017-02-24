using System;
using System.Collections.Generic;

namespace GO
{
 public partial class Passagier : Person
 {
  // Primärschlüssel wird geerbt!

  // --- Einfache Eigenschaften
  public Nullable<System.DateTime> KundeSeit { get; set; }
  public string PassagierStatus { get; set; }

  // --- Navigationseigenschaften deklariert auf Mengentyp
  public List<Buchung> BuchungSet { get; set; }
 }
}
