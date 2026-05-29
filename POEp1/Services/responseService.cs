using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POEp1.Services
{
    public static class ResponseService
    {

        public static string GetResponse(string input)
        {
            input = input.ToLower();

            return input switch
            {
                string s when s.Contains("how are you") =>
                    "I'm operating at full capacity, never been better thank you for asking " ,

                string s when s.Contains("purpose") =>
                    "My purpose is to educate you about cybersecurity.",

                string s when s.Contains("what can i ask") =>
                    "You can ask about the following passwords, phishing, and safe browsing, and i will answer to my fullest capacity",

                string s when s.Contains("password") =>
                    "Password is a security measure that allows grants access, to have a strong password you must use strong passwords with uppercase, numbers, symbols. and lenght should be at least 8 characters",

                string s when s.Contains("phishing") =>
                    "Phishing is a scam used by hackers where attackers trick you into revealing personal info.",

                string s when s.Contains("browsing") =>
                    "browsing is the act of searching for information on the internet but always use secure websites that contain HTTPS and avoid suspicious links.",

                string s when s.Contains("exit") =>
                    "SESSION TERMINATED.",

                _ =>
                    "I didn't quite understand that. Could you rephrase?"
            };
        }
    }
}