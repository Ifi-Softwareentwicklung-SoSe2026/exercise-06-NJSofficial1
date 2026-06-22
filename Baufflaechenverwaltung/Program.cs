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

        public void FlaecheReservieren() => Status = FlaechenStatus.Reserviert;
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

            var grundstueck = new Grundstueck { Bezeichnung = "GS-Nord-1" };
            grundstueck.Flaechen.Add(flaeche1);

            var vorhaben = new Bauvorhaben
            {
                Titel = "Neubau Wohnhaus",
                Antragsteller = new Antragsteller { Name = "Erika Musterfrau", Firma = "Bau GmbH" },
                GeplanteNutzung = "Wohngebäude",
                Beginn = DateTime.Now,
                Fertigstellung = DateTime.Now.AddYears(1)
            };
            vorhaben.ZugeordneteFlaechen.Add(flaeche1);
            flaeche1.FlaecheReservieren();

            Console.WriteLine($"Bauvorhaben '{vorhaben.Titel}' für Fläche {flaeche1.FlurstueckNummer} angelegt.");
            Console.WriteLine($"Status der Fläche: {flaeche1.Status}");
            Console.WriteLine($"Status des Vorhabens: {vorhaben.Status}");
        }
    }
}