using System;
using System.Collections.Generic;

namespace Chatbot_WPF
{
    public class QuizManager
    {
        private List<QuizQuestion> _questions;
        private int _currentIndex = 0;
        private int _score = 0;
        private ActivityLogger _logger;

        public QuizManager(ActivityLogger logger = null)
        {
            _logger = logger;
            InitializeQuestions();
        }

        private void InitializeQuestions()
        {
            _questions = new List<QuizQuestion>
            {
                // Phishing Questions
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Options = new List<string> { "A) Reply with your password", "B) Delete the email", "C) Report the email as phishing", "D) Ignore it" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! Reporting phishing emails helps prevent scams. Never share passwords via email.",
                    IsTrueFalse = false
                },

                new QuizQuestion
                {
                    Question = "Phishing emails always contain spelling mistakes and poor grammar.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. Modern phishing emails can be well-written and professional-looking. Always verify the sender.",
                    IsTrueFalse = true
                },

                new QuizQuestion
                {
                    Question = "Which of these is NOT a sign of a phishing email?",
                    Options = new List<string> { "A) Urgent language requesting immediate action", "B) A professional company logo", "C) Links that don't match the stated URL", "D) Requests for personal information" },
                    CorrectAnswer = "B",
                    Explanation = "Professional logos can be faked. Phishing emails often mimic legitimate companies. Check the sender's email address and hover over links.",
                    IsTrueFalse = false
                },

                // Password Safety Questions
                new QuizQuestion
                {
                    Question = "A strong password should be at least how many characters long?",
                    Options = new List<string> { "A) 6 characters", "B) 8 characters", "C) 12 characters", "D) Any length works" },
                    CorrectAnswer = "C",
                    Explanation = "Correct! 12+ characters with a mix of uppercase, lowercase, numbers, and symbols is recommended.",
                    IsTrueFalse = false
                },

                new QuizQuestion
                {
                    Question = "It is safe to reuse the same strong password across multiple accounts.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. Never reuse passwords. If one account is breached, all accounts with that password are at risk.",
                    IsTrueFalse = true
                },

                new QuizQuestion
                {
                    Question = "Which password is the strongest?",
                    Options = new List<string> { "A) password123", "B) MyP@ssw0rd!2024", "C) 12345678", "D) admin" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! 'MyP@ssw0rd!2024' contains uppercase, lowercase, numbers, symbols, and is 15 characters long.",
                    IsTrueFalse = false
                },

                // Safe Browsing Questions
                new QuizQuestion
                {
                    Question = "What does the 'S' in HTTPS stand for?",
                    Options = new List<string> { "A) Secure", "B) Server", "C) Standard", "D) Simple" },
                    CorrectAnswer = "A",
                    Explanation = "Correct! HTTPS encrypts data between your browser and the website, protecting your information.",
                    IsTrueFalse = false
                },

                new QuizQuestion
                {
                    Question = "It is safe to enter your credit card information on public Wi-Fi networks.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. Public Wi-Fi is not encrypted. Hackers can intercept unencrypted data. Use a VPN or wait until you're on secure Wi-Fi.",
                    IsTrueFalse = true
                },

                // Social Engineering
                new QuizQuestion
                {
                    Question = "Social engineering is primarily a technical attack that requires hacking skills.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "False",
                    Explanation = "False. Social engineering manipulates people into revealing information. It relies on psychology, not technical skills.",
                    IsTrueFalse = true
                },

                new QuizQuestion
                {
                    Question = "A caller claims to be from IT support and asks you to reset your password immediately.",
                    Options = new List<string> { "A) Reset immediately", "B) Hang up and call IT directly from your records", "C) Give them temporary access", "D) Provide your username" },
                    CorrectAnswer = "B",
                    Explanation = "Correct! Never share information with unsolicited callers. Verify their identity through official channels.",
                    IsTrueFalse = false
                },

                // Two-Factor Authentication
                new QuizQuestion
                {
                    Question = "Two-factor authentication requires two different types of verification to access an account.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "True! Examples include password + SMS code, password + authenticator app, or password + biometric.",
                    IsTrueFalse = true
                },

                // Malware
                new QuizQuestion
                {
                    Question = "Ransomware is a type of malware that encrypts your files and demands payment for decryption.",
                    Options = new List<string> { "True", "False" },
                    CorrectAnswer = "True",
                    Explanation = "True! Ransomware can be devastating. Regular backups and updates are your best defense.",
                    IsTrueFalse = true
                },

                // Privacy
                new QuizQuestion
                {
                    Question = "Which of these is the best way to protect your privacy online?",
                    Options = new List<string> { "A) Limit what you share publicly", "B) Review privacy settings regularly", "C) Use strong, unique passwords", "D) All of the above" },
                    CorrectAnswer = "D",
                    Explanation = "Correct! All are essential. Good privacy requires multiple layers of protection.",
                    IsTrueFalse = false
                }
            };
        }

        /// <summary>
        /// Get the current question
        /// </summary>
        public QuizQuestion GetCurrentQuestion()
        {
            if (_currentIndex < _questions.Count)
                return _questions[_currentIndex];
            return null;
        }

        /// <summary>
        /// Submit an answer and check if correct
        /// </summary>
        public bool SubmitAnswer(string userAnswer)
        {
            if (_currentIndex >= _questions.Count)
                return false;

            var currentQuestion = _questions[_currentIndex];
            bool isCorrect = userAnswer.ToUpper() == currentQuestion.CorrectAnswer.ToUpper();

            if (isCorrect)
                _score++;

            _currentIndex++;
            return isCorrect;
        }

        /// <summary>
        /// Get feedback for an answer
        /// </summary>
        public string GetFeedback(bool correct)
        {
            if (_currentIndex == 0)
                return "Start the quiz first.";

            var question = _questions[_currentIndex - 1];
            string result = correct ? "✓ Correct!" : "✗ Incorrect.";
            return $"{result} {question.Explanation}";
        }

        /// <summary>
        /// Check if quiz is finished
        /// </summary>
        public bool IsFinished()
        {
            return _currentIndex >= _questions.Count;
        }

        /// <summary>
        /// Get the final score
        /// </summary>
        public string GetFinalScore()
        {
            return $"{_score} out of {_questions.Count}";
        }

        /// <summary>
        /// Get final message based on score
        /// </summary>
        public string GetFinalMessage()
        {
            double percentage = (_score / (double)_questions.Count) * 100;

            if (percentage >= 80)
                return $"Great job! You scored {GetFinalScore()} ({percentage:F0}%). You have excellent cybersecurity knowledge!";
            else if (percentage >= 60)
                return $"Good effort! You scored {GetFinalScore()} ({percentage:F0}%). Keep learning to improve your security awareness.";
            else
                return $"You scored {GetFinalScore()} ({percentage:F0}%). Keep practicing and learning about cybersecurity!";
        }

        /// <summary>
        /// Reset the quiz
        /// </summary>
        public void ResetQuiz()
        {
            _currentIndex = 0;
            _score = 0;
        }

        /// <summary>
        /// Get total number of questions
        /// </summary>
        public int GetTotalQuestions()
        {
            return _questions.Count;
        }

        /// <summary>
        /// Get current question number
        /// </summary>
        public int GetCurrentQuestionNumber()
        {
            return _currentIndex + 1;
        }
    }
}