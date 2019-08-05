using System;
using System.Timers;
using CustomBot.Enums;
using CustomBot.model;
using Telegram.Bot;
using static CustomBot.biz.Process.ProcessCommon;

namespace CustomBot.biz.Process
{
    public class ProcessTelegram : ProcessBase
    {
        public ProcessTelegram()
        {
            baseBehavior = Behavior.Telegram;
        }

        private static Timer timer;

        /// <summary>
        /// 텔레그램 봇 기능 재정의
        /// </summary>
        public override void Run_Bot()
        {
            var Bot = new TelegramBotClient(GlobalData.Variable._telegramToken);

            var me = Bot.GetMeAsync().Result;

            Bot.OnMessage += BotOnMessageReceived;
            Bot.OnMessageEdited += BotOnMessageReceived;
            Bot.OnCallbackQuery += BotOnCallbackQueryReceived;
            //Bot.OnInlineQuery += BotOnInlineQueryReceived;
            Bot.OnInlineResultChosen += BotOnChosenInlineResultReceived;
            Bot.OnReceiveError += BotOnReceiveError;


            // 타이머로 1초마다 이벤트 호출
            timer = new Timer(1000);
            timer.Elapsed += OnTimedEventSendMessage;
            timer.Enabled = true;

            Bot.StartReceiving();
            //Console.WriteLine($"Start listening for @{me.Username}");
            //Console.WriteLine($"_");
            Console.ReadLine();
            Bot.StopReceiving();
        }
    }
}

