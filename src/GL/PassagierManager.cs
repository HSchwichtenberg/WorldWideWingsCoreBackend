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
 /// Datenmanager für Passagier-Entitäten
 /// </summary>
 public class PassagierManager : ITVisions.EFC.DataManagerBase<WWWingsModell>
 {

   /// <summary>
  /// Öffentlicher Konstruktor für GL
  /// </summary> 
  /// 
  public PassagierManager() : this(null)
  {  }

  ///<summary>
  /// Dieser Konstruktur muss internal sein, denn sonst würde das WebAPI eine Referenz auf EF brauchen!!!
  /// </summary>
  internal PassagierManager(WWWingsModell kontext = null)
  : base(kontext)
  {  }

  /// <summary>
  /// Holt einen Passagier
  /// </summary>
  public Passagier GetPassagier(int PassagierID)
  {
   // .OfType<Passagier>() notwendig wegen Vererbung
   var abfrage = from p in ctx.PassagierSet where p.ID == PassagierID select p;
   return abfrage.SingleOrDefault();
  }

  /// <summary>
  /// Holt alle Passagiere mit einem Namensbestandteil
  /// </summary>
  public List<Passagier> GetPassagierSet(string Namensbestandteil)
  {
   // .OfType<Passagier>() notwendig wegen Vererbung
   var abfrage = from p in ctx.PassagierSet where p.Name.Contains(Namensbestandteil) || p.Vorname.Contains(Namensbestandteil) select p;
   return abfrage.ToList();
  }

  /// <summary>
  /// Holt alle Passagiere mit einem Namensbestandteil
  /// </summary>
  public List<Passagier> GetPassagierSet()
  {
   // .OfType<Passagier>() notwendig wegen Vererbung
   var abfrage = from p in ctx.PassagierSet select p;
   return abfrage.ToList();
  }

  /// <summary>
  /// Füge einen Passagier zu einem Flug hinzu
  /// </summary>
  public bool AddPassagierZuFlug(int PassagierID, int FlugID)
  {
   try
   {
    var fm = new FlugManager(ctx, true);

    Flug flug = fm.GetFlug(FlugID); 
 
    // Hinzufügen über "Join"-Klasse
    var b = new Buchung();
    b.FlugNr = FlugID;
    b.PassagierID = PassagierID;
    flug.BuchungSet.Add(b);

    int anz = ctx.SaveChanges();
    if (anz != 1) return false;
    fm.Dispose();
    return true;
   }
   catch (Exception)
   {
    return false;
   }
  }

  /// <summary>
  /// Änderungen an einer Liste von Passagieren speichern
  /// Die neu hinzugefügten Passagiere muss die Routine wieder zurückgeben, da die IDs für die 
  /// neuen Passagiere erst beim Speichern von der Datenbank vergeben werden
  /// Statistik liefert einen Kette der Form "Geändert: 0 Neu: 1 Gelöscht: 0"
  /// </summary>
  public List<Passagier> SavePassagierSet(List<Passagier> PassagierSet, out string Statistik)
  {
   return Save(PassagierSet, out Statistik);
  }
 }
}
