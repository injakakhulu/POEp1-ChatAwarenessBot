using System;
using System.Collections.Generic;
using POEp1.Models;

namespace POEp1.Services
{
  
    public delegate string BotResponseProcessor(string input, out string analyzedMood);

    public class ChatbotService
    {
        private readonly UserProfile _user;
        private readonly Random _random;

        // Generic Collections storing multiple randomized tips for variation
        private readonly Dictionary<string, List<string>> _topicTips;
        private readonly Dictionary<string, string> _sentimentResponses;

        public ChatbotService(UserProfile user)
        {
            _user = user;
            _random = new Random();

            // Populate generic collections for Cybersecurity keywords
            _topicTips = new Dictionary<string, List<string>>
            {
                { "password", new List<string> {
                    "Use strong passwords with uppercase, numbers, symbols, and at least 12 characters.",
                    "Never reuse the same password across multiple platforms. Try a master Password Manager.",
                    "Change your credentials immediately if a service you use reports a data breach leaks.",
                    "Enable biometric security locks or complex passphrases instead of predictable words.",
                    "Avoid using personal tracking data like birthdays or pet names inside password strings."
                }},
                { "scam", new List<string> {
                    "Phishing is an online scam where attackers trick you into revealing sensitive credentials.",
                    "Always verify corporate senders directly via official channels before clicking unknown links.",
                    "If an urgent email demands instant payments using crypto or gift cards, it is an absolute scam.",
                    "Be wary of unexpected text messages offering fake delivery tracking updates or refunds.",
                    "Scammers use psychological pressure; if an alert panics you into acting fast, pause and verify."
                }},
                { "privacy", new List<string> {
                   "Review privacy account configuration settings on social media to restrict global tracking.",
                   "Enforce multi factor authentication everywhere to secure your accounts from remote bypasses.",
                   "Avoid accessing your banking portals or processing private credentials on public WiFi zones.",
                   "Clear browser cookies regularly and use privacyfocused search tools to limit data tracking.",
                   "Be careful with app permissions; don't give a basic utility app access to your contacts or location."
                }}
            };

            // Populate sentiment configuration map
            _sentimentResponses = new Dictionary<string, string>
            {
                { "worried", "It is completely understandable to feel anxious about cybersecurity risks. The threat landscape can be intimidating, but staying informed puts control back in your hands." },
                { "frustrated", "I understand your frustration entirely. Complex security frameworks can feel exhausting, but maintaining steady habits keeps your data secure." },
                { "curious", "Fantastic! Curiosity is your greatest asset in engineering secure habits. Let us dive deep into the data matrix." }
            };
        }

        public string GetBotReply(string rawInput, out string analyzedMood)
        {
            analyzedMood = "neutral";
            string input = rawInput.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(input))
                return "SYSTEM PROTOCOL EXCEPTION: Empty transmission detected.";

            // 1. Process Sentiment Engine Sub-Routine Interception
            string sentimentFound = "";
            if (input.Contains("worried") || input.Contains("scared") || input.Contains("afraid")) sentimentFound = "worried";
            else if (input.Contains("frustrated") || input.Contains("annoyed") || input.Contains("tired")) sentimentFound = "frustrated";
            else if (input.Contains("curious") || input.Contains("interested") || input.Contains("learn")) sentimentFound = "curious";

            // 2. Handle Continuous Sequential Follow-up State Prompts
            bool isFollowUp = input.Contains("more") || input.Contains("another") || input.Contains("explain") || input.Contains("continue");

            if (isFollowUp && !string.IsNullOrEmpty(_user.LastDiscussedTopic))
            {
                return $"Continuing content sequence on [{_user.LastDiscussedTopic.ToUpper()}]:\n" + GetRandomTip(_user.LastDiscussedTopic);
            }

            // 3. Keyword Domain Matching Evaluators
            string matchedTopic = "";
            if (input.Contains("password")) matchedTopic = "password";
            else if (input.Contains("scam") || input.Contains("phishing")) matchedTopic = "scam";
            else if (input.Contains("privacy") || input.Contains("browsing")) matchedTopic = "privacy";

            
            if (!string.IsNullOrEmpty(sentimentFound))
            {
                analyzedMood = sentimentFound;
                string emotionalEmpathy = _sentimentResponses[sentimentFound];

                // If a topic was mentioned along with a sentiment, link them into one seamless flow
                if (!string.IsNullOrEmpty(matchedTopic))
                {
                    _user.LastDiscussedTopic = matchedTopic;
                    _user.FavouriteTopic = matchedTopic; // Save preference to user memory profile
                    return $"{emotionalEmpathy}\n\nHere is an immediate safety insight on {matchedTopic.ToUpper()} to help you protect yourself:\n> " + GetRandomTip(matchedTopic);
                }

                // If a user just expressed a sentiment without a topic, use their favorite topic or prompt them
                if (!string.IsNullOrEmpty(_user.FavouriteTopic))
                {
                    return $"{emotionalEmpathy}\n\nSince you previously expressed interest in {_user.FavouriteTopic.ToUpper()}, keep this defensive step in mind:\n> " + GetRandomTip(_user.FavouriteTopic);
                }

                return $"{emotionalEmpathy}\n\nWhat security domain can I help clarify for you right now? (Passwords, Scams, Privacy)";
            }

            if (!string.IsNullOrEmpty(matchedTopic))
            {
                _user.LastDiscussedTopic = matchedTopic;

                // Personalize the response if the user matches their logged favorite area
                string memoryPrefix = "";
                if (string.Equals(_user.FavouriteTopic, matchedTopic, StringComparison.OrdinalIgnoreCase))
                {
                    memoryPrefix = $"[SYSTEM RECALL: Based on your target interest in {matchedTopic.ToUpper()}]\n";
                }
                else if (string.IsNullOrEmpty(_user.FavouriteTopic))
                {
                    _user.FavouriteTopic = matchedTopic;
                    memoryPrefix = $"[PROFILE UPDATED: Saved '{matchedTopic}' as your primary interest area]\n";
                }

                return $"{memoryPrefix}Defensive standard guidelines configured for {matchedTopic.ToUpper()}:\n> " + GetRandomTip(matchedTopic);
            }

            // Fallback Engine Commands Menu
            if (input.Contains("help") || input.Contains("status"))
            {
                return "SECURE TERMINAL ASSISTANCE DIRECTORY:\n" +
                       "• Inquire directly regarding system threat layers: 'passwords', 'scams', or 'privacy'.\n" +
                       "• Type your operational state to test sentiment tracking: 'I am worried about threats'.\n" +
                       "• For additional topic details on your active track, try typing: 'Tell me more'.";
            }

            return "I DONT UNDERSTAND YOUR STATEMENT.\n" +
                   "Please focus inquiry parameters around 'passwords', 'scams', or 'privacy', or ask for 'help'.";
              
        }

        private string GetRandomTip(string topic)
        {
            if (_topicTips.TryGetValue(topic, out List<string> tips))
            {
                int index = _random.Next(tips.Count);
                return tips[index];
            }
            return "No auxiliary defense matrix logs cataloged for this item track index.";
        }
    }
}