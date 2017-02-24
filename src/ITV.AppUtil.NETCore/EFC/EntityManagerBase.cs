using Microsoft.EntityFrameworkCore;

using System.Linq;

namespace ITVisions.EFC
{
 /// <summary>
 /// Basisklasse für alle Datenmanager zur Verwaltung eines bestimmten Entitätstyps, auch wenn diese detached sind!  / mit EFC 1.1
 /// V1.2
 /// Annahme: Es gibt immer nur eine Primärschlüsselspalte!
 /// </summary>
 public abstract class EntityManagerBase<TDbContext, TEntity> : DataManagerBase<TDbContext>
  where TDbContext : DbContext, new()
  where TEntity : class
 {
  public EntityManagerBase() : base(false)
  {

  }
  public EntityManagerBase(bool tracking) : base(tracking)
  {

  }
  protected EntityManagerBase(TDbContext kontext = null, bool tracking = false) : base(kontext, tracking)
  {

  }
  /// <summary>
  /// Holt Objekt anhand des Primärschlüssels
  /// </summary>
  /// <param name="id">Primärschlüsselwert</param>
  /// <returns></returns>
  public virtual TEntity GetByID(object id)
  {
   return ctx.Set<TEntity>().Find(id);
  }

  /// <summary>
  /// Speichert geändertes Objekt
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public TEntity Update(TEntity obj)
  {
   if (!this.tracking) this.StartTracking(); // Tracking kurzzeitig einschalten
   ctx.Set<TEntity>().Attach(obj);
   ctx.Entry(obj).State = EntityState.Modified;
   ctx.SaveChanges();
   this.SetTracking();
   return obj;
  }

  /// <summary>
  /// Ergänzt ein neues Objekt
  /// </summary>
  /// <param name="obj">das neue Objekt</param>
  /// <returns></returns>
  public TEntity New(TEntity obj)
  {
   if (!this.tracking) this.StartTracking(); // Tracking kurzzeitig einschalten
   ctx.Set<TEntity>().Add(obj);
   ctx.SaveChanges();
   this.SetTracking();
   return obj;
  }

  /// <summary>
  /// Prüft, ob ein Objekt schon im lokalen Cache ist
  /// </summary>
  /// <param name="obj"></param>
  /// <returns></returns>
  public bool IsLoaded(TEntity obj)
  {
   return ctx.Set<TEntity>().Local.Any(e => e == obj);
  }

  /// <summary>
  /// Löscht Objekt anhand des Primärschlüssels
  /// </summary>
  /// <param name="id"></param>
  public virtual void Remove(object id)
  {
   if (!this.tracking) this.StartTracking(); // Tracking kurzzeitig einschalten
   TEntity obj = ctx.Set<TEntity>().Find(id);
   Remove(obj);
   this.SetTracking();
  }

  /// <summary>
  /// Löscht Objekt 
  /// </summary>
  public bool Remove(TEntity obj)
  {
   if (!this.tracking) this.StartTracking(); // Tracking kurzzeitig einschalten
   if (!this.IsLoaded(obj)) ctx.Set<TEntity>().Attach(obj);
   ctx.Set<TEntity>().Remove(obj);
   ctx.SaveChanges();
   this.SetTracking();
   return true;
  }
 }

}