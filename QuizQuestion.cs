using System.Collections.Generic;

namespace Chatbot_WPF
{
    public class QuizQuestion
    {
        public string Question { get; set; }
        public List<string> Options { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }

        public QuizQuestion()
        {
            Options = new List<string>();
            IsTrueFalse = false;
        }
    }
} 