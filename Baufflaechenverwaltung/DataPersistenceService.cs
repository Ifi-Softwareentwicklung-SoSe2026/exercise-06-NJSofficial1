using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Baufflaechenverwaltung
{
    public class PersistenceData
    {
        public List<Bauflaeche> Flaechen { get; set; } = new List<Bauflaeche>();
        public List<Bauvorhaben> Vorhaben { get; set; } = new List<Bauvorhaben>();
    }

    public static class DataPersistenceService
    {
        private static readonly string FilePath = "data.json";

        public static void Save(List<Bauflaeche> flaeche, List<Bauvorhaben> vorhaben)
        {
            var data = new PersistenceData { Flaechen = flaeche, Vorhaben = vorhaben };
            string json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FilePath, json);
        }

        public static PersistenceData Load()
        {
            if (!File.Exists(FilePath)) return new PersistenceData();
            string json = File.ReadAllText(FilePath);
            return JsonSerializer.Deserialize<PersistenceData>(json) ?? new PersistenceData();
        }
    }
}