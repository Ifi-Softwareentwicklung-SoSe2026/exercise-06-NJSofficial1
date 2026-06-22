using System;
using System.Collections.Generic;

namespace Baufflaechenverwaltung.Models
{
    public enum Nutzung { Gewerbe, Landwirtschaft, Forst, Wohnnutzung, Brachfläche }
    public enum Bebaubarkeit { Ja, Nein, Auflagen }
    public enum FlaechenStatus { Frei, Reserviert, Bebaut }
    public enum VorhabenStatus { AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen }

    public interface IAntragsteller
    {
        string Name { get; set; }
        string Kontaktdaten { get; set; }
        string Firma { get; set; }
    }

    public interface IBauflaeche
    {
        string Flurstuecknummer { get; set; }
        double Groesse { get; set; }
        string Lage { get; set; }
        Nutzung AktuelleNutzung { get; set; }
        Bebaubarkeit Bebaubarkeit { get; set; }
        string BPlanNummer { get; set; }
        decimal Bodenrichtwert { get; set; }
        string Eigentuemer { get; set; }
        FlaechenStatus Status { get; set; }
        void ResetToDefaults();
    }

    public interface IBauvorhaben
    {
        IAntragsteller Antragsteller { get; set; }
        string GeplanteNutzung { get; set; }
        DateTime Beginn { get; set; }
        DateTime Fertigstellung { get; set; }
        VorhabenStatus Status { get; set; }
        List<IBauflaeche> ZugeordneteFlaechen { get; set; }
    }

    public class Antragsteller : IAntragsteller
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public string Firma { get; set; } = string.Empty;
    }

    public class Bauflaeche : IBauflaeche
    {
        public string Flurstuecknummer { get; set; } = string.Empty;
        public double Groesse { get; set; }
        public string Lage { get; set; } = string.Empty;
        public Nutzung AktuelleNutzung { get; set; }
        public Bebaubarkeit Bebaubarkeit { get; set; }
        public string BPlanNummer { get; set; } = string.Empty;
        public decimal Bodenrichtwert { get; set; }
        public string Eigentuemer { get; set; } = string.Empty;
        public FlaechenStatus Status { get; set; }

        public void ResetToDefaults()
        {
            Flurstuecknummer = "0000 00000 000/000";
            Groesse = 0;
            Lage = "Unbekannt";
            AktuelleNutzung = Nutzung.Brachfläche;
            Bebaubarkeit = Bebaubarkeit.Nein;
            BPlanNummer = "Keine";
            Bodenrichtwert = 0;
            Eigentuemer = "Unbekannt";
            Status = FlaechenStatus.Frei;
        }
    }

    public class Bauvorhaben : IBauvorhaben
    {
        public IAntragsteller Antragsteller { get; set; } = new Antragsteller();
        public string GeplanteNutzung { get; set; } = string.Empty;
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public VorhabenStatus Status { get; set; }
        public List<IBauflaeche> ZugeordneteFlaechen { get; set; } = new List<IBauflaeche>();
    }

    public class Grundstueck
    {
        public string Id { get; set; } = string.Empty;
        public List<IBauflaeche> Flaechen { get; set; } = new List<IBauflaeche>();
    }
}

namespace Baufflaechenverwaltung
{
    using Baufflaechenverwaltung.Models;

    class Program
    {
        static void Main(string[] args)
        {
            var flaeche = new Bauflaeche
            {
                Flurstuecknummer = "0015 00012 001/002",
                Groesse = 500,
                Lage = "Leipzig-Nord",
                AktuelleNutzung = Nutzung.Wohnnutzung,
                Bebaubarkeit = Bebaubarkeit.Ja,
                BPlanNummer = "BP-2022-089",
                Bodenrichtwert = 500m,
                Eigentuemer = "Max Mustermann",
                Status = FlaechenStatus.Frei
            };

            Console.WriteLine($"Flurstück: {flaeche.Flurstuecknummer}, Status: {flaeche.Status}");
            
            flaeche.ResetToDefaults();
            Console.WriteLine($"Nach Reset - Flurstück: {flaeche.Flurstuecknummer}, Status: {flaeche.Status}");

            var vorhaben = new Bauvorhaben
            {
                Antragsteller = new Antragsteller { Name = "Bau GmbH", Firma = "Bau GmbH" },
                GeplanteNutzung = "Wohngebäude",
                Status = VorhabenStatus.AntragEingereicht
            };
            vorhaben.ZugeordneteFlaechen.Add(flaeche);

            Console.WriteLine($"Bauvorhaben Status: {vorhaben.Status}, Flächenanzahl: {vorhaben.ZugeordneteFlaechen.Count}");
        }
    }
}