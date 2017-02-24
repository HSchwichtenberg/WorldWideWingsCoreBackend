using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using GO;
namespace DZ
{
 public class WWWingsModell : DbContext
 {
  protected override void OnConfiguring(DbContextOptionsBuilder builder)
  {

   string ConnectingString = System.Environment.GetEnvironmentVariable("WWWings_CS");

   if (string.IsNullOrEmpty(ConnectingString)) { ConnectingString = @"Server=.;Database=WWWings_EtoE_NETCore;Trusted_Connection=True;MultipleActiveResultSets=True;"; };

   // oder z.B. 
   //@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=WWWings;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=True;ApplicationIntent=ReadWrite;MultiSubnetFailover=False"

   // Provider und Connectring String festlegen!
   if (!string.IsNullOrEmpty(ConnectingString))
   {
    builder.UseSqlServer(ConnectingString).ConfigureWarnings(w => w.Ignore(RelationalEventId.AmbientTransactionWarning));
   }
   else
   {
    //builder.UseInMemoryDatabase().ConfigureWarnings(w => w.Ignore(InMemoryEventId.TransactionIgnoredWarning));
   }
  }
  public DbSet<Flug> FlugSet { get; set; }
  public DbSet<Pilot> PilotSet { get; set; }
  public DbSet<Passagier> PassagierSet { get; set; }
  public DbSet<Buchung> BuchungSet { get; set; }
  /// <summary>
  /// Methode zur Aktivierung/Deaktivierung von Konventionen
  /// sowie zur manuellen Konfiguration per Fluent-API
  /// </summary>
  protected override void OnModelCreating(ModelBuilder builder)
  {
   // ---------- Pflichtangaben
   // Primärschlüssel für erbende Klassen
   builder.Entity<Passagier>().HasKey(x => x.ID);
   builder.Entity<Pilot>().HasKey(x => x.ID);
   // Zusammengesetzter Primärschlüssel für Zwischenklassen
   builder.Entity<Buchung>().HasKey(b => new { b.FlugNr, b.PassagierID });
   // Zweifach-Beziehungen zwischen Flug und Pilot festlegen und kaskadierendes Löschen ausschalten
   builder.Entity<Pilot>().HasMany(p => p.FluegeAlsPilot).
  WithOne(p => p.Pilot).HasForeignKey(f => f.PilotId).OnDelete(DeleteBehavior.Restrict);
   builder.Entity<Pilot>().HasMany(p => p.FluegeAlsCopilot)
   .WithOne(p => p.Copilot).HasForeignKey(f => f.CopilotId).OnDelete(DeleteBehavior.Restrict);
   // Indexe festlegen
   builder.Entity<Flug>().HasIndex(f => f.Abflugort);
   builder.Entity<Flug>().HasIndex(f => f.Zielort);
   // Index auf abgeleiteten Klassen, weil die Basisklasse Person ja gar nicht explizit verwendet wird. Kann EF Core nicht anders
   builder.Entity<Passagier>().HasIndex(f => f.Vorname);
   builder.Entity<Passagier>().HasIndex(f => f.Name);
   builder.Entity<Pilot>().HasIndex(f => f.Vorname);
   builder.Entity<Pilot>().HasIndex(f => f.Name);
   // ---------- Optional
   // Massen-Konfiguration über Model-Klasse: Tabellen heißen wie Klassen und nicht wie DbSet-Properties
   foreach (IMutableEntityType entity in builder.Model.GetEntityTypes())
   {
    // Alle Tabellennamen wie bisher
    entity.Relational().TableName = entity.DisplayName();
   }
  }
 }
}