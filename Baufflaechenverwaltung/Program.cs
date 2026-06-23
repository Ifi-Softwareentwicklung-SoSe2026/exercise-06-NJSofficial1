using System;
using System.Collections.Generic;

namespace Baufflaechenverwaltung
{
    public enum Nutzung { Gewerbe, Landwirtschaft, Forst, Wohnnutzung, Brachfläche }
    public enum Bebaubarkeit { Ja, Nein, Auflagen }
    public enum FlaechenStatus { Frei, Reserviert, Bebaut }
    public enum VorhabenStatus { AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen }

    public class Antragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;
    }

    public class Bauflaeche
    {
        public string FlurstueckNummer { get; set; } = string.Empty;
        public double Groesse { get; set; }
        public string Lage { get; set; } = string.Empty;
        public Nutzung AktuelleNutzung { get; set; }
        public Bebaubarkeit Bebaubarkeit { get; set; }
        public string BPlanNummer { get; set; } = string.Empty;
        public decimal Bodenrichtwert { get; set; }
        public string Eigentuemer { get; set; } = string.Empty;
        public FlaechenStatus Status { get; set; } = FlaechenStatus.Frei;

        public bool BebaubarkeitPruefen()
        {
            if (Bebaubarkeit == Bebaubarkeit.Nein)
            {
                Console.WriteLine("Fläche kann nicht bebaut werden.");
                return false;
            }
            return true;
        }

        public void FlaecheReservieren()
        {
            if (Status == FlaechenStatus.Bebaut)
            {
                Console.WriteLine("Bereits bebaute Flächen können nicht reserviert werden.");
                return;
            }
            Status = FlaechenStatus.Reserviert;
        }
    }

    public class Grundstueck
    {
        public string Bezeichnung { get; set; } = string.Empty;
        public List<Bauflaeche> Flaechen { get; set; } = new List<Bauflaeche>();
    }

    public class Bauvorhaben
    {
        public string Titel { get; set; } = string.Empty;
        public Antragsteller Antragsteller { get; set; } = new Antragsteller();
        public string GeplanteNutzung { get; set; } = string.Empty;
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public VorhabenStatus Status { get; set; } = VorhabenStatus.AntragEingereicht;
        public List<Bauflaeche> ZugeordneteFlaechen { get; set; } = new List<Bauflaeche>();

        public void StatusAktualisieren(VorhabenStatus neuerStatus) => Status = neuerStatus;
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Demonstration
            var flaeche1 = new Bauflaeche
            {
                FlurstueckNummer = "0015 00012 001/002",
                Groesse = 500.0,
                Lage = "Leipzig-Nord",
                AktuelleNutzung = Nutzung.Brachfläche,
                Bebaubarkeit = Bebaubarkeit.Ja,
                BPlanNummer = "BP-2022-089",
                Bodenrichtwert = 500m,
                Eigentuemer = "Max Mustermann"
            };

            var flaeche2 = new Bauflaeche
            {
                FlurstueckNummer = "0015 00012 001/003",
                Bebaubarkeit = Bebaubarkeit.Nein,
                Status = FlaechenStatus.Bebaut
            };

            var grundstueck = new Grundstueck { Bezeichnung = "GS-Nord-1" };
            grundstueck.Flaechen.Add(flaeche1);
            grundstueck.Flaechen.Add(flaeche2);

            var vorhaben = new Bauvorhaben
            {
                Titel = "Neubau Wohnhaus",
                Antragsteller = new Antragsteller { Name = "Erika Musterfrau", Firma = "Bau GmbH" },
                GeplanteNutzung = "Wohngebäude",
                Beginn = DateTime.Now,
                Fertigstellung = DateTime.Now.AddYears(1)
            };

            // Test 1: Gültige Reservierung
            if (flaeche1.BebaubarkeitPruefen())
            {
                flaeche1.FlaecheReservieren();
                vorhaben.ZugeordneteFlaechen.Add(flaeche1);
                Console.WriteLine($"Fläche {flaeche1.FlurstueckNummer} erfolgreich reserviert.");
            }

            // Test 2: Nicht bebaubar
            Console.WriteLine("Prüfe Fläche 2 (nicht bebaubar):");
            flaeche2.BebaubarkeitPruefen();

            // Test 3: Bereits bebaut
            Console.WriteLine("Versuche Fläche 2 (bebaut) zu reservieren:");
            flaeche2.FlaecheReservieren();

            Console.WriteLine($"\nStatus der Fläche 1: {flaeche1.Status}");
            Console.WriteLine($"Status der Fläche 2: {flaeche2.Status}");
        }
    }
}