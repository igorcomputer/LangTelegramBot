using System;
using System.Collections.Generic;
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

        // <user_id, word> 
        static Dictionary<long, string> LastWord = new Dictionary<long, string>();

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

        private static async void Bot_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e == null || e.Message == null || e.Message.Type != Telegram.Bot.Types.Enums.MessageType.Text)
                return;

            Console.WriteLine(e.Message.Text);
            Console.WriteLine(e.Message.From.Username);

            var userID = e.Message.From.Id;
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
                    text = GetRandomEngWord(userID);
                    break;

                case "/check":
                    text = CheckWord(msgArgs);
                    break;

                default:
                    if (LastWord.ContainsKey(userID))
                        text = CheckWord(LastWord[userID], msgArgs[0]);
                    else
                        text = COMMAND_LIST;
                    break;
            }

            await Bot.SendTextMessageAsync(e.Message.From.Id, text);
        }


        private static string GetRandomEngWord(long userID)
        {
            var text = Tutor.GetRandomEngWord();
            if (LastWord.ContainsKey(userID))
                LastWord[userID] = text;
            else
                LastWord.Add(userID, text);

            return text;
        }

        private static string CheckWord(string[] msgArr)
        {
            if (msgArr.Length != 3)
                return "Не правильное количество аргументов. Их должно быть два";
            else
            {
                if (Tutor.HasWord(msgArr[1]))
                    return CheckWord(msgArr[1], msgArr[2]);
                else
                    return $"Слова \"{msgArr[1]}\" нет в словаре";
            }
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
            if (msgArr.Length != 3)
                return "Не правильное количество аргументов. Их должно быть два";
            else
            {
                Tutor.AddWord(msgArr[1], msgArr[2]);
                return "Новое слово добавлено в словарь";
            }
        }
    }
}
