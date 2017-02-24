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
 /// Geschäftslogik für Buchung, verwendet FlugManager und PassagierManager
 /// </summary>
 public class BuchungManager : EntityManagerBase<WWWingsModell, Buchung>
 {

  /// <summary>
  /// Flugbuchung erstellen, wobei dafür sowohl die eigentliche Buchung als auch die Reduzierung der Anzahl der freien Plätze in einer Transaktion erfolgen muss! Methode liefert "OK", wenn die Buchung erfolgreich war, sonst den Fehlertext.
  /// </summary>
  public string CreateBuchung(int FlugID, int PassagierID)
  {
   // Transaktion, nur erfolgreich wenn Platzanzahl reduziert und Buchung erstellt!
   using (var ctx = new WWWingsModell())
   {
    using (var transaction = ctx.Database.BeginTransaction())
    {
     try
     {
      FlugManager fm = new FlugManager(ctx);
      PassagierManager pm = new PassagierManager(ctx);

      if (!fm.ReducePlatzAnzahl(FlugID, 1))
      {
        return "Fehler: Kein Platz auf diesem Flug vorhanden!";
      }
      if (!pm.AddPassagierZuFlug(PassagierID, FlugID))
      {
       return "Fehler: Buchung nicht möglich!";
      }
      //  Transaktion erfolgreich abschließen
      transaction.Commit();

      fm.Dispose();
      pm.Dispose();
      return "OK";
     }
     catch (Exception ex)
     {
      return "Unerwarteter Fehler: " + ex.Message;
     }
     finally
     {
      Console.WriteLine("Rollback!");
     }
    } // End using Transaction
   } // End using context
  }
 }
}

