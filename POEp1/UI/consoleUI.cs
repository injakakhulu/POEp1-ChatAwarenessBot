/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace POEp1.UI
{
    public class ConsoleUI
    {
        public static void InitUI()
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Green;
        }

        public static void DisplayHeader()
        {
            InitUI();

            TypeText("****************************************");
            TypeText("   CYBERSECURITY AWARENESS BOT");
            TypeText("**************************************\n");

            TypeText("[SYSTEM] Initializing...");
            Thread.Sleep(500);

            TypeText("[SYSTEM] Loading modules...");
            Thread.Sleep(500);

            TypeText("[SYSTEM] Secure connection established.\n");
        }

        public static string GetUserName()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Thread.Sleep(500);
            TypeText("\n> ENTER NAME: ");
            string name = Console.ReadLine();
            Console.ResetColor();

            while (string.IsNullOrWhiteSpace(name))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Thread.Sleep(500);
                TypeText("> INVALID INPUT. NAME CANT BE EMPTY TRY AGAIN: ");
                Console.ResetColor();
                name = Console.ReadLine();
            }

            return name;
        }

        public static void TypeText(string text)
        {
            foreach (char c in text)
            {
                Console.Write(c);

                if (c == '.' || c == ',' || c == '!')
                    Thread.Sleep(200);
                else
                    Thread.Sleep(15);
            }
            Console.WriteLine();
        }
    }
} */