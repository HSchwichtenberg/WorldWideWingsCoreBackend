using ITVisions.EFC;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using DZ;
using GO;
using ITVisions;

namespace GL
{
 /// <summary>
 /// Generiert zufällige Testdaten für die Datenbank aus häufigen Vor- und Nachnamen sowie ausgewählten Städten
 /// </summary>
 public static class Datengenerator
 {
  static string[] Flughaefen = { "Berlin", "Frankfurt", "München", "Hamburg", "Köln/Bonn", "Rom", "London", "Paris", "Mailand", "Prag", "Moskau", "New York", "Seattle", "Essen/Mülheim", "Kapstadt", "Madrid", "Oslo", "Dallas", "Graz" };
  // Häufigste Vor- und Nachnamen
  // Quelle: http://de.wikipedia.org/wiki/Liste_der_h%C3%A4ufigsten_Familiennamen_in_Deutschland
  static string[] Nachnamen = { "Müller", "Schmidt", "Schneider", "Fischer", "Weber", "Meyer", "Wagner", "Becker", "Schulz", "Hoffmann", "Schäfer", "Koch", "Bauer", "Richter", "Klein", "Wolf", "Schröder", "Neumann", "Schwarz", "Zimmermann" };
  // Quelle: http://www.beliebte-vornamen.de/3467-alle-spitzenreiter.htm
  static string[] Vornamen = { "Leon", "Hannah", "Lukas", "Anna", "Leonie", "Marie", "Niklas", "Sarah", "Jan", "Laura", "Julia", "Lisa", "Kevin" };
  // Politiker-Namenskarusell :-)
  static string[] PilotenNachnamen = { "Gysi", "Stoiber", "Koch", "Steinmeyer", "Schröder", "Merkel", "Westerwelle", "Beck", "Lafontaine", "Trittin", "Roth" };
  static string[] PilotenVornamen = { "Edmund", "Olaf", "Roland", "Gerhard", "Angela", "Joschka", "Guido", "Gregor", "Kurt", "Frank-Walter", "Oskar", "Jürgen", "Claudia" };
  static int ANZFlugSet = 100;
  static int ANZPass = 100;
  static int ANZPilot = 20;
  static string[] PassagierStatus = { "A", "B", "C" };
  static Random rnd = new Random(DateTime.Now.Millisecond);
  public static void Run(int anz = 100, bool delete = true)
  {
   ANZFlugSet = anz;
   ANZPass = anz;
   ANZPilot = anz / 5;

   CUI.Headline($"Datengenerator. {ANZFlugSet} Flüge, { ANZPass} Passagiere, { ANZPilot} Piloten");

   using (var ctx = new WWWingsModell())
   {
    Console.WriteLine(ctx.Database.GetDbConnection().ConnectionString);
    Console.WriteLine("Flüge zu Beginn: " + ctx.FlugSet.Count());
    Console.WriteLine("Passagiere zu Beginn: " + ctx.PassagierSet.Count());
    Console.WriteLine("Buchungen zu Beginn: " + ctx.BuchungSet.Count());
    // Alle Buchungen löschen!
    if (delete)
    {
     ctx.Database.ExecuteSqlCommand("Delete from Buchung");
     ctx.Database.ExecuteSqlCommand("Delete from Flug");
     ctx.Database.ExecuteSqlCommand("Delete from Passagier");
     ctx.Database.ExecuteSqlCommand("Delete from Pilot");
    }
    Console.WriteLine("Flüge nach Löschen: " + ctx.FlugSet.Count());
    Console.WriteLine("Passagiere nach Löschen: " + ctx.PassagierSet.Count());
    Init_Passagiere(ctx);
    Init_Piloten(ctx);
    Init_FlugSet(ctx);
    Init_Buchungen();

    CUI.PrintSuccess("Flüge am Ende: " + ctx.FlugSet.Count());
    CUI.PrintSuccess("Passagiere am Ende: " + ctx.PassagierSet.Count());
    CUI.PrintSuccess("Buchungen am Ende: " + ctx.BuchungSet.Count());
   }
  }
  /// <summary>
  /// Menge von Flügen erzeugen
  /// </summary>
  private static Random Init_FlugSet(WWWingsModell ctx)
  {
   CUI.Headline($"Erzeuge {ANZFlugSet} Flüge...");
   var FlugSet = from f in ctx.FlugSet select f;
   int Start = 100;
   Pilot[] PilotArray = ctx.PilotSet.ToArray();
   for (int i = Start; i < (Start + ANZFlugSet); i++)
   {
    if (i % (ANZFlugSet / 10) == 0) { Console.Write("\r"); Console.Write(i * 10); }
    if (i % 100 == 0)
    {
     try
     {
      var anz = ctx.SaveChanges();
     }
     catch (Exception ex)
     {
      Console.WriteLine("FEHLER: " + ex.Message.ToString());
     }
    }
    var FlugNeu = new Flug();
    FlugNeu.FlugNr = i;
    FlugNeu.Abflugort = Flughaefen[rnd.Next(0, Flughaefen.Length - 1)];
    FlugNeu.Zielort = Flughaefen[rnd.Next(0, Flughaefen.Length - 1)]; ;
    if (FlugNeu.Abflugort == FlugNeu.Zielort) { i--; continue; } // Keine Rundflüge!
    FlugNeu.FreiePlaetze = Convert.ToInt16(new Random(i).Next(250));
    FlugNeu.Plaetze = 250;
    FlugNeu.Datum = DateTime.Now.AddDays((double)FlugNeu.FreiePlaetze).AddMinutes((double)FlugNeu.FreiePlaetze * 7);
    FlugNeu.PilotId = PilotArray[rnd.Next(PilotArray.Length - 1)].ID;
    FlugNeu.CopilotId = PilotArray[rnd.Next(PilotArray.Length - 1)].ID;
    ctx.FlugSet.Add(FlugNeu);
   }
   ctx.SaveChanges();
   Console.WriteLine("\rFlüge nach dem Einfügen: " + ctx.FlugSet.Count());
   return rnd;
  }
  private static void Init_Buchungen()
  {
   WWWingsModell ctx = new WWWingsModell();
   int k = 0;
   Random rnd5 = new Random();
   CUI.Headline($"Erzeuge {ANZFlugSet*10} Buchungen...");
   var FlugSet2 = from f in ctx.FlugSet select f;
   Passagier[] PassagierArray = ctx.PassagierSet.ToArray();
   foreach (Flug f in FlugSet2.ToList())
   {
    k++;
    if (k % (ANZFlugSet / 10) == 0) { Console.Write("\r"); Console.Write(k * 10); }
    for (int j = 1; j < 10; j++)
    {
     Random rnd2 = new Random(DateTime.Now.Millisecond);
     // Wähle Zufällig einen Passagier
     Passagier p = PassagierArray[Convert.ToInt16(rnd5.Next(PassagierArray.Length))];
     if (!ctx.BuchungSet.Any(b => b.FlugNr == f.FlugNr && b.PassagierID == p.ID))
     {
      Buchung b = null;
      try // Weil doppelte möglich sind durch Zufallsauswahl!
      {
       b = new Buchung();
       b.FlugNr = f.FlugNr;
       b.PassagierID = p.ID;
       // Buchung anlegen
       ctx.BuchungSet.Add(b);
       ctx.SaveChanges();
      }
      catch (Exception ex)
      {
       Console.WriteLine(ex.Message);
       f.BuchungSet.Remove(b);
      }
     }
    }
   }
   Console.WriteLine("\rBuchungen nach dem Einfügen: " + ctx.BuchungSet.Count());
  }
  private static void Init_Piloten(WWWingsModell ctx)
  {
   CUI.Headline($"Erzeuge {ANZPilot} Piloten...");
   for (int PNummer = 1; PNummer <= ANZPilot; PNummer++)
   {
    if (PNummer % (ANZPass / 10) == 0) { Console.Write("\r"); Console.Write(PNummer * 10); }
    string Vorname = PilotenVornamen[rnd.Next(0, PilotenVornamen.Length - 1)]; ;
    string Nachname = PilotenNachnamen[rnd.Next(0, PilotenNachnamen.Length - 1)]; ;
    Pilot p = new Pilot()
    {
     //p.PersonID = 1000 + PNummer;
     Name = Nachname,
     Vorname = Vorname,
     Geburtsdatum = new DateTime(1940, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(20000)))
    };
    //p.Einstellungsdatum = DateTime.Now.AddDays(-rnd.Next(365));
    ctx.PilotSet.Add(p);
    ctx.SaveChanges();
    //Console.WriteLine("PILOT: " + PNummer + ": " + Vorname + " " + Nachname);
   }
   Console.WriteLine("\rPiloten nach dem Einfügen: " + ctx.PilotSet.Count());
  }

  // Passagiere Anlegen
  private static void Init_Passagiere(WWWingsModell ctx)
  {
   CUI.Headline($"Erzeuge {ANZPass} Passagiere...");
   Random rnd2 = new Random();
   for (int PNummer = 1; PNummer <= ANZPass; PNummer++)
   {
    if (PNummer % (ANZPass / 10) == 0) { Console.Write("\r"); Console.Write(PNummer * 10); }
    string Vorname = Vornamen[rnd2.Next(0, Vornamen.Length - 1)]; ;
    string Nachname = Nachnamen[rnd2.Next(0, Nachnamen.Length - 1)]; ;
    Passagier p = new Passagier();
    //  p.ID = 1;
    p.Name = Nachname;
    p.Vorname = Vorname;
    p.Geburtsdatum = new DateTime(1940, 1, 1).AddDays(Convert.ToInt32(new Random(DateTime.Now.Millisecond).Next(20000)));
    //p.PersonID = PNummer;
    p.PassagierStatus = PassagierStatus.ElementAt(rnd.Next(3));
    ctx.PassagierSet.Add(p);
    ctx.SaveChanges();
    //Console.WriteLine("PASSAGIER: " + PNummer + ": " + Vorname + " " + Nachname);
   }
   Console.WriteLine("\rPassagiere nach dem Anfügen: " + ctx.PassagierSet.Count());
  }
 }
}
