using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DZ;
using GO;

namespace DZ.Migrations
{
    [DbContext(typeof(WWWingsModell))]
    partial class WWWingsModellModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GO.Buchung", b =>
                {
                    b.Property<int>("FlugNr");

                    b.Property<int>("PassagierID");

                    b.HasKey("FlugNr", "PassagierID");

                    b.HasIndex("PassagierID");

                    b.ToTable("Buchung");
                });

            modelBuilder.Entity("GO.Flug", b =>
                {
                    b.Property<int>("FlugNr");

                    b.Property<string>("Abflugort")
                        .HasMaxLength(50);

                    b.Property<int>("CopilotId");

                    b.Property<DateTime>("Datum");

                    b.Property<short?>("FreiePlaetze");

                    b.Property<string>("Memo");

                    b.Property<bool>("NichtRaucherFlug");

                    b.Property<int>("PilotId");

                    b.Property<short?>("Plaetze");

                    b.Property<decimal>("Preis");

                    b.Property<string>("Zielort")
                        .HasMaxLength(50);

                    b.HasKey("FlugNr");

                    b.HasIndex("Abflugort");

                    b.HasIndex("CopilotId");

                    b.HasIndex("PilotId");

                    b.HasIndex("Zielort");

                    b.ToTable("Flug");
                });

            modelBuilder.Entity("GO.Passagier", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EMail");

                    b.Property<byte[]>("Foto");

                    b.Property<DateTime?>("Geburtsdatum");

                    b.Property<DateTime?>("KundeSeit");

                    b.Property<string>("Land");

                    b.Property<string>("Memo");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("PassagierStatus");

                    b.Property<string>("Stadt");

                    b.Property<string>("Strasse");

                    b.Property<string>("Vorname")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("Name");

                    b.HasIndex("Vorname");

                    b.ToTable("Passagier");
                });

            modelBuilder.Entity("GO.Pilot", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("EMail");

                    b.Property<int>("Fluglizenztyp");

                    b.Property<DateTime>("FlugscheinSeit");

                    b.Property<string>("Flugschule");

                    b.Property<int?>("Flugstunden");

                    b.Property<byte[]>("Foto");

                    b.Property<DateTime?>("Geburtsdatum");

                    b.Property<string>("Land");

                    b.Property<string>("Memo");

                    b.Property<string>("Name")
                        .HasMaxLength(50);

                    b.Property<string>("Stadt");

                    b.Property<string>("Strasse");

                    b.Property<string>("Vorname")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("Name");

                    b.HasIndex("Vorname");

                    b.ToTable("Pilot");
                });

            modelBuilder.Entity("GO.Buchung", b =>
                {
                    b.HasOne("GO.Flug", "Flug")
                        .WithMany("BuchungSet")
                        .HasForeignKey("FlugNr")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("GO.Passagier", "Passagier")
                        .WithMany("BuchungSet")
                        .HasForeignKey("PassagierID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("GO.Flug", b =>
                {
                    b.HasOne("GO.Pilot", "Copilot")
                        .WithMany("FluegeAlsCopilot")
                        .HasForeignKey("CopilotId");

                    b.HasOne("GO.Pilot", "Pilot")
                        .WithMany("FluegeAlsPilot")
                        .HasForeignKey("PilotId");
                });
        }
    }
}
