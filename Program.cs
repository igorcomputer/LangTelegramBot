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

        const string COMMAND_LIST =
@"
/add <eng> <rus> - Добавление английского слова и его перевод в словарь
/get - получаем случайное английское слово из словаря
/check <eng> <rus> - проверяем правильность перевода английского слова
";

        [Obsolete]
        static void Main(string[] args)
        {

            Bot = new TelegramBotClient("6191785024:AAEYEPcVg7oyd1C5xweaJc3kRFJLYJUQew4");



            // TEST 
            //var me = Bot.GetMeAsync().Result;
            //Console.WriteLine(me.FirstName);

            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.ReadLine();
            Bot.StopReceiving();

        }

        private static void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e == null || e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            var msgArgs = e.Message.Text.Split(' ');

            switch (msgArgs[0])
            {
                case "/start":
                    Bot.SendTextMessageAsync(e.Message.From.Id, COMMAND_LIST);
                    break;
            }

            Console.WriteLine(e.Message.Text);
            Console.WriteLine(e.Message.From.Username);
        }

    }
}
