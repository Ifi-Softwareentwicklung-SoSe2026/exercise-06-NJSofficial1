using System;
using System.Collections.Generic;

// Kevins Implementierung
namespace Baufflaechenverwaltung
{
    public enum Nutzung { Gewerbe, Landwirtschaft, Forst, Wohnnutzung, Brachfläche }
    public enum Bebaubarkeit { Ja, Nein, Auflagen }
    public enum FlaechenStatus { Frei, Reserviert, Bebaut, Naturschutzgebiet }
    public enum VorhabenStatus { AntragEingereicht, Genehmigt, Abgelehnt, InBearbeitung, Abgeschlossen }
    public enum Rolle { Administrator, Bauamtsmitarbeiter, Antragsteller, ExternerGutachter }

    public class Beteiligter
    {
        public string Name { get; set; } = string.Empty;
        public string Kontaktdaten { get; set; } = string.Empty;
        public Rolle Rolle { get; set; }

        public Beteiligter(string name, string kontaktdaten, Rolle rolle)
        {
            Name = name;
            Kontaktdaten = kontaktdaten;
            Rolle = rolle;
        }
    }

    public class Antragsteller : Beteiligter
    {
        public string Firma { get; set; } = string.Empty;

        // Nutzt base, um Name und Kontaktdaten an 'Beteiligter' weiterzugeben
        public Antragsteller(string name, string kontaktdaten, string firma) 
            : base(name, kontaktdaten, Rolle.Antragsteller)
        {
            Firma = firma;
        }
    }

    public class Bauamtsmitarbeiter : Beteiligter
    {
        public string Dienstnummer { get; set; } = string.Empty;

        public Bauamtsmitarbeiter(string name, string kontaktdaten, string dienstnummer) 
            : base(name, kontaktdaten, Rolle.Bauamtsmitarbeiter)
        {
            Dienstnummer = dienstnummer;
        }
    }

    public class ExternerGutachter : Beteiligter
    {
        public string Fachgebiet { get; set; } = string.Empty;

        public ExternerGutachter(string name, string kontaktdaten, string fachgebiet) 
            : base(name, kontaktdaten, Rolle.ExternerGutachter)
        {
            Fachgebiet = fachgebiet;
        }
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
            if (Bebaubarkeit == Bebaubarkeit.Nein || Status == FlaechenStatus.Naturschutzgebiet)
            {
                Console.WriteLine("Fläche kann nicht bebaut werden.");
                if (Bebaubarkeit != Bebaubarkeit.Nein)
                {
                    Bebaubarkeit = Bebaubarkeit.Nein;
                }
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
        public Beteiligter Ersteller { get; set; }
        public string GeplanteNutzung { get; set; } = string.Empty;
        public DateTime Beginn { get; set; }
        public DateTime Fertigstellung { get; set; }
        public VorhabenStatus Status { get; set; } = VorhabenStatus.AntragEingereicht;
        public List<Bauflaeche> ZugeordneteFlaechen { get; set; } = new List<Bauflaeche>();

        public void StatusAktualisieren(VorhabenStatus neuerStatus) => Status = neuerStatus;

        public bool StatusAktualisierungMitPruefung(Beteiligter user, VorhabenStatus neuerStatus)
        {
            if (user.Rolle == Rolle.ExternerGutachter)
            {
                Console.WriteLine($"[ZUGRIFF VERWEIGERT] {user.Name} darf als externer Gutachter den Status nicht ändern.");
                return false;
            }

            StatusAktualisieren(neuerStatus);
            return true;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var antragsteller = new Antragsteller("Erika Musterfrau", "erika@bau-gmbh.de", "Bau GmbH");
            var gutachter = new ExternerGutachter("Dr. Umwelt", "mueller@umwelt.de", "Greenpeace");
            var bauamtsmitarbeiter = new Bauamtsmitarbeiter("Herr Amtsschimmel", "amt@amt.de", "Bauamt");

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

            var flaeche3 = new Bauflaeche
            {
                FlurstueckNummer="0015 00012 001/004",
                Bebaubarkeit = Bebaubarkeit.Ja,
                Status = FlaechenStatus.Naturschutzgebiet
            };

            var grundstueck = new Grundstueck { Bezeichnung = "GS-Nord-1" };
            grundstueck.Flaechen.Add(flaeche1);
            grundstueck.Flaechen.Add(flaeche2);

            var vorhaben = new Bauvorhaben
            {
                Titel = "Neubau Wohnhaus",
                Ersteller = antragsteller,
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
            
            // ----- Rechtetest -----
            Console.WriteLine("\n----- Zugriffstest: -----");
            Console.WriteLine($"Aktueller Vorhabenstatus: {vorhaben.Status}\n");

            Console.WriteLine($"{gutachter.Name} ({gutachter.Rolle}) versucht Änderung");
            bool gutachterErfolg = vorhaben.StatusAktualisierungMitPruefung(gutachter, VorhabenStatus.Genehmigt);
            Console.WriteLine($"Aktion erfolgreich? {gutachterErfolg}");
            Console.WriteLine($"Status des Vorhabens ist: {vorhaben.Status}\n");
            // -----

            // Test 2: Nicht bebaubar
            Console.WriteLine("Prüfe Fläche 2 (nicht bebaubar):");
            flaeche2.BebaubarkeitPruefen();

            flaeche3.BebaubarkeitPruefen();
            Console.WriteLine($"Ist Fläche 3 bebaubar? \n {flaeche3.Bebaubarkeit}");

            // Test 3: Bereits bebaut
            Console.WriteLine("Versuche Fläche 2 (bebaut) zu reservieren:");
            flaeche2.FlaecheReservieren();

            Console.WriteLine($"\nStatus der Fläche 1: {flaeche1.Status}");
            Console.WriteLine($"Status der Fläche 2: {flaeche2.Status}");
        }
    }
}