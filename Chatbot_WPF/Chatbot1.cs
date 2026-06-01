using System;

namespace Chatbot_WPF
{
    public class ChatMessage
    {
        public string Sender { get; set; }
        public string Message { get; set; }
        public string Timestamp { get; set; }
        public bool IsUser { get; set; }
    }
}