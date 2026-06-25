using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using POEp1.Models;

namespace POEp1.Services
{
    public static class BotEngine
    {
        private static readonly List<LogEntry> _activityLogs = new List<LogEntry>();

        // Tracks the multi-stage state workflow for interactive task creation
        private static string _stagedTaskTitle = string.Empty;
        private static string _stagedTaskDesc = string.Empty;
        private static bool _awaitingReminderConfirmation = false;

        // Instance tracking link to pass operations directly to your Part 2 state engine
        private static ChatbotService _legacyChatbotInstance;

        static BotEngine()
        {
            AddLog("System", "Terminal environment sub-engine architecture instantiated smoothly.");

            // Instantiate a fallback structural user profile context to keep memory working safely
            UserProfile standardProfile = new UserProfile { Name = "User", FavouriteTopic = "" };
            _legacyChatbotInstance = new ChatbotService(standardProfile);
        }

        /// <summary>
        /// Allows your UI Forms to hand over your existing logged-in User ChatbotService instance 
        /// to ensure memory profile sync is maintained perfectly across windows.
        /// </summary>
        public static void RegisterChatbotContext(ChatbotService activeService)
        {
            if (activeService != null)
            {
                _legacyChatbotInstance = activeService;
            }
        }

        public static void AddLog(string category, string description)
        {
            _activityLogs.Add(new LogEntry(category, description));
        }

        public static string ProcessInput(string input, out string mood)
        {
            string clean = input.ToLower().Trim();
            mood = "default";

            // ==========================================
            // INTERCEPT A: RUNNING QUIZ HOOK
            // ==========================================
            if (QuizService.IsQuizRunning)
            {
                return QuizService.ProcessAnswer(input, out mood);
            }

            // ==========================================
            // INTERCEPT B: ACTIVE MULTI-STAGE TASK WORKFLOW
            // ==========================================
            if (_awaitingReminderConfirmation)
            {
                _awaitingReminderConfirmation = false;
                int reminderDays = 0;

                var match = Regex.Match(clean, @"\d+");
                if (match.Success)
                {
                    int.TryParse(match.Value, out reminderDays);
                }

                bool success = DatabaseService.AddTask(_stagedTaskTitle, _stagedTaskDesc, reminderDays);
                AddLog("Database", $"Committed new entry: '{_stagedTaskTitle}' with a {reminderDays}-day threshold block.");

                string responseString = success
                    ? $"Got it! I've logged the reminder threshold for {reminderDays} days in the active storage server profile."
                    : "Notice: Internal database writing operational pipeline encountered a local timeout fault.";

                _stagedTaskTitle = string.Empty;
                _stagedTaskDesc = string.Empty;
                return responseString;
            }

            // ==========================================
            // 1. AUDIT LOG PARSING (Task 4)
            // ==========================================
            if (Regex.IsMatch(clean, @"\b(show\s+activity\s+log|view\s+log|activity\s+log)\b"))
            {
                AddLog("NLP", "Interpreted structural request tracking action.");
                mood = "curious";

                StringBuilder sb = new StringBuilder("### ACTIVE SYSTEM TRACK LOG RECORDS:\n");
                var records = _activityLogs.AsEnumerable().Reverse().Take(8).ToList();
                for (int i = 0; i < records.Count; i++)
                {
                    sb.AppendLine($"{i + 1}. {records[i]}");
                }
                return sb.ToString();
            }

            // ==========================================
            // 2. OPERATIONAL ACTION LIST DUMPS
            // ==========================================
            if (Regex.IsMatch(clean, @"\b(what\s+have\s+you\s+done\s+for\s+me|recent\s+actions|summary)\b"))
            {
                AddLog("NLP", "Parsed automated log state query via intent engine.");
                mood = "curious";

                var activeDbTasks = DatabaseService.GetAllTasks();
                if (activeDbTasks.Count == 0)
                {
                    return "OPERATIONAL METRICS LOG:\nNo active tasks compiled in structural cluster arrays yet.";
                }

                StringBuilder sb = new StringBuilder("Here's a summary of recent actions:\n");
                int idx = 1;
                foreach (var task in activeDbTasks.Take(5))
                {
                    string trackingContext = task.ReminderDays > 0 ? $"Reminder flagged for {task.ReminderDays} days" : "No active verification timer set";
                    sb.AppendLine($"{idx++}. Task managed: '{task.Title}' ({trackingContext}) [Status: {(task.IsCompleted ? "DONE" : "PENDING")}].");
                }
                return sb.ToString();
            }

            // ==========================================
            // 3. TASK COMPILATION INTERFACE HOOKS (Task 1 & Task 3)
            // ==========================================
            if (Regex.IsMatch(clean, @"\b(add\s+task|remind|reminder|set\s+up\s+a\s+task)\b"))
            {
                AddLog("NLP", "Advanced RegEx extracted interactive task building configuration intent.");

                if (clean.Contains("password"))
                {
                    _stagedTaskTitle = "Update Password Profile";
                    _stagedTaskDesc = "Rotate authentication parameter matrix to block dictionary crack attacks.";
                    _awaitingReminderConfirmation = true;
                    return "Reminder set for 'Update my password' on tomorrow's date. Would you like to log a specific structural reminder interval numeric value (e.g., 3 days)?";
                }
                else if (Regex.IsMatch(clean, @"\b(2fa|two-factor|auth)\b"))
                {
                    _stagedTaskTitle = "Enable 2FA Authentication Layers";
                    _stagedTaskDesc = "Activate two-factor security profiles to prevent peripheral credential bypass.";
                    _awaitingReminderConfirmation = true;
                    return "Task added: 'Enable two-factor authentication.' Would you like to set a reminder timeframe for this task?";
                }
                else if (clean.Contains("privacy"))
                {
                    _stagedTaskTitle = "Review Privacy Settings Configuration";
                    _stagedTaskDesc = "Audit privacy layouts to prevent unauthorized external vector exposure.";
                    _awaitingReminderConfirmation = true;
                    return "Task added with the description 'Review account privacy settings to ensure your data is protected.' Would you like a reminder timeframe configured?";
                }

                _stagedTaskTitle = "Custom Cybersecurity Mitigation Task";
                _stagedTaskDesc = input;
                _awaitingReminderConfirmation = true;
                return $"I've registered a custom action intent tracking block: '{input}'. Would you like me to assign a reminder follow-up metric (e.g. 7 days)?";
            }

            // ==========================================
            // 4. ADVANCED DATABASE MANIPULATION MODES
            // ==========================================
            var completeMatch = Regex.Match(clean, @"\b(complete|finish|done)\s+task\s+(\d+)\b");
            if (completeMatch.Success)
            {
                int taskId = int.Parse(completeMatch.Groups[2].Value);

                DatabaseService.MarkTaskCompleted(taskId);
                AddLog("Database", $"Marked system item ID [{taskId}] complete successfully.");

                return $"Database sequence tracking updated. Task ID {taskId} is marked as completed in the persistent storage profile.";
            }

            var deleteMatch = Regex.Match(clean, @"\b(delete|remove|purge)\s+task\s+(\d+)\b");
            if (deleteMatch.Success)
            {
                int taskId = int.Parse(deleteMatch.Groups[2].Value);

                DatabaseService.DeleteTask(taskId);
                AddLog("Database", $"Purged data row ID [{taskId}] out of global schemas cleanly.");

                return $"Database sequence executed. Target data index row {taskId} wiped out of structural database records.";
            }

            // ==========================================
            // 5. INTERACTIVE QUIZ TRIGGER
            // ==========================================
            if (Regex.IsMatch(clean, @"\b(quiz|game|start\s+quiz)\b"))
            {
                return QuizService.StartQuiz();
            }

            // ==========================================
            // 6. MASTER PASSTHROUGH INTELLIGENCE LAYER
            // ==========================================
            // Run the prompt through your advanced legacy ChatbotService infrastructure.
            // This preserves memory, topic tracking, random variations, and emotional state detection perfectly!
            string responseOutput = _legacyChatbotInstance.GetBotReply(input, out string legacyMood);
            mood = legacyMood;

            // If your legacy chatbot caught a known prompt/keyword/help, pass it directly to the UI
            if (!responseOutput.Contains("I DONT UNDERSTAND YOUR STATEMENT"))
            {
                AddLog("Core Pipeline", $"Routed input to ChatbotService workspace matrix successfully. Mood context pulled: {mood}");
                return responseOutput;
            }

            // If it hits a total dead end across Part 1, Part 2, and Part 3 pipelines, render custom unified fallback help tips
            mood = "error_fallback";
            return "I didn't quite catch those configuration parameters.\n\n" +
                   "👉 *To use Part 3 Task Manager*: Type 'Add task to check my privacy' or 'remind me to update my password'\n" +
                   "👉 *To play the Game*: Type 'start quiz'\n" +
                   "👉 *To view database tracking*: Type 'show activity log' or 'summary'\n" +
                   "👉 *For security domains*: Ask regarding 'passwords', 'scams', or 'privacy' to parse emotional metrics!";
        }
    }
}