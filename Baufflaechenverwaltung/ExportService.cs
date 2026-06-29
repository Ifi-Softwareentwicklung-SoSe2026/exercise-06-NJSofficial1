using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Baufflaechenverwaltung
{
    public static class ExportService
    {
        public static void ExportActiveProjects(List<Bauvorhaben> projects, string filePath)
        {
            var activeProjects = projects
                .Where(p => p.Status != VorhabenStatus.Abgeschlossen && p.Status != VorhabenStatus.Abgelehnt)
                .ToList();

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine("Bericht: Aktive Bauvorhaben");
                writer.WriteLine(new string('=', 30));
                writer.WriteLine($"Datum: {DateTime.Now:dd.MM.yyyy HH:mm}");
                writer.WriteLine();

                if (activeProjects.Count == 0)
                {
                    writer.WriteLine("Keine aktiven Bauvorhaben gefunden.");
                }
                else
                {
                    foreach (var project in activeProjects)
                    {
                        writer.WriteLine($"Vorhaben: {project.Titel}");
                        writer.WriteLine($"Status:   {project.Status}");
                        writer.WriteLine($"Antragsteller: {project.Ersteller?.Name}");
                        writer.WriteLine(new string('-', 20));
                    }
                }
            }
        }
    }
}