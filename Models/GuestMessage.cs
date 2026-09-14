namespace CyberTrace.Models
{
    public class GuestMessage
    {
        public int Id { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}