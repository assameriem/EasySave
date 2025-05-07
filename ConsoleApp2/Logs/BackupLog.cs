namespace EasySave.Jobs
{
    public class BackupLog
    {
        public string JobName { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int FilesCopied { get; set; }
        public long TotalSizeBytes { get; set; }
        public double DurationSeconds { get; set; }
    }
}