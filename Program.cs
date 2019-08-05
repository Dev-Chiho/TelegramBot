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
    }
}
