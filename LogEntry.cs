namespace Chatbot_WPF
{
    public class LogEntry
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }

        public LogEntry()
        {
            CreatedAt = System.DateTime.Now.ToString("[HH:mm:ss]");
        }
    }
}