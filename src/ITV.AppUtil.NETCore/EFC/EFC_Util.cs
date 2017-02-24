using ITVisions;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Metadata;

// (C) www.IT-Visions.de - Dr. Holger Schwichtenberg

namespace ITVisions.EFCore
{
 public  static partial class EFC_Util
 {

  /// <summary>
  /// Ausgabe aller geänderten Objekte und die geänderten Properties
  /// </summary>
  /// <param name="ctx"></param>
  public static void PrintChangeInfo(DbContext ctx)
  {

   foreach (EntityEntry entry in ctx.ChangeTracker.Entries())
   {
    if (entry.State == EntityState.Modified)
    {

     CUI.Print(entry.Entity.ToString() + ": ist im Zustand " + entry.State, ConsoleColor.Yellow);
     IReadOnlyList<IProperty> listProp = entry.OriginalValues.Properties;
     PrintChangedProperties(entry);
    }
   }
  }

  public static void PrintChangedProperties(EntityEntry entry)
  {

   var dbObj = entry.GetDatabaseValues();


   //foreach (IProperty prop in entry.Properties.Where(x => x.IsModified).OfType<IProperty>())
   //{
   // Console.WriteLine(prop.Name + ": " +
   //                   entry.OriginalValues[prop] + "->" +
   //                   entry.CurrentValues[prop] + " Datenbankzustand: " + dbObj[prop]);

   //}

   foreach (PropertyEntry prop in entry.Properties.Where(x => x.IsModified))
   {
    Console.WriteLine(prop.Metadata.Name + ": " +
                      entry.OriginalValues[prop.Metadata.Name] + "->" +
                      entry.CurrentValues[prop.Metadata.Name] + " Datenbankzustand: " + dbObj[prop.Metadata.Name]);

   }
  }

  public static void PrintChangedProperties2(EntityEntry entry)
  {

   var dbObj = entry.GetDatabaseValues();

  }
 }

}