using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Input;

namespace Chatbot_WPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ChatMessage> chatMessages =
            new ObservableCollection<ChatMessage>();

        private Dictionary<string, string> userMemory =
            new Dictionary<string, string>();

        private Random random = new Random();

        private string currentTopic = "";

        private List<string> phishingTips = new List<string>()
        {
            "Never click suspicious links in emails.",
            "Verify the sender before sharing information.",
            "Scammers often pretend to be trusted companies.",
            "Look for spelling mistakes in phishing emails."
        };

        private List<string> passwordTips = new List<string>()
        {
            "Use strong passwords with symbols and numbers.",
            "Never reuse passwords across accounts.",
            "Enable two-factor authentication whenever possible.",
            "Use a password manager to store passwords safely."
        };

        private List<string> privacyTips = new List<string>()
        {
            "Review your privacy settings regularly.",
            "Avoid sharing personal information publicly.",
            "Be careful what you post online.",
            "Use strong privacy settings on social media."
        };

        public MainWindow()
        {
            InitializeComponent();

            ChatListBox.ItemsSource = chatMessages;

            AudioPlayer player = new AudioPlayer();
            player.PlayGreeting();

            AddBotMessage(
                "Welcome to the Cybersecurity Bot! What is your name?");
        }

        private void SendMessageButton_Click(object sender, RoutedEventArgs e)
        {
            ProcessMessage();
        }

        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                ProcessMessage();
            }
        }

        private void ProcessMessage()
        {
            string input = MessageTextBox.Text.Trim();

            if (string.IsNullOrEmpty(input))
                return;

            AddUserMessage(input);

            string response = GenerateResponse(input);

            AddBotMessage(response);

            MessageTextBox.Clear();

            ChatScrollViewer.ScrollToBottom();
        }

        private string GenerateResponse(string input)
        {
            string message = input.ToLower();

            // Store user name
            if (message.StartsWith("my name is"))
            {
                string name = input.Substring(11).Trim();

                userMemory["name"] = name;

                return $"Nice to meet you {name}! I'll remember your name.";
            }

            // Memory Recall
            if (message.Contains("what is my name"))
            {
                if (userMemory.ContainsKey("name"))
                {
                    return $"Your name is {userMemory["name"]}.";
                }

                return "I don't know your name yet. Tell me by saying 'My name is ...'";
            }

            // Password Recognition
            if (message.Contains("password"))
            {
                currentTopic = "password";

                return passwordTips[random.Next(passwordTips.Count)];
            }

            // Phishing Recognition
            if (message.Contains("phishing"))
            {
                currentTopic = "phishing";

                return phishingTips[random.Next(phishingTips.Count)];
            }

            // Scam Recognition
            if (message.Contains("scam"))
            {
                currentTopic = "scam";

                return "Be cautious of offers that seem too good to be true.";
            }

            // Privacy Recognition
            if (message.Contains("privacy"))
            {
                currentTopic = "privacy";

                userMemory["interest"] = "privacy";

                return privacyTips[random.Next(privacyTips.Count)];
            }

            // Conversation Flow
            if (message.Contains("tell me more") ||
                message.Contains("another tip") ||
                message.Contains("explain more"))
            {
                switch (currentTopic)
                {
                    case "password":
                        return passwordTips[random.Next(passwordTips.Count)];

                    case "phishing":
                        return phishingTips[random.Next(phishingTips.Count)];

                    case "privacy":
                        return privacyTips[random.Next(privacyTips.Count)];

                    default:
                        return "What cybersecurity topic would you like to know more about?";
                }
            }

            // Sentiment Detection
            if (message.Contains("worried"))
            {
                return "It's understandable to feel worried. Staying informed is the best way to protect yourself online.";
            }

            if (message.Contains("frustrated"))
            {
                return "Cybersecurity can seem complicated, but you're learning valuable skills to stay safe.";
            }

            if (message.Contains("curious"))
            {
                return "Curiosity is great! Learning cybersecurity helps protect you from online threats.";
            }

            // Recall user interests
            if (message.Contains("what do you remember"))
            {
                if (userMemory.ContainsKey("interest"))
                {
                    return $"I remember that you're interested in {userMemory["interest"]}.";
                }

                return "I don't remember any interests yet.";
            }

            return "I didn't quite understand that. Could you rephrase your question?";
        }

        private void AddUserMessage(string message)
        {
            chatMessages.Add(new ChatMessage
            {
                Sender = "You",
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm"),
                IsUser = true
            });
        }

        private void AddBotMessage(string message)
        {
            chatMessages.Add(new ChatMessage
            {
                Sender = "Cyber Bot",
                Message = message,
                Timestamp = DateTime.Now.ToString("HH:mm"),
                IsUser = false
            });
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            chatMessages.Clear();

            AddBotMessage(
                "Chat cleared. How can I help you stay safe online?");
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e)
        {
            AudioPlayer player = new AudioPlayer();
            player.PlayGreeting();
        }
    }
}