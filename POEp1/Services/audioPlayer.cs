using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Media;
using POEp1.UI;

namespace POEp1.Services
{
    public class AudioPlayer
    {
        public static void PlayGreeting()
        {
            try
            {
                SoundPlayer player = new SoundPlayer("Assets/greeting.wav");
                player.PlaySync();
            }
            catch
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Thread.Sleep(100);
                ConsoleUI.TypeText("(Audio file not found)");
                Console.ResetColor();

            }
        }
    }
} 