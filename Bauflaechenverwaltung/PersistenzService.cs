using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bauflaechenverwaltung;

namespace PersistenzService
{
    public class PersistenceData
    {
        public List<Bauflaeche> Flaechen { get; set; } = new List<Bauflaeche>();
        public List<Bauvorhaben> Vorhaben { get; set; } = new List<Bauvorhaben>();
    }

    public static class DataPersistenceService
    {
        private static readonly string FilePath = "data.json";

        private static readonly JsonSerializerOptions Options = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReferenceHandler = ReferenceHandler.Preserve // Verhindert doppelte JSON-Einträge für referenzierte Flächen
        };

        public static void Save(List<Bauflaeche> flaeche, List<Bauvorhaben> vorhaben)
        {
            try
            {
                var data = new PersistenceData { Flaechen = flaeche, Vorhaben = vorhaben };
                string json = JsonSerializer.Serialize(data, Options);
                File.WriteAllText(FilePath, json);
                Console.WriteLine("Daten erfolgreich gespeichert!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Speichern: {ex.Message}");
            }
        }

        public static PersistenceData Load()
        {
            try
            {
                if (!File.Exists(FilePath)) return new PersistenceData();
                
                string json = File.ReadAllText(FilePath);
                return JsonSerializer.Deserialize<PersistenceData>(json, Options) ?? new PersistenceData();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Fehler beim Laden (erstelle leeres Datenobjekt): {ex.Message}");
                return new PersistenceData();
            }
        }
    }
}