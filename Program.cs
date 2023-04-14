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
        static Tutor Tutor = new Tutor();

        const string COMMAND_LIST =
@"
/add <eng> <rus> - Добавление английского слова и его перевод в словарь
/get - получаем случайное английское слово из словаря
/check <eng> <rus> - проверяем правильность перевода английского слова
";

        //Tutor Tutor = new Tutor();

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

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e == null || e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            var msgArgs = e.Message.Text.Split(' ');
            string text;

            switch (msgArgs[0])
            {
                case "/start":
                    text = COMMAND_LIST;
                    break;

                case "/add":
                    text = AddWords(msgArgs);
                    break;

                case "/get":
                    text = Tutor.GetRandomEngWord();
                    break;

                case "/check":
                    text = CheckWord(msgArgs);
                    break;

                default:
                    text = COMMAND_LIST;
                    break;
            }

            await Bot.SendTextMessageAsync(e.Message.From.Id, text);

            Console.WriteLine(e.Message.Text);
            Console.WriteLine(e.Message.From.Username);
        }

        private static string CheckWord(string[] msgArr)
        {
            if (msgArr.Length != 3)
                return "Не правильное количество аргументов. Их должно быть два";
            else
                return CheckWord(msgArr[1], msgArr[2]);
        }

        private static string CheckWord(string eng, string rus)
        {
            if (Tutor.CheckWord(eng, rus))
                return "Правильно!";
            else
            {
                var correctAnswer = Tutor.Translate(eng);
                return ($"Неверно. Правильный ответ: \"{correctAnswer}\".");
            }
        }

        private static string AddWords(string[] msgArr)
        {
            if(msgArr.Length !=3)
                return "Не правильное количество аргументов. Их должно быть два";
            else
            {
                Tutor.AddWord(msgArr[1], msgArr[2]);
                return "Новое слово добавлено в словарь";
            }
            
        }
    }
}
