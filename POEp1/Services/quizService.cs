using System;
using System.Collections.Generic;

namespace POEp1.Services
{
    public class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string[] Options { get; set; }
        public string CorrectOption { get; set; }
        public string Explanation { get; set; }

        public QuizQuestion(string text, string[] opts, string correct, string exp)
        {
            QuestionText = text;
            Options = opts;
            CorrectOption = correct;
            Explanation = exp;
        }
    }

    public static class QuizService
    {
        private static List<QuizQuestion> _questions = new List<QuizQuestion>();
        private static int _currentIndex = 0;
        private static int _score = 0;
        public static bool IsQuizRunning { get; private set; } = false;

        static QuizService()
        {
            _questions.Add(new QuizQuestion("What should you do if you receive an email asking for your account credentials?", new[] { "A) Reply immediately", "B) Delete it", "C) Report email as phishing", "D) Ignore it" }, "C", "Reporting phishing vectors empowers baseline filtering mitigations."));
            _questions.Add(new QuizQuestion("True or False: Using the exact same password across multiple servers is safe if it contains complex symbols.", new[] { "A) True", "B) False" }, "B", "Credential stuffing threats mean a breach on one site exposes all synchronized profiles."));
            _questions.Add(new QuizQuestion("Which protocol indicates data transmission streams are actively encrypted online?", new[] { "A) HTTP", "B) FTP", "C) HTTPS", "D) SMTP" }, "C", "HTTPS uses TLS structures to protect data-in-transit."));
            _questions.Add(new QuizQuestion("What is the safest option for managing multiple complex structural passwords?", new[] { "A) Write them down in text files", "B) Use a dedicated Password Manager", "C) Re-use a variations of your birth date", "D) Disable local authentication tracking entirely" }, "B", "Dedicated managers generate and store strong, unique cryptographic salts."));
            _questions.Add(new QuizQuestion("What type of vector involves attackers calling a target posing as internal support agents?", new[] { "A) Ransomware", "B) DDoS", "C) Social Engineering / Vishing", "D) Trojan injection" }, "C", "Manipulating human authorization elements relies directly on exploitation through social Engineering."));
            _questions.Add(new QuizQuestion("True or False: Public Wi-Fi access points are completely safe for processing financial banking transfers.", new[] { "A) True", "B) False" }, "B", "Unsecured open networks expose transaction logs to Man-In-The-Middle packet sniffing tools."));
            _questions.Add(new QuizQuestion("What does activating Multi-Factor Authentication (MFA) provide?", new[] { "A) It speeds up device processing rates", "B) It establishes a secondary defense barrier", "C) It deletes temporary system storage profiles", "D) It guarantees absolute immunity from computer hardware faults" }, "B", "MFA blocks access even if core password tokens become exposed."));
            _questions.Add(new QuizQuestion("What does a ransomware payload target?", new[] { "A) Stealing local visual hardware components", "B) Encrypting baseline configuration files to demand payment", "C) Accelerating memory fans configurations", "D) Automating structural syntax formatting checks" }, "B", "Ransomware locks target user files using cryptographic chains to extort financial payouts."));
            _questions.Add(new QuizQuestion("True or False: Software patches should be delayed indefinitely to optimize platform resource uptime metrics.", new[] { "A) True", "B) False" }, "B", "Delayed patching expands exposure windows to critical Zero-Day vulnerabilities."));
            _questions.Add(new QuizQuestion("What defines a strong, highly reliable secure password?", new[] { "A) At least 8-12 characters mixed with symbols and cases", "B) A common term spelled entirely in lowercase", "C) Short alphanumeric keys refreshed weekly", "D) Any numeric sequence matching a primary cell number" }, "A", "Length combined with systemic complexity increases brute-force computation costs exponentially."));
            _questions.Add(new QuizQuestion("True or False: A USB drive found in a shared public zone can safely be used to test local drive storage.", new[] { "A) True", "B) False" }, "B", "Unverified hardware units can execute malicious physical HID drops or script injectors immediately."));
        }

        public static string StartQuiz()
        {
            _currentIndex = 0;
            _score = 0;
            IsQuizRunning = true;
            BotEngine.AddLog("Quiz", "A new cybersecurity validation test pipeline has been initialized.");

            
            return $" CYBERSECURITY ASSESSMENT STARTED!\nAnswer by entering the letter corresponding to your selection.\n\n[QUESTION 1]: {_questions[_currentIndex].QuestionText}\n" + string.Join("\n", _questions[_currentIndex].Options);
        }

        public static string ProcessAnswer(string input, out string reactionMood)
        {
            reactionMood = "default";
            if (!IsQuizRunning) return "Quiz engine offline.";

            string cleanAns = input.Trim().ToUpper();
            bool isCorrect = cleanAns == _questions[_currentIndex].CorrectOption;

            string feedbackText = "";
            if (isCorrect)
            {
                _score++;
                reactionMood = "curious";
                feedbackText = $"✅ CORRECT! {_questions[_currentIndex].Explanation}";
            }
            else
            {
                reactionMood = "error_fallback";
                feedbackText = $"❌ INCORRECT. (The correct answer was {_questions[_currentIndex].CorrectOption}).\n💡 {_questions[_currentIndex].Explanation}";
            }

            _currentIndex++;

            if (_currentIndex < _questions.Count)
            {
                return $"{feedbackText}\n\n-------------------------\n[QUESTION {_currentIndex + 1} / {_questions.Count}]:\n{_questions[_currentIndex].QuestionText}\n" + string.Join("\n", _questions[_currentIndex].Options);
            }
            else
            {
                IsQuizRunning = false;
                BotEngine.AddLog("Quiz", $"Completed quiz module with a final tracking evaluation: {_score}/{_questions.Count}");

                string closingPerformanceVerdict = (_score >= 8)
                    ? "🏆 Great job! You're a cybersecurity pro!"
                    : "⚠️ Keep learning to stay safe online!";

                return $"{feedbackText}\n\n🏁 ASSESSMENT COMPLETE!\nFinal Score Evaluated: {_score} / {_questions.Count}\n{closingPerformanceVerdict}";
            }
        }
    }
}