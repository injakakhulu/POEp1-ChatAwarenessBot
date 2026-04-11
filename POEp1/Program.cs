using POEp1.Models;
using POEp1.Services;
using POEp1.UI;
using System;


class Program
{
    static void Main(string[] args)
    {
        ConsoleUI.DisplayHeader();

        AudioPlayer.PlayGreeting();

        string name = ConsoleUI.GetUserName();

        UserProfile user = new UserProfile(name);

        ChatbotService chatbot = new ChatbotService(user);
        chatbot.StartChat();
    }
}