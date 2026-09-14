namespace CyberTrace.Models
{
    public class ScanResult
    {
        public int Id { get; set; }

        public string TargetUrl { get; set; } = string.Empty;

        public DateTime ScanDate { get; set; }

        public bool IsSafe { get; set; }

        public List<Vulnerability> Vulnerabilities { get; set; } = new();
    }
}