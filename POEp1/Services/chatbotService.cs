using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using POEp1.Models;
using POEp1.UI;
using System;

namespace POEp1.Services
{
    public class ChatbotService
    {
        private UserProfile _user;

        public ChatbotService(UserProfile user)
        {
            _user = user;
        }

        public void StartChat()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            ConsoleUI.TypeText($"\n[SYSTEM] WELCOME, {_user.Name}.");
            ConsoleUI.TypeText("[SYSTEM] Type 'help' for commands.");
            Console.ResetColor();

            while (true)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write("\n> ");
                Console.ResetColor();

                string input = Console.ReadLine().ToLower();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    ConsoleUI.TypeText("[SYSTEM] Invalid input.");
                    Console.ResetColor();
                    continue;
                }

                switch (input)
                {
                    case "exit":
                        Console.ForegroundColor = ConsoleColor.Red;
                        ConsoleUI.TypeText("[SYSTEM] Session terminated.");
                        Console.ResetColor();
                        return;

                    case "help":
                        ShowHelp();
                        break;

                    case "clear":
                        Console.Clear();
                        ConsoleUI.DisplayHeader();
                        break;

                    default:
                        HandleChat(input);
                        break;
                }
            }
        }

        private void HandleChat(string input)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;

            ConsoleUI.TypeText("[BOT] Thinking...");
            System.Threading.Thread.Sleep(400);

            string response = ResponseService.GetResponse(input);

            ConsoleUI.TypeText("[BOT] " + response);

            Console.ResetColor();
        }

        private void ShowHelp()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            ConsoleUI.TypeText("\n[SYSTEM] Available Commands:");
            ConsoleUI.TypeText(" - help   Show this menu");
            ConsoleUI.TypeText(" - clear  Clear screen");
            ConsoleUI.TypeText(" - exit   Close chatbot");
            ConsoleUI.TypeText("\n[SYSTEM] You can also ask about:");
            ConsoleUI.TypeText(" - passwords");
            ConsoleUI.TypeText(" - phishing");
            ConsoleUI.TypeText(" - browsing");
            ConsoleUI.TypeText(" - purpose");
            Console.ResetColor();
        }
    }
}
 
