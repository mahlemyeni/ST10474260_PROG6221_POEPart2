using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace Chatbot_WPF
{
    public partial class MainWindow : Window
    {
        private ObservableCollection<ChatMessage> chatMessages = new ObservableCollection<ChatMessage>();
        private ObservableCollection<TaskItem> taskList = new ObservableCollection<TaskItem>();
        private Dictionary<string, string> userMemory = new Dictionary<string, string>();
        private Random random = new Random();

        private TaskManager _taskManager;
        private ActivityLogger _activityLogger;
        private QuizManager _quizManager;

        private string currentTopic = "";
        private string awaitingReminderResponse = "";
        private bool _isInQuiz = false;
        private bool _quizAnswerSubmitted = false;

        // Cybersecurity tips
        private readonly Dictionary<string, List<string>> tips = new Dictionary<string, List<string>>
        {
            { "password", new List<string> { "Use strong passwords with symbols and numbers.", "Never reuse passwords across accounts.", "Enable two-factor authentication whenever possible.", "Use a password manager to store passwords safely." } },
            { "phishing", new List<string> { "Never click suspicious links in emails.", "Verify the sender before sharing information.", "Scammers often pretend to be trusted companies.", "Look for spelling mistakes in phishing emails." } },
            { "privacy", new List<string> { "Review your privacy settings regularly.", "Avoid sharing personal information publicly.", "Be careful what you post online.", "Use strong privacy settings on social media." } }
        };

        public MainWindow()
        {
            InitializeComponent();

            _activityLogger = new ActivityLogger();
            _taskManager = new TaskManager(_activityLogger);
            _quizManager = new QuizManager(_activityLogger);

            ChatListBox.ItemsSource = chatMessages;
            TaskListBox.ItemsSource = taskList;

            LoadTasksFromDatabase();
            new AudioPlayer().PlayGreeting();
            _activityLogger.Log("Application started");
            AddBotMessage("Welcome to the Cybersecurity Bot! What is your name?");
        }

        // ========== MESSAGE HANDLING ==========
        private void SendMessageButton_Click(object sender, RoutedEventArgs e) => ProcessMessage();
        private void MessageTextBox_KeyDown(object sender, KeyEventArgs e) { if (e.Key == Key.Enter) ProcessMessage(); }

        private void ProcessMessage()
        {
            string input = MessageTextBox.Text.Trim();
            if (string.IsNullOrEmpty(input)) return;

            AddUserMessage(input);

            string response = _isInQuiz ? ProcessQuizInput(input) :
                            !string.IsNullOrEmpty(awaitingReminderResponse) ? ProcessReminderResponse(input) :
                            GenerateResponse(input);

            AddBotMessage(response);
            MessageTextBox.Clear();
            ChatScrollViewer.ScrollToBottom();
        }

        // ========== NLP & RESPONSE GENERATION ==========
        private string GenerateResponse(string input)
        {
            string msg = input.ToLower();

            // INTENT DETECTION (Task, Reminder, Quiz, Log)
            if (msg.Contains("add task") || msg.Contains("add a task") || msg.Contains("create task") ||
                msg.Contains("i need to") || (msg.Contains("enable") && (msg.Contains("task") || msg.Contains("security"))))
            {
                awaitingReminderResponse = ExtractTaskName(input);
                _activityLogger.Log($"NLP recognised task intent from: '{input}'");
                return $"Task added: '{awaitingReminderResponse}'. Would you like to set a reminder?";
            }

            if (msg.Contains("remind me") || msg.Contains("reminder") || msg.Contains("set a reminder") || msg.Contains("don't forget"))
            {
                string reminder = ExtractReminderTime(input);
                _activityLogger.Log($"Reminder set: '{reminder}'");
                return $"Got it! I'll remind you {reminder}";
            }

            if (msg.Contains("start quiz") || msg.Contains("take quiz") || msg.Contains("test my knowledge") || msg.Contains("quiz me") || msg.Contains("play the game"))
            {
                _isInQuiz = true;
                _quizManager.ResetQuiz();
                _activityLogger.Log("Quiz started");
                return FormatQuizQuestion(_quizManager.GetCurrentQuestion());
            }

            if (msg.Contains("show activity log") || msg.Contains("what have you done") || msg.Contains("show log") || msg.Contains("recent actions"))
            {
                _activityLogger.Log("Activity log viewed");
                return _activityLogger.GetRecentLog(10);
            }

            // USER INFO
            if (msg.StartsWith("my name is"))
            {
                userMemory["name"] = input.Substring(10).Trim();
                _activityLogger.Log($"User name: {userMemory["name"]}");
                return $"Nice to meet you {userMemory["name"]}! I'll remember your name.";
            }

            if (msg.Contains("what is my name"))
                return userMemory.ContainsKey("name") ? $"Your name is {userMemory["name"]}." : "I don't know your name yet.";

            // KEYWORD RECOGNITION
            foreach (var keyword in tips.Keys)
            {
                if (msg.Contains(keyword))
                {
                    currentTopic = char.ToUpper(keyword[0]) + keyword.Substring(1);
                    if (keyword == "privacy") userMemory["interest"] = "Privacy";
                    _activityLogger.Log($"Keyword matched: {keyword}");
                    return tips[keyword][random.Next(tips[keyword].Count)];
                }
            }

            if (msg.Contains("scam")) { currentTopic = "Scam"; return "Be cautious of offers that seem too good to be true."; }
            if (msg.Contains("malware") || msg.Contains("ransomware")) { currentTopic = "Malware"; return "Keep your antivirus updated and avoid suspicious downloads."; }
            if (msg.Contains("2fa") || msg.Contains("two factor")) { currentTopic = "2FA"; return "Two-factor authentication adds extra security to accounts."; }

            // SENTIMENT
            if (msg.Contains("worried")) return "It's understandable to feel worried. Staying informed protects you.";
            if (msg.Contains("frustrated")) return "Cybersecurity seems complicated, but you're learning valuable skills.";
            if (msg.Contains("curious")) return "Curiosity is great! Learning helps protect you from threats.";

            // FOLLOW-UP
            if (msg.Contains("tell me more") || msg.Contains("another tip") || msg.Contains("explain more"))
            {
                if (tips.ContainsKey(currentTopic.ToLower()))
                    return tips[currentTopic.ToLower()][random.Next(tips[currentTopic.ToLower()].Count)];
                return "What cybersecurity topic interests you?";
            }

            if (msg.Contains("what do you remember"))
                return userMemory.ContainsKey("interest") ? $"You're interested in {userMemory["interest"]}." : "I don't remember interests yet.";

            _activityLogger.Log($"Unrecognized: {input}");
            return "I didn't understand that. Try 'add task', 'start quiz', or ask about cybersecurity.";
        }

        // ========== QUIZ HANDLING ==========
        private string ProcessQuizInput(string input)
        {
            if (!_quizAnswerSubmitted)
            {
                bool isCorrect = _quizManager.SubmitAnswer(input);
                _quizAnswerSubmitted = true;
                return _quizManager.GetFeedback(isCorrect);
            }

            _quizAnswerSubmitted = false;
            if (_quizManager.IsFinished())
            {
                _isInQuiz = false;
                _activityLogger.Log($"Quiz completed: {_quizManager.GetFinalScore()}");
                return _quizManager.GetFinalMessage();
            }

            return FormatQuizQuestion(_quizManager.GetCurrentQuestion());
        }

        // ========== REMINDER HANDLING ==========
        private string ProcessReminderResponse(string input)
        {
            string msg = input.ToLower();
            string taskTitle = awaitingReminderResponse;

            if (msg.Contains("yes") || msg.Contains("yeah") || msg.Contains("sure"))
            {
                awaitingReminderResponse = ""; // Flag for reminder time
                return "When would you like a reminder? (e.g., 'in 3 days', 'tomorrow')";
            }

            if (msg.Contains("no") || msg.Contains("nope") || msg.Contains("skip"))
            {
                awaitingReminderResponse = "";
                _taskManager.AddTask(taskTitle, taskTitle, "");
                RefreshTaskList();
                return $"Task '{taskTitle}' added (no reminder).";
            }

            // Capture reminder time
            string reminderTime = ExtractReminderTime(input);
            awaitingReminderResponse = "";
            _taskManager.AddTask(taskTitle, taskTitle, reminderTime);
            RefreshTaskList();
            return $"Task '{taskTitle}' added with reminder for {reminderTime}!";
        }

        // ========== HELPER METHODS ==========
        private string FormatQuizQuestion(QuizQuestion q)
        {
            if (q == null) return "Quiz error.";
            string result = $"\n[Q{_quizManager.GetCurrentQuestionNumber()}/{_quizManager.GetTotalQuestions()}]\n\n{q.Question}\n\n";
            if (!q.IsTrueFalse) result += string.Join("\n", q.Options);
            else result += "A) True\nB) False";
            return result + "\n\nAnswer: A, B, C, or D";
        }

        private string ExtractTaskName(string input)
        {
            string task = input.Replace("add task", "").Replace("add a task", "").Replace("create task", "")
                              .Replace("i need to", "").Replace("enable", "").Trim();
            return string.IsNullOrEmpty(task) ? "Cybersecurity Task" : char.ToUpper(task[0]) + task.Substring(1);
        }

        private string ExtractReminderTime(string input)
        {
            if (input.Contains("tomorrow")) return "tomorrow";
            if (input.Contains("today")) return "today";
            if (input.Contains("week")) return "next week";
            if (input.Contains("day")) return input;
            return "soon";
        }

        private void AddUserMessage(string message) => chatMessages.Add(new ChatMessage
        {
            Sender = "You",
            Message = message,
            Timestamp = DateTime.Now.ToString("HH:mm"),
            IsUser = true
        });

        private void AddBotMessage(string message) => chatMessages.Add(new ChatMessage
        {
            Sender = "Cyber Bot",
            Message = message,
            Timestamp = DateTime.Now.ToString("HH:mm"),
            IsUser = false
        });

        // ========== TASK MANAGEMENT ==========
        private void LoadTasksFromDatabase()
        {
            try { taskList.Clear(); _taskManager.GetAllTasks().ForEach(t => taskList.Add(t)); }
            catch { }
        }

        private void RefreshTaskList() => LoadTasksFromDatabase();

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            string title = TaskTitleTextBox.Text.Trim();
            if (string.IsNullOrEmpty(title)) { MessageBox.Show("Enter a task title."); return; }

            _taskManager.AddTask(title, TaskDescriptionTextBox.Text.Trim(), TaskReminderTextBox.Text.Trim());
            RefreshTaskList();
            TaskTitleTextBox.Clear();
            TaskDescriptionTextBox.Clear();
            TaskReminderTextBox.Clear();
            MessageBox.Show($"Task '{title}' added!");
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem task)
            {
                _taskManager.MarkAsComplete(task.Id);
                RefreshTaskList();
                MessageBox.Show($"Task '{task.Title}' completed!");
            }
            else MessageBox.Show("Select a task first.");
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TaskListBox.SelectedItem is TaskItem task)
            {
                _taskManager.DeleteTask(task.Id);
                RefreshTaskList();
                MessageBox.Show($"Task '{task.Title}' deleted!");
            }
            else MessageBox.Show("Select a task first.");
        }

        private void ShowFullLogButton_Click(object sender, RoutedEventArgs e)
        {
            AddBotMessage(_activityLogger.GetFullLog());
        }

        // ========== UTILITY BUTTONS ==========
        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            chatMessages.Clear();
            _activityLogger.Log("Chat cleared");
            AddBotMessage("Chat cleared. How can I help you stay safe?");
        }

        private void VoiceButton_Click(object sender, RoutedEventArgs e) => new AudioPlayer().PlayGreeting();
    }
}
