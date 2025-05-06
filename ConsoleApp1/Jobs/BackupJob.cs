using System;

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
            Type = type;
        }
    }
}