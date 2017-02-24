using ITVisions.EFC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DZ;
using GO;

namespace GL
{
 /// <summary>
 /// Datenmanager für Flug-Entitäten
 /// abgeleitet von DataManagerBase
 /// </summary>
 public class FlugManager : ITVisions.EFC.EntityManagerBase<WWWingsModell, Flug>
 {
  public FlugManager() 
  {  }

  ///<summary>
  /// Dieser Konstruktur muss internal sein, denn sonst würde das WebAPI eine Referenz auf EF brauchen!!!
  /// </summary>
  internal FlugManager(WWWingsModell kontext = null, bool tracking = false) : base(kontext, tracking)
  {   }

  public Flug GetFlug(int FlugID)
  {
     return ctx.FlugSet.Find(FlugID);
  }

  public List<Flug> GetFlugSet(string Abflugort, string Zielort)
  {
   // Nutzung der Query-Hilfsmethode aus der Basisklasse
   var abfrage = from f in ctx.FlugSet select f;

   // Eigentliche Logik für das Zusammensetzen der Abfrage
   if (!String.IsNullOrEmpty(Abflugort)) abfrage = from f in abfrage
                                                   where f.Abflugort == Abflugort
                                                   select f;

   if (!String.IsNullOrEmpty(Zielort)) abfrage = abfrage.Where(f => f.Zielort == Zielort);

   // Ausführen der Abfrage
   List<Flug> ergebnis = abfrage.ToList();

   return ergebnis;
  }

  public List<string> GetFlughaefen()
  {
   var l1 = ctx.FlugSet.Select(f => f.Abflugort).Distinct();
   var l2 = ctx.FlugSet.Select(f => f.Zielort).Distinct();
   var l3 = l1.Union(l2).Distinct();
   return l3.OrderBy(z => z).ToList();
  }


  /// <summary>
  /// Implementierung einer Kapselung von SaveChanges() direkt in einer konkreten Datenzugriffsmanagerklasse
  /// Rückgabewert ist die Summe der Anzahl der gespeicherten neuen, geänderten und gelöschten Datensätze
  /// </summary>
  /// <returns></returns>
  public int Speichern()
  {
   return ctx.SaveChanges();
  }

  /// <summary>
  /// Änderungen an einer Liste von Passagieren speichern
  /// Die neu hinzugefügten Passagiere muss die Routine wieder zurückgeben, da die IDs für die 
  /// neuen Passagiere erst beim Speichern von der Datenbank vergeben werden
  /// </summary>
  public List<Flug> Speichern(List<Flug> flugSet, out string statistik)
  {
   return Save<Flug>(flugSet, out statistik);
  }

  /// <summary>
  /// Reduziert die Anzahl der freie Plätzen auf dem genannten Flug, sofern die Plätze noch verfügbar sind. Liefert true, wenn  erfolgreich, sonst false.
  /// </summary>
  /// <param name="FlugID"></param>
  /// <param name="PlatzAnzahl"></param>
  /// <returns>true, wenn erfolgreich</returns>
  public bool ReducePlatzAnzahl(int FlugID, short PlatzAnzahl)
  {
   var einzelnerFlug = GetFlug(FlugID);
   if (einzelnerFlug != null)
   {
    if (einzelnerFlug.FreiePlaetze >= PlatzAnzahl)
    {
     einzelnerFlug.FreiePlaetze -= PlatzAnzahl;
     ctx.SaveChanges();
     return true;
    }
   }
   return false;
  }

  #region Beispiel für GroupBy-Problem mit EF statt EFC. Läuft nicht auf .NET Core!!!
  //public class AbflugortStatistik
  //{
  // public string Ort { get; set; }
  // public int Anzahl { get; set; }
  //}

  //public List<AbflugortStatistik> GetFluegeProAbflugOrt()
  //{
  // System.Data.Entity.Database.SetInitializer<WWWingsModell_EF>(null);
  // using (var ctx = new WWWings.DZ.EF.WWWingsModell_EF())
  // {
  //  //Console.WriteLine(ctx.Database.Connection.ConnectionString);    ctx.FlugSet.ToList();
  //  var gruppenSet = new List<AbflugortStatistik>();
  //  var gruppen = from p in ctx.FlugSet
  //                orderby p.FreiePlaetze
  //                group p by p.Abflugort into g
  //                select new { Ort = g.Key, Anzahl = g.Count() };

  //  //Console.WriteLine("Anzahl: " + gruppen.Count());

  //  //OO - Mapping
  //  foreach (var g in gruppen)
  //  {
  //   gruppenSet.Add(new AbflugortStatistik() { Anzahl = g.Anzahl, Ort = g.Ort });
  //  }
  //  return gruppenSet;
  // }

  //}
  #endregion

 }
}
