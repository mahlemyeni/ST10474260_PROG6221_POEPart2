namespace Chatbot_WPF
{
    public class TaskItem
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reminder { get; set; }
        public bool IsComplete { get; set; }
        public string CreatedAt { get; set; }

        public TaskItem()
        {
            IsComplete = false;
            CreatedAt = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}   