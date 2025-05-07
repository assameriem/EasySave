using System;
using System.IO;
using System.Text.Json;

namespace EasySave.Jobs
{
    public class BackupJob
    {
        public string Name { get; set; }
        public string SourcePath { get; set; }
        public string TargetPath { get; set; }
        public string Type { get; set; } // "full" or "diff"

        public BackupJob(string name, string sourcePath, string targetPath, string type)
        {
            Name = name;
            SourcePath = sourcePath;
            TargetPath = targetPath;
            Type = type.ToLower();
        }

        public void Run()
        {
            Console.WriteLine($"[EasySave] Lancement de la tâche : {Name} ({Type})");

            if (!Directory.Exists(SourcePath))
            {
                Console.WriteLine("❌ Erreur : le dossier source n'existe pas.");
                return;
            }

            if (!Directory.Exists(TargetPath))
            {
                Directory.CreateDirectory(TargetPath);
            }

            var startTime = DateTime.Now;
            long totalSize = 0;
            int copiedFiles = 0;

            try
            {
                var files = Directory.GetFiles(SourcePath, "*", SearchOption.AllDirectories);

                foreach (var sourceFile in files)
                {
                    var relativePath = Path.GetRelativePath(SourcePath, sourceFile);
                    var destFile = Path.Combine(TargetPath, relativePath);
                    var destDir = Path.GetDirectoryName(destFile);

                    if (!Directory.Exists(destDir))
                    {
                        Directory.CreateDirectory(destDir);
                    }

                    bool shouldCopy = Type == "full" ||
                                      !File.Exists(destFile) ||
                                      File.GetLastWriteTime(sourceFile) > File.GetLastWriteTime(destFile);

                    if (shouldCopy)
                    {
                        File.Copy(sourceFile, destFile, true);
                        totalSize += new FileInfo(sourceFile).Length;
                        copiedFiles++;
                    }
                }

                Console.WriteLine($"✅ Tâche \"{Name}\" exécutée ({Type}) : {copiedFiles} fichiers copiés.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de l'exécution de la tâche : {ex.Message}");
            }

            var endTime = DateTime.Now;
            var duration = (endTime - startTime).TotalSeconds;

            var log = new BackupLog
            {
                JobName = Name,
                Type = Type,
                Date = endTime,
                FilesCopied = copiedFiles,
                TotalSizeBytes = totalSize,
                DurationSeconds = duration
            };

            var logJson = JsonSerializer.Serialize(log, new JsonSerializerOptions { WriteIndented = true });

            var logDir = "/Users/matteosourdes/Library/Mobile Documents/com~apple~CloudDocs/Cesi/Prosit/EasySave/ConsoleApp2/Logs";
            if (!Directory.Exists(logDir)) Directory.CreateDirectory(logDir);

            var logFile = Path.Combine(logDir, $"{Name}_{DateTime.Now:yyyyMMdd_HHmmss}.json");

            try
            {
                File.WriteAllText(logFile, logJson);
                Console.WriteLine($"📄 Log généré : {logFile}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Erreur lors de l’écriture du fichier log : {ex.Message}");
            }
        }
    }
}