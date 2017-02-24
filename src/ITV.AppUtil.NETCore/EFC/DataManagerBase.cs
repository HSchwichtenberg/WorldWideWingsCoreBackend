using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using ITVisions.EFCore;

namespace ITVisions.EFC
{

 /// <summary>
 /// Basisklasse für alle Datenmanager  / mit EF 6.x
 /// </summary>
 abstract public class DataManagerBase<TDbContext> : IDisposable
  where TDbContext : DbContext, new()
 {
  // Eine Instanz des Framework-Kontextes pro Manager-Instanz
  protected TDbContext ctx;
  protected bool disposeContext = true;
  protected bool tracking = false;

  protected DataManagerBase(bool tracking) : this(null, tracking)
  {
  }

  public void StartTracking()
  {
   ctx.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.TrackAll;
  }

  public void SetTracking()
  {
   if (tracking) ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.TrackAll;
   else ctx.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
  }
  
  public DataManagerBase()
  {
   this.ctx = new TDbContext();
  }
  protected DataManagerBase(TDbContext kontext = null, bool tracking = false)
  {
   this.tracking = tracking;
   // Falls ein Kontext hineingereicht wurde, nehme diesen!
   if (kontext != null) { this.ctx = kontext; disposeContext = false; }
   else
   {
    this.ctx = new TDbContext();
   }

   // Einstellung für Tracking
  SetTracking();
  }

  /// <summary>
  /// DataManager vernichten (vernichtet auch den EF-Kontext)
  /// </summary>
  public void Dispose()
  {
   // Falls der Kontext von außen hineingereicht wurde, darf man nicht Dispose() aufrufen.
   // Das ist dann Sache des Aufrufers
   if (disposeContext) ctx.Dispose();
  }


  /// <summary>
  /// Aufruf von SaveChanges() zur Lebenszeit des Kontextes (für Änderungen an attached objects)
  /// Rückgabewert ist eine Zeichenkette, die Informationen über die Anzahl der neuen, geänderten und gelöschten Datensätze enthält
  /// </summary>
  /// <returns></returns>
  public string Save()
  {
   string ergebnis = GetStatistik();
   var anz = ctx.SaveChanges();
   return ergebnis;
  }

  /// <summary>
  /// Speichern für losgelöste (detached) Entitätsobjekte mit Autowert-Primärschlüssel namens ID
  /// Die neu hinzugefügten Objekte muss die Speichern-Routine wieder zurückgeben, da die IDs für die 
  /// neuen Objekte erst beim Speichern von der Datenbank vergeben werden
  /// </summary>
  protected List<TEntity> Save<TEntity>(IEnumerable<TEntity> menge, out string Statistik)
  where TEntity : class
  {
   StartTracking();
   var neue = new List<TEntity>();

   // Änderungen für jeden einzelnen Passagier übernehmen
   foreach (dynamic o in menge)
   {
    // Anfügen an diesen Kontext
    ctx.Set<TEntity>().Attach((TEntity)o);
    // Unterscheidung anhand des Primärschlüssels bei Autorwerten
    // 0 -> neu
    if (o.ID == 0)
    {
     ctx.Entry(o).State = EntityState.Added;
     if (o.ID < 0) o.ID = 0; // Notwendiger Hack, weil EF nach dem Added eine große negative Zahl in ID schreibt und das als Schlüssel betrachtet :-(
     // Neue Datensätze merken, da diese nach Speichern zurückgegeben werden müssen (haben dann erst ihre IDs!)
     neue.Add(o);
    }
    else
    {
     // nicht 0 -> geändert
     ctx.Entry(o).State = EntityState.Modified;
    }
    SetTracking();
   }

   // Statistik der Änderungen zusammenstellen
   Statistik = GetStatistik<TEntity>();
   //ctx.Log();
   // Änderungen speichern
   var e = ctx.SaveChanges();

   return neue;
  }



  /// <summary>
  /// Speichern für losgelöste Entitätsobjekte mit EntityState-Property
  /// Die neu hinzugefügten Objekte muss die Speichern-Routine wieder zurückgeben, da die IDs für die 
  /// neuen Objekte erst beim Speichern von der Datenbank vergeben werden
  /// </summary>
  protected List<TEntity> SaveEx<TEntity>(IEnumerable<TEntity> menge, out string Statistik)
where TEntity : class
  {
   StartTracking();
   var neue = new List<TEntity>();

   // Änderungen für jeden einzelne Objekt aus dessen EntityState übernehmen
   foreach (dynamic o in menge)
   {

    if (o.EntityState == ITVEntityState.Added)
    {

     ctx.Entry(o).State = EntityState.Added;
     neue.Add(o);
    }
    if (o.EntityState == ITVEntityState.Deleted)
    {
     ctx.Set<TEntity>().Attach((TEntity)o);
     ctx.Set<TEntity>().Remove(o);
    }
    if (o.EntityState == ITVEntityState.Modified)
    {
     ctx.Set<TEntity>().Attach((TEntity)o);
     ctx.Entry(o).State = EntityState.Modified;
    }
   }

   // Statistik der Änderungen zusammenstellen
   Statistik = GetStatistik<TEntity>();

   // Änderungen speichern
   var e = ctx.SaveChanges();

   SetTracking();
   return neue;
  }


  /// <summary>
  /// Liefert Informationen über ChangeTracker-Status als Zeichenkette
  /// </summary>
  protected string GetStatistik<TEntity>()
where TEntity : class
  {
   string Statistik = "";
   Statistik += "Geaendert: " + ctx.ChangeTracker.Entries<TEntity>().Where(x => x.State == EntityState.Modified).Count();
   Statistik += " Neu: " + ctx.ChangeTracker.Entries<TEntity>().Where(x => x.State == EntityState.Added).Count();
   Statistik += " Geloescht: " + ctx.ChangeTracker.Entries<TEntity>().Where(x => x.State == EntityState.Deleted).Count();
   return Statistik;
  }

  /// <summary>
  /// Liefert Informationen über ChangeTracker-Status als Zeichenkette
  /// </summary>
  protected string GetStatistik()
  {
   string Statistik = "";
   Statistik += "Geändert: " + ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Modified).Count();
   Statistik += " Neu: " + ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Added).Count();
   Statistik += " Gelöscht: " + ctx.ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted).Count();
   return Statistik;
  }
 }
}
