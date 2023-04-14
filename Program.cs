using System;
using Telegram.Bot;

namespace LangTranslationTelegramBot
{
    class Program
    {

        // Install Telegram Bot v16.0.2
        // bot name: LangTelegramBot
        // 6191785024:AAEYEPcVg7oyd1C5xweaJc3kRFJLYJUQew4

        static TelegramBotClient Bot;

        [Obsolete]
        static void Main(string[] args)
        {

            Bot = new TelegramBotClient("6191785024:AAEYEPcVg7oyd1C5xweaJc3kRFJLYJUQew4");

            //var me = Bot.GetMeAsync().Result;
            //Console.WriteLine(me.FirstName);

            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            Console.WriteLine(e.Message.Text);
            Console.WriteLine(e.Message.From.Username);
        }

    }
}
