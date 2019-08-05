using System;
using CustomBot.biz.Generate;
using CustomBot.Enums;

namespace CustomBot
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Console Program";
            var generate = Generate.Instance();
            generate.Excute(Behavior.Telegram);
        }

        //static void Main(string[] args)
        //{
        //    // 봇 접근 함수 호출
        //    APIAsync();
        //    System.Console.ReadLine();
        //}

        //// 비동기 봇 접근
        //static async void APIAsync()
        //{
        //    var Bot = new TelegramBotClient(GlobalData.Variable._telegramToken);
        //    var me = await Bot.GetMeAsync();

        //    //올바르고 접속이 되면 봇 이름이 출력된다.
        //    System.Console.WriteLine($"Hello my name is {me.FirstName}");
        //}
    }
}
