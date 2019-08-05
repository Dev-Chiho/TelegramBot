using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using Abot.Crawler;
using Abot.Poco;
using CustomBot.model;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ScrapySharp.Extensions;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace CustomBot.biz.Process
{
    public class ProcessCommon
    {
        private static ProcessCommon _instance;
        public ProcessCommon()
        {

        }

        public static ProcessCommon Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProcessCommon();
                }

                return _instance;
            }
        }

        //봇 생성
        public static readonly TelegramBotClient Bot = new TelegramBotClient(GlobalData.Variable._telegramToken);

        #region 봇 MessageReceived
        public static async void BotOnMessageReceived(object sender, MessageEventArgs messageEventArgs)
        {
            var message = messageEventArgs.Message;

            #region 위치 관련
            if ("Location".Equals(message.Type.ToString()))
            {
                string Result = GetWeatherLatLng(Convert.ToInt16(message.Location.Latitude), Convert.ToInt16(message.Location.Longitude));
                await Bot.SendTextMessageAsync
                (
                    message.Chat.Id,
                    Result
                );
            }
            else if ("Venue".Equals(message.Type.ToString()))
            {
                string Result = GetWeatherLatLng(Convert.ToInt16(message.Location.Latitude), Convert.ToInt16(message.Location.Longitude));
                await Bot.SendTextMessageAsync
                (
                    message.Chat.Id,
                    Result
                );
            }
            #endregion

            string getResult = string.Empty;

            if (message == null || message.Type != MessageType.Text) return;

            #region 확인용
            string _detail = string.Empty;

            _detail += $"\r\n========================\r\n";
            _detail += $"{message.Chat.Id}\r\n";
            _detail += $"========================\r\n";
            if (message.From != null)
            {
                _detail += $"{message.From.FirstName} ";
                _detail += $"{message.From.LastName}\r\n";
                _detail += $"{message.From.Username}\r\n";
            }
            else if (message.Chat.FirstName != null)
            {
                _detail += $"{message.Chat.FirstName} ";
                _detail += $"{message.Chat.LastName}\r\n";
            }
            if (message.Chat.Username != null)
            {
                _detail += $"@{message.Chat.Username}\r\n";
            }
            _detail += $"{message.Text}\r\n";
            _detail += $"========================\r\n";
            //$"{message.Chat.Id}\r\n{message.Chat.FirstName} {message.Chat.LastName}\r\n@{message.Chat.Username}\r\n\r\n"
            Console.WriteLine(_detail);
            #endregion

            #region 찾아줘
            if (message.Text.IndexOf("찾아줘") > -1)
            {
                if (!"".Equals(message.Text.Replace("찾아줘", "")))
                {
                    getResult = GetWebSearch(message.Text.Substring(0, message.Text.IndexOf(" 찾아줘")), "n");

                    await Bot.SendTextMessageAsync
                    (
                        message.Chat.Id,
                        getResult,
                        parseMode: ParseMode.Markdown
                    );
                }
            }
            else if (message.Text.IndexOf("검색해줘") > -1)
            {
                if (!"".Equals(message.Text.Replace("검색해줘", "")))
                {
                    getResult = GetWebSearch(message.Text.Substring(0, message.Text.IndexOf(" 검색해줘")), "g");

                    await Bot.SendTextMessageAsync
                    (
                        message.Chat.Id,
                        getResult,
                        parseMode: ParseMode.Markdown
                    );
                }
            }
            #endregion            

            #region $명령어
            if (message.Text.IndexOf('$') > -1)
            {
                switch (message.Text.Split(' ').First())
                {
                    #region 네이버 검색
                    case "$s":
                        if (!"".Equals(message.Text.Replace("$s", "")))
                        {
                            getResult = GetNaverSearch(message.Text.Replace("$s ", ""));
                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;
                    case "$ㄴ":
                        if (!"".Equals(message.Text.Replace("$ㄴ", "")))
                        {
                            getResult = GetNaverSearch(message.Text.Replace("$ㄴ ", ""));
                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;
                        #endregion
                }
            }
            #endregion

            #region 로또 번호 생성
            if (message.Text.IndexOf("/내놔") > -1)
            {
                if ("".Equals(message.Text.Replace("/내놔", "")))
                {
                    getResult = GetLottoNumbers(1);

                    await Bot.SendTextMessageAsync
                    (
                        message.Chat.Id,
                        getResult
                    );
                    return;
                }
                else if (!"".Equals(message.Text.Replace("/내놔", "")))
                {
                    var _number = Convert.ToInt32(message.Text.Replace("/내놔", ""));
                    if (_number < 31)
                    {
                        getResult = GetLottoNumbers(_number);

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            getResult
                        );
                        return;
                    }
                    else
                    {

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            "과하시네요.."
                        );
                        return;
                    }
                }
            }

            if (message.Text.IndexOf("/soshk") > -1)
            {
                if ("".Equals(message.Text.Replace("/soshk", "")))
                {
                    getResult = GetLottoNumbers(1);

                    await Bot.SendTextMessageAsync
                    (
                        message.Chat.Id,
                        getResult
                    );
                    return;
                }
                else if (!"".Equals(message.Text.Replace("/soshk", "")))
                {
                    var _number = Convert.ToInt32(message.Text.Replace("/soshk", ""));
                    if (_number < 31)
                    {
                        getResult = GetLottoNumbers(_number);

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            getResult
                        );
                        return;
                    }
                    else
                    {

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            "That's too much.."
                        );
                        return;
                    }
                }
            }
            #endregion

            #region /명령어
            else if (message.Text.IndexOf('/') > -1)
            {
                switch (message.Text.Split(' ').First())
                {
                    #region 현재시간
                    case "/time":

                        await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                DateTime.Now.ToString("현재 시간은 HH시 mm분 ss초 입니다.")
                            );
                        break;
                    case "/샤ㅡㄷ":

                        await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                DateTime.Now.ToString("현재 시간은 HH시 mm분 ss초 입니다.")
                            );
                        break;
                    #endregion

                    #region 코인시세 API
                    case "/?":
                        if (!"".Equals(message.Text.Replace("/?", "")))
                        {
                            getResult = GetExchangeApi(message.Text.Split(' ').LastOrDefault());

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    #endregion

                    #region 날씨 API
                    case "/w":
                        getResult = GetWeatherApi();

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            getResult
                        );
                        break;
                    case "/ㅈ":
                        getResult = GetWeatherApi();

                        await Bot.SendTextMessageAsync
                        (
                            message.Chat.Id,
                            getResult
                        );
                        break;
                    #endregion

                    #region Naver ShortURL
                    case "/s":
                        if (!"".Equals(message.Text.Replace("/s", "")))
                        {
                            getResult = GetShortURL(message.Text.Split(' ').LastOrDefault());

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    case "/ㄴ":
                        if (!"".Equals(message.Text.Replace("/ㄴ", "")))
                        {
                            getResult = GetShortURL(message.Text.Split(' ').LastOrDefault());

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    #endregion

                    #region Naver Papago
                    case "/t":
                        if (!"".Equals(message.Text.Replace("/t", "")))
                        {
                            getResult = GetTranslate(message.Text.Replace("/t ", ""));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    case "/ㅅ":
                        if (!"".Equals(message.Text.Replace("/ㅅ", "")))
                        {
                            getResult = GetTranslate(message.Text.Replace("/ㅅ ", ""));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    #endregion

                    #region 크롤링
                    case "/실검":
                        if ("".Equals(message.Text.Replace("/실검", "")))
                        {
                            GetTypingMotion(message);
                            getResult = GetWebCrawler("네이버");
                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true

                            );
                        }
                        break;
                    case "/tlfrja":
                        if ("".Equals(message.Text.Replace("/tlfrja", "")))
                        {
                            GetTypingMotion(message);
                            getResult = GetWebCrawler("네이버");
                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    #endregion

                    #region 웹검색
                    case "/g":
                        if (!"".Equals(message.Text.Replace("/g", "")))
                        {
                            getResult = GetWebSearch(message.Text.Replace("/g ", ""), "g");

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;
                    case "/ㅎ":
                        if (!"".Equals(message.Text.Replace("/ㅎ", "")))
                        {
                            getResult = GetWebSearch(message.Text.Replace("/ㅎ ", ""), "g");

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;
                    case "/n":
                        if (!"".Equals(message.Text.Replace("/n", "")))
                        {
                            getResult = GetWebSearch(message.Text.Replace("/n ", ""), "n");

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;
                    case "/ㅜ":
                        if (!"".Equals(message.Text.Replace("/ㅜ", "")))
                        {
                            getResult = GetWebSearch(message.Text.Replace("/ㅜ ", ""), "n");

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown
                            );
                        }
                        break;

                    #endregion

                    #region 휴일 API
                    case "/h":
                        if ("".Equals(message.Text.Replace("/h", "")))
                        {
                            getResult = GetHoliDayApi(DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        else
                        {
                            if (message.Text.Replace("/h ", "").IndexOf("-") > -1)
                            {
                                getResult = GetHoliDayApi(message.Text.Replace("/h ", "").Split('-')[0], message.Text.Replace("/h ", "").Split('-')[1]);

                                await Bot.SendTextMessageAsync
                                (
                                    message.Chat.Id,
                                    getResult
                                );
                            }
                            else
                            {
                                getResult = GetHoliDayApi(message.Text.Replace("/h ", ""), "0");

                                await Bot.SendTextMessageAsync
                                (
                                    message.Chat.Id,
                                    getResult
                                );
                            }
                        }
                        break;
                    case "/ㅗ":
                        if ("".Equals(message.Text.Replace("/ㅗ", "")))
                        {
                            getResult = GetHoliDayApi(DateTime.Now.ToString("yyyy"), DateTime.Now.ToString("MM"));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        else
                        {
                            if (message.Text.Replace("/ㅗ ", "").IndexOf("-") > -1)
                            {
                                getResult = GetHoliDayApi(message.Text.Replace("/ㅗ ", "").Split('-')[0], message.Text.Replace("/ㅗ ", "").Split('-')[1]);

                                await Bot.SendTextMessageAsync
                                (
                                    message.Chat.Id,
                                    getResult
                                );
                            }
                            else
                            {
                                getResult = GetHoliDayApi(message.Text.Replace("/ㅗ ", ""), "0");

                                await Bot.SendTextMessageAsync
                                (
                                    message.Chat.Id,
                                    getResult
                                );
                            }
                        }
                        break;
                    #endregion

                    #region 로또번호
                    case "/로또":
                        if ("".Equals(message.Text.Replace("/로또", "")))
                        {
                            getResult = GetLottoPrizeNumber();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    case "/fhEh":
                        if ("".Equals(message.Text.Replace("/fhEh", "")))
                        {
                            getResult = GetLottoPrizeNumber();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    case "/fhEH":
                        if ("".Equals(message.Text.Replace("/fhEH", "")))
                        {
                            getResult = GetLottoPrizeNumber();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    case "/FHEH":
                        if ("".Equals(message.Text.Replace("/FHEH", "")))
                        {
                            getResult = GetLottoPrizeNumber();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult
                            );
                        }
                        break;
                    #endregion

                    #region 네이버 검색
                    case "/b":
                        if (!"".Equals(message.Text.Replace("/b", "")))
                        {
                            getResult = GetNaverSearch(message.Text.Replace("/b ", ""));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    case "/ㅠ":
                        if (!"".Equals(message.Text.Replace("/ㅠ", "")))
                        {
                            getResult = GetNaverSearch(message.Text.Replace("/ㅠ ", ""));

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    #endregion

                    #region 오늘의 운세
                    case "/운세":
                        if ("".Equals(message.Text.Replace("/운세", "")))
                        {
                            getResult = GetFortune();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    case "/dnstp":
                        if ("".Equals(message.Text.Replace("/dnstp", "")))
                        {
                            getResult = GetFortune();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    case "/f":
                        if ("".Equals(message.Text.Replace("/f", "")))
                        {
                            getResult = GetFortune();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    case "/ㄹ":
                        if ("".Equals(message.Text.Replace("/ㄹ", "")))
                        {
                            getResult = GetFortune();

                            await Bot.SendTextMessageAsync
                            (
                                message.Chat.Id,
                                getResult,
                                parseMode: ParseMode.Markdown,
                                disableWebPagePreview: true
                            );
                        }
                        break;
                    #endregion

                    #region 설명
                    case "///":
                        string usage = @"설명:"
                            + Environment.NewLine + "/time - 현재시간을 알려줍니다."
                            + Environment.NewLine + "/? 코인코드 - 업비트 코인의 원화 시세를 알려줍니다. 예시) /? btc"
                            + Environment.NewLine + "/w - 날씨정보를 알려줍니다."
                            + Environment.NewLine + "/s URL주소 - Naver 단축URL 기능"
                            + Environment.NewLine + "/n 검색어 - 검색어를 네이버에서 검색"
                            + Environment.NewLine + "/g 검색어 - 검색어를 구글에서 검색"
                            + Environment.NewLine + "/실검 - 네이버 실시간 검색어를 보여줍니다"
                            + Environment.NewLine + "/h - 휴무일을 알려줍니다."
                            + Environment.NewLine + "/h 2019 - 휴무일을 알려줍니다.(전체 다 보여주는건 없다고함 무능한 공공데이터 놈들"
                            + Environment.NewLine + "/h 2019-12 휴무일을 알려줍니다."
                            + Environment.NewLine + "/b @@@ 맛집 - 네이버 블로그 검색"
                            + Environment.NewLine + "/로또 - 최근 회차의 당첨 정보 확인"
                            + Environment.NewLine + "/내놔 - 로또 번호 추출 1~30개 까지"
                            + Environment.NewLine + "/운세, /f - 오늘의 운세";

                        //+ System.Environment.NewLine + "/phone  - 자신의 전화번호를 공유합니다.";

                        await Bot.SendTextMessageAsync(
                            message.Chat.Id,
                            usage,
                            replyMarkup: new ReplyKeyboardRemove());
                        break;
                        #endregion

                    #region 예시 
                        //// send inline keyboard
                        //case "/inline":
                        //    await Bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

                        //    await Task.Delay(500); // simulate longer running task

                        //    var inlineKeyboard = new InlineKeyboardMarkup(new[]
                        //    {
                        //    new [] // first row
                        //    {
                        //        InlineKeyboardButton.WithCallbackData("1.1"),
                        //        InlineKeyboardButton.WithCallbackData("1.2"),
                        //    },
                        //    new [] // second row
                        //    {
                        //        InlineKeyboardButton.WithCallbackData("2.1"),
                        //        InlineKeyboardButton.WithCallbackData("2.2"),
                        //    }
                        //});

                        //    await Bot.SendTextMessageAsync(
                        //        message.Chat.Id,
                        //        "Choose",
                        //        replyMarkup: inlineKeyboard);
                        //    break;

                        //// send custom keyboard
                        //case "/keyboard":
                        //    ReplyKeyboardMarkup ReplyKeyboard = new[]
                        //    {
                        //    new[] { "1.1", "1.2" },
                        //    new[] { "2.1", "2.2" },
                        //};

                        //    await Bot.SendTextMessageAsync(
                        //        message.Chat.Id,
                        //        "Choose",
                        //        replyMarkup: ReplyKeyboard);
                        //    break;

                        //// request location or contact
                        //case "/request":
                        //    var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                        //    {
                        //    KeyboardButton.WithRequestContact("Contact"),
                        //});

                        //    await Bot.SendTextMessageAsync(
                        //        message.Chat.Id,
                        //        "Who or Where are you?",
                        //        replyMarkup: RequestReplyKeyboard);
                        //    break;
                        //case "/phone":
                        //    var RequestReplyKeyboard = new ReplyKeyboardMarkup(new[]
                        //    {
                        //    KeyboardButton.WithRequestContact("내 전화번호 공유하기"),
                        //});

                        //    await Bot.SendTextMessageAsync(
                        //        message.Chat.Id,
                        //        "버튼을 누르면 전화번호를 공유합니다.",
                        //        replyMarkup: RequestReplyKeyboard);
                        //    break;
                        #endregion
                }
            }
            #endregion
        }
        #endregion

        public static async void BotOnCallbackQueryReceived(object sender, CallbackQueryEventArgs callbackQueryEventArgs)
        {
            var callbackQuery = callbackQueryEventArgs.CallbackQuery;

            await Bot.AnswerCallbackQueryAsync(
                callbackQuery.Id,
                $"Received {callbackQuery.Data}");

            await Bot.SendTextMessageAsync(
                callbackQuery.Message.Chat.Id,
                $"Received {callbackQuery.Data}");
        }

        //public static async void BotOnInlineQueryReceived(object sender, InlineQueryEventArgs inlineQueryEventArgs)
        //{
        //    Console.WriteLine($"Received inline query from: {inlineQueryEventArgs.InlineQuery.From.Id}");

        //    InlineQueryResultBase[] results = {
        //    new InlineQueryResultLocation(
        //        id: "1",
        //        latitude: 40.7058316f,
        //        longitude: -74.2581888f,
        //        title: "New York")   // displayed result
        //        {
        //            InputMessageContent = new InputLocationMessageContent(
        //                latitude: 40.7058316f,
        //                longitude: -74.2581888f)    // message if result is selected
        //        },

        //    new InlineQueryResultLocation(
        //        id: "2",
        //        latitude: 13.1449577f,
        //        longitude: 52.507629f,
        //        title: "Berlin") // displayed result
        //        {
        //            InputMessageContent = new InputLocationMessageContent(
        //                latitude: 13.1449577f,
        //                longitude: 52.507629f)   // message if result is selected
        //        }
        //};

        //    await Bot.AnswerInlineQueryAsync(
        //        inlineQueryEventArgs.InlineQuery.Id,
        //        results,
        //        isPersonal: true,
        //        cacheTime: 0);
        //}

        public static void BotOnChosenInlineResultReceived(object sender, ChosenInlineResultEventArgs chosenInlineResultEventArgs)
        {
            Console.WriteLine($"Received inline result: {chosenInlineResultEventArgs.ChosenInlineResult.ResultId}");
        }

        public static void BotOnReceiveError(object sender, ReceiveErrorEventArgs receiveErrorEventArgs)
        {
            Console.WriteLine("Received error: {0} — {1}",
                receiveErrorEventArgs.ApiRequestException.ErrorCode,
                receiveErrorEventArgs.ApiRequestException.Message);
        }

        #region 시간기반 메시지 보내기
        public static void OnTimedEventSendMessage(Object source, ElapsedEventArgs e)
        {
            // 플래그가 true 일때
            if (GlobalData.Variable._telegramTimeFlag)
            {
                if ((e.SignalTime.Hour == 12 || e.SignalTime.Hour == 17) && e.SignalTime.Minute == 0 && e.SignalTime.Second == 0)
                {
                    if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                    {
                        var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                        foreach (var item in _chatID)
                        {
                            Bot.SendTextMessageAsync(item, GetWeatherApi());
                        }
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, GetWeatherApi());
                    }
                }

                if ("Saturday".Equals(e.SignalTime.DayOfWeek.ToString()) || "Sunday".Equals(e.SignalTime.DayOfWeek.ToString()))
                {
                    //Console.WriteLine($"휴일 {e.SignalTime.DayOfWeek}");
                    return;
                }

                //02,05,08,11,14,17,20,23
                if (e.SignalTime.Hour == 7 && e.SignalTime.Minute == 30 && e.SignalTime.Second == 0)
                {
                    if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                    {
                        var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                        foreach (var item in _chatID)
                        {
                            Bot.SendTextMessageAsync(item, GetWeatherApi());
                        }
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, GetWeatherApi());
                    }
                }

                #region TEST
                //if (e.SignalTime.Minute == 0 && e.SignalTime.Second == 0)
                //{
                //    //Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"각 시간 메시지 {e.SignalTime}");
                //    Console.WriteLine($"각 시간 메시지 {e.SignalTime}");
                //}
                //else if (e.SignalTime.Minute % 30 == 0 && e.SignalTime.Second == 0)
                //{
                //    //Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"30분 마다 메시지 {e.SignalTime}");
                //    Console.WriteLine($"30분 마다 메시지 {e.SignalTime}");
                //}
                //else if (e.SignalTime.Minute % 5 == 0 && e.SignalTime.Second == 0)
                //{
                //    //Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"5분 마다 메시지 {e.SignalTime}");
                //    Console.WriteLine($"5분 마다 메시지 {e.SignalTime}");
                //}
                //else 
                //if (e.SignalTime.Second % 2 == 0)
                //{
                //    if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                //    {
                //        var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                //        foreach (var item in _chatID)
                //        {
                //            //Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"2초 마다 메시지 {e.SignalTime}");
                //            Bot.SendTextMessageAsync(item, $"2초 마다 메시지 {e.SignalTime}");
                //            Console.WriteLine($"2초 마다 메시지 {e.SignalTime}");
                //            //Console.WriteLine(GetWeatherApi());
                //        }
                //    }
                //    else
                //    {
                //        Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"2초 마다 메시지 {e.SignalTime}");
                //        Console.WriteLine($"2초 마다 메시지 {e.SignalTime}");
                //    }
                //}
                #endregion

                if (e.SignalTime.Hour == 18 && e.SignalTime.Minute == 0 && e.SignalTime.Second == 0)
                {
                    if ("Friday".Equals(e.SignalTime.DayOfWeek.ToString()))
                    {
                        if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                        {
                            var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                            foreach (var item in _chatID)
                            {
                                Bot.SendTextMessageAsync(item, $"🕕 퇴근시간 입니다. 다음주에 봬요.");
                            }
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"🕕 퇴근시간 입니다. 다음주에 봬요.");
                        }
                    }
                    else
                    {
                        if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                        {
                            var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                            foreach (var item in _chatID)
                            {
                                Bot.SendTextMessageAsync(item, $"🕕 퇴근시간 입니다. 들어가세요");
                            }
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"🕕 퇴근시간 입니다. 들어가세요");
                        }
                    }
                }

                if (e.SignalTime.Hour == 19 && e.SignalTime.Minute == 0 && e.SignalTime.Second == 0)
                {
                    if ("Friday".Equals(e.SignalTime.DayOfWeek.ToString()))
                    {
                        if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                        {
                            var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                            foreach (var item in _chatID)
                            {
                                Bot.SendTextMessageAsync(item, $"🕖 퇴근시간 입니다. 다음주에 봬요.");
                            }
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"🕖 퇴근시간 입니다. 다음주에 봬요.");
                        }
                    }
                    else
                    {
                        if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                        {
                            var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                            foreach (var item in _chatID)
                            {
                                Bot.SendTextMessageAsync(item, $"🕖 퇴근시간 입니다. 들어가세요");
                            }
                        }
                        else
                        {
                            Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"🕖 퇴근시간 입니다. 들어가세요");
                        }
                    }
                }

                if (e.SignalTime.Hour == 12 && e.SignalTime.Minute == 30 && e.SignalTime.Second == 0)
                {
                    if (GlobalData.Variable._telegramChatId.IndexOf(",") > -1)
                    {
                        var _chatID = GlobalData.Variable._telegramChatId.Split(',');
                        foreach (var item in _chatID)
                        {
                            Bot.SendTextMessageAsync(item, $"🕧 점심 식사 맛있게 하세요.");
                        }
                    }
                    else
                    {
                        Bot.SendTextMessageAsync(GlobalData.Variable._telegramChatId, $"🕧 점심 식사 맛있게 하세요.");
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 날씨정보
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static string GetWeatherApi()
        {
            string output = string.Empty;

            ApiClient api = new ApiClient();

            string hour = "";

            var nowHour = DateTime.Now.Hour;
            string emo = string.Empty;

            //02,05,08,11,14,17,20,23
            if (nowHour == 2 || nowHour == 3 || nowHour == 4)
            {
                emo = "🕑";
                hour = "0200";
            }
            else if (nowHour == 5 || nowHour == 6 || nowHour == 7)
            {
                emo = "🕓";
                hour = "0500";
            }
            else if (nowHour == 8 || nowHour == 9 || nowHour == 10)
            {
                emo = "🕗";
                hour = "0800";
            }
            else if (nowHour == 11 || nowHour == 12 || nowHour == 13)
            {
                emo = "🕚";
                hour = "1100";
            }
            else if (nowHour == 14 || nowHour == 15 || nowHour == 16)
            {
                emo = "🕑";
                hour = "1400";
            }
            else if (nowHour == 17 || nowHour == 18 || nowHour == 19)
            {
                emo = "🕓";
                hour = "1700";
            }
            else if (nowHour == 20 || nowHour == 21 || nowHour == 22)
            {
                emo = "🕗";
                hour = "2000";
            }
            else if (nowHour == 23 || nowHour == 0 || nowHour == 1)
            {
                emo = "🕚";
                hour = "2300";
            }

            var obj = (JObject)JsonConvert.DeserializeObject(api.GetWether(hour));

            var objWeather = obj["response"];

            if ("0000".Equals(objWeather.SelectToken("header").SelectToken("resultCode").ToString()))
            {
                var oWeatherItem = objWeather.SelectToken("body").SelectToken("items").SelectToken("item");

                output += $"최종 업데이트 시간 {emo}" + Environment.NewLine;
                output += "예보 시간 " + oWeatherItem[0]["fcstTime"].ToString().Substring(0, 2) + "시" + Environment.NewLine;

                for (var i = 0; i < oWeatherItem.Count(); i++)
                {
                    if (oWeatherItem[0]["fcstTime"].ToString().Equals(oWeatherItem[i]["fcstTime"].ToString()))
                    {
                        switch (oWeatherItem[i]["category"].ToString())
                        {
                            //강수확률
                            case "POP":
                                output += "☔️ : " + oWeatherItem[i]["fcstValue"].ToString() + "%" + Environment.NewLine;
                                break;
                            ////6시간 강수량범주
                            //case "R06":
                            //    output += "(" + oWeatherItem[i]["fcstValue"].ToString() + ")" + Environment.NewLine;
                            //    break;
                            case "REH":
                                //습도
                                output += "💦 : " + oWeatherItem[i]["fcstValue"].ToString() + "%" + Environment.NewLine;
                                break;
                            //case "TMN":
                            //    category = "아침 최저기온 : ";
                            //    value = oWeatherItem[i]["fcstValue"].ToString();
                            //    unit = "℃" + Environment.NewLine;
                            //    _mm = string.Empty;
                            //    break;
                            //case "TMX":
                            //    category = "낮 최고기온 : ";
                            //    value = oWeatherItem[i]["fcstValue"].ToString();
                            //    unit = "℃" + Environment.NewLine;
                            //    _mm = string.Empty;
                            //    break;
                            default:
                                break;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// 코인시세
        /// </summary>
        /// <param name="coincode"></param>
        /// <returns></returns>
        public static string GetExchangeApi(string coincode)
        {
            if (coincode.IndexOf("?") > -1)
            {
                return @"설명:" + Environment.NewLine + "/? 코인코드 - 업비트 코인의 원화 시세를 알려줍니다. 예시) /? SC";
            }

            string output = string.Empty;

            // 플래그가 true 일때
            if (GlobalData.Variable._telegramTickerFlag)
            {
                ApiClient api = new ApiClient();

                string upbitStatus = (api.GetTicker("upbit", coincode).ToLower().IndexOf("error") > -1) ? "404" : "0";
                string bithumbStatus = (api.GetTicker("bithumb", coincode).ToLower().IndexOf("error") > -1) || (api.GetTicker("bithumb", coincode).ToLower().IndexOf("5500") > -1) ? "5500" : "0";

                if ("404".Equals(upbitStatus) && "5500".Equals(bithumbStatus))
                {
                    return "시세정보가 거래소에 없습니다.";
                }

                string coinName = string.Empty;

                var objNM = JsonConvert.DeserializeObject<JArray>(api.GetName()).ToObject<List<JObject>>().ToList();
                objNM.ForEach(item =>
                {
                    if (("KRW-" + coincode.ToUpper()).Equals(item.SelectToken("market").ToString()))
                    {
                        coinName = item.SelectToken("korean_name").ToString();
                    }
                });

                if (!"404".Equals(upbitStatus))
                {
                    var objUpbit = JsonConvert.DeserializeObject<JArray>(api.GetTicker("upbit", coincode)).ToObject<List<JObject>>().FirstOrDefault();
                    var uNowPrice = String.Format("{0:###,##0.########}", objUpbit.SelectToken("trade_price"));
                    var uHighPrice = String.Format("{0:###,##0.########}", objUpbit.SelectToken("high_price"));
                    var uLowPrice = String.Format("{0:###,##0.########}", objUpbit.SelectToken("low_price"));

                    output += $"{coinName}" + Environment.NewLine + ""
                            + Environment.NewLine + "업비트"
                            + Environment.NewLine + $"현재가 : {uNowPrice}원"
                            + Environment.NewLine + $"최고가 : {uHighPrice}원"
                            + Environment.NewLine + $"최저가 : {uLowPrice}원";
                }

                if (!"5500".Equals(bithumbStatus))
                {
                    if ("404".Equals(upbitStatus))
                    {
                        output += $"{coincode.ToUpper()}";
                    }

                    JObject jo = (JObject)JsonConvert.DeserializeObject(api.GetTicker("bithumb", coincode));

                    var objBithumb = jo["data"];

                    var bNowPrice = String.Format("{0:###,##0.########}", objBithumb.SelectToken("closing_price"));
                    var bHighPrice = String.Format("{0:###,##0.########}", objBithumb.SelectToken("max_price"));
                    var bLowPrice = String.Format("{0:###,##0.########}", objBithumb.SelectToken("min_price"));

                    bNowPrice = Convert.ToDecimal(bNowPrice).ToString("###,##0.########");
                    bHighPrice = Convert.ToDecimal(bHighPrice).ToString("###,##0.########");
                    bLowPrice = Convert.ToDecimal(bLowPrice).ToString("###,##0.########");

                    output += Environment.NewLine + "============================"
                            + Environment.NewLine + "빗썸"
                            + Environment.NewLine + $"현재가 : {bNowPrice}원"
                            + Environment.NewLine + $"최고가 : {bHighPrice}원"
                            + Environment.NewLine + $"최저가 : {bLowPrice}원";
                }
            }
            Console.WriteLine(output);
            return output;
        }

        /// <summary>
        /// 날씨정보 (위도, 경도)
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lng"></param>
        /// <returns></returns>
        public static string GetWeatherLatLng(int lat, int lng)
        {
            string output = string.Empty;

            ApiClient api = new ApiClient();

            string hour = "";

            var nowHour = DateTime.Now.Hour;
            string emo = string.Empty;

            //02,05,08,11,14,17,20,23
            if (nowHour == 2 || nowHour == 3 || nowHour == 4)
            {
                emo = "🕑";
                hour = "0200";
            }
            else if (nowHour == 5 || nowHour == 6 || nowHour == 7)
            {
                emo = "🕓";
                hour = "0500";
            }
            else if (nowHour == 8 || nowHour == 9 || nowHour == 10)
            {
                emo = "🕗";
                hour = "0800";
            }
            else if (nowHour == 11 || nowHour == 12 || nowHour == 13)
            {
                emo = "🕚";
                hour = "1100";
            }
            else if (nowHour == 14 || nowHour == 15 || nowHour == 16)
            {
                emo = "🕑";
                hour = "1400";
            }
            else if (nowHour == 17 || nowHour == 18 || nowHour == 19)
            {
                emo = "🕓";
                hour = "1700";
            }
            else if (nowHour == 20 || nowHour == 21 || nowHour == 22)
            {
                emo = "🕗";
                hour = "2000";
            }
            else if (nowHour == 23 || nowHour == 0 || nowHour == 1)
            {
                emo = "🕚";
                hour = "2300";
            }

            var obj = (JObject)JsonConvert.DeserializeObject(api.GetWether(hour, lat, lng));

            var objWeather = obj["response"];

            if ("0000".Equals(objWeather.SelectToken("header").SelectToken("resultCode").ToString()))
            {
                var oWeatherItem = objWeather.SelectToken("body").SelectToken("items").SelectToken("item");

                output += $"최종 업데이트 시간 {emo}" + Environment.NewLine;
                output += "예보 시간 " + oWeatherItem[0]["fcstTime"].ToString().Substring(0, 2) + "시" + Environment.NewLine;

                for (var i = 0; i < oWeatherItem.Count(); i++)
                {
                    if (oWeatherItem[0]["fcstTime"].ToString().Equals(oWeatherItem[i]["fcstTime"].ToString()))
                    {
                        switch (oWeatherItem[i]["category"].ToString())
                        {
                            //강수확률
                            case "POP":
                                output += "☔️ : " + oWeatherItem[i]["fcstValue"].ToString() + "%" + Environment.NewLine;
                                break;
                            ////6시간 강수량범주
                            //case "R06":
                            //    output += "(" + oWeatherItem[i]["fcstValue"].ToString() + ")" + Environment.NewLine;
                            //    break;
                            case "REH":
                                //습도
                                output += "💦 : " + oWeatherItem[i]["fcstValue"].ToString() + "%" + Environment.NewLine;
                                break;
                            //case "TMN":
                            //    category = "아침 최저기온 : ";
                            //    value = oWeatherItem[i]["fcstValue"].ToString();
                            //    unit = "℃" + Environment.NewLine;
                            //    _mm = string.Empty;
                            //    break;
                            //case "TMX":
                            //    category = "낮 최고기온 : ";
                            //    value = oWeatherItem[i]["fcstValue"].ToString();
                            //    unit = "℃" + Environment.NewLine;
                            //    _mm = string.Empty;
                            //    break;
                            default:
                                break;
                        }
                    }
                }
            }

            return output;
        }

        /// <summary>
        /// short url
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetShortURL(string url)
        {
            string output = string.Empty;
            ApiClient api = new ApiClient();
            var obj = api.GetShortURL(url);

            if (obj.Item1 == "OK")
            {
                var _data = (JObject)JsonConvert.DeserializeObject(obj.Item2);
                output = _data["result"].SelectToken("url").ToString();
            }
            else
            {
                output = "Error 발생=" + obj.Item1;
            }
            Console.WriteLine(output);

            return output;
        }

        /// <summary>
        /// 번역 (파파고)
        /// </summary>
        /// <param name="transText"></param>
        /// <returns></returns>
        public static string GetTranslate(string transText)
        {
            string output = string.Empty;
            ApiClient api = new ApiClient();

            var obj = api.GetTranslate(transText);

            if (obj.Item1 == "OK")
            {
                var _data = (JObject)JsonConvert.DeserializeObject(obj.Item2);
                output = _data["message"].SelectToken("result").SelectToken("translatedText").ToString();
            }
            else
            {
                output = "Error 발생=" + obj.Item1;
            }
            Console.WriteLine(output);

            return output;
        }

        /// <summary>
        /// 웹 크롤러
        /// </summary>
        /// <param name="site"></param>
        /// <returns></returns>
        public static string GetWebCrawler(string site)
        {
            // 크롤러 인스턴스 생성
            IWebCrawler crawler = new PoliteWebCrawler();
            string realTimeSearch = string.Empty;
            // 옵션과 함께 크롤러 인스턴스 생성할 경우
            // var crawlConfig = new CrawlConfiguration();
            // crawlConfig.CrawlTimeoutSeconds = 1000;
            // crawlConfig.MaxConcurrentThreads = 10;
            // crawlConfig.MaxPagesToCrawl = 10;
            // crawlConfig.UserAgentString = "Mozilla/5.0 (Windows NT 6.1; WOW64; rv:51.0) Gecko/20100101 Firefox/51.0";
            // IWebCrawler crawler = new PoliteWebCrawler(crawlConfig);

            // 이벤트 핸들러 셋업
            crawler.PageCrawlStartingAsync += (s, e) =>
            {
                Console.WriteLine($"Starting : {e.PageToCrawl}");
            };

            crawler.PageCrawlCompletedAsync += (s, e) =>
            {
                CrawledPage pg = e.CrawledPage;
                HtmlDocument doc = new HtmlDocument();

                doc.LoadHtml(pg.Content.Text);
                if ("네이버".Equals(site))
                {
                    realTimeSearch = doc.DocumentNode.SelectSingleNode("//ul[@class='ah_l']").InnerText.Replace("\n\n\n\n\n", "==").Replace("\n", ".").Replace("==", "\r\n").Replace("...", "");

                    if (!"".Equals(realTimeSearch))
                    {
                        var _realTimeSearch = realTimeSearch.Replace("\r\n", "。");
                        _realTimeSearch = _realTimeSearch + "。";
                        var split = _realTimeSearch.Split('.');
                        string str = string.Empty;
                        realTimeSearch = string.Empty;
                        for (var i = 1; i <= split.Length; i++)
                        {
                            split[i] = split[i].Substring(0, split[i].IndexOf("。")).ToString();

                            realTimeSearch += i + "." + "[" + split[i] + "](https://search.naver.com/search.naver?ie=utf8&query=" + split[i].Replace(" ", "+") + ")\r\n";
                            if (i == split.Length)
                            {
                                crawler.Dispose();
                                break;
                            }
                        }
                    }
                }
            };

            //if (url.IndexOf("http://") == -1)
            //{
            //    url = "http://" + url;
            //}

            // 크롤 시작
            string siteUrl = string.Empty;

            switch (site)
            {
                case "네이버":
                    siteUrl = "http://www.naver.com";
                    break;
            }

            Uri uri = new Uri(siteUrl);

            crawler.Crawl(uri);
            return realTimeSearch;
        }

        /// <summary>
        /// 웹 검색
        /// </summary>
        /// <param name="str"></param>
        /// <param name="g"></param>
        /// <returns></returns>
        public static string GetWebSearch(string str, string g)
        {
            Console.WriteLine(str);

            if ("n".Equals(g))
            {
                str = "[" + str + "](https://search.naver.com/search.naver?ie=utf8&query=" + str.Replace(" ", "+") + ")";
            }
            else if ("g".Equals(g))
            {
                str = "[" + str + "](https://www.google.com/search?q=" + str.Replace(" ", "+") + ")";
            }
            return str;
        }

        /// <summary>
        /// 휴일 API
        /// </summary>
        /// <returns></returns>
        public static string GetHoliDayApi(string year, string month)
        {
            string output = string.Empty;

            ApiClient api = new ApiClient();

            var obj = (JObject)JsonConvert.DeserializeObject(api.GetHoliDay(year, month));
            var _obj = obj["response"];
            var _totalCount = Convert.ToInt32(_obj["body"]["totalCount"].ToString());
            if (_totalCount < 1)
            {
                return "휴일이 없네요~ 아~쉬워라~";
            }

            //var objHoliDayCount = _obj.SelectToken("body").SelectToken("items").Count();
            var _item = _obj.SelectToken("body").SelectToken("items").SelectToken("item");
            if (_totalCount > 1)
            {
                for (var i = 0; i < _totalCount; i++)
                {
                    output += $"{_item[i].SelectToken("dateName")}\r\n날짜 : {_item[i].SelectToken("locdate")}\r\n휴일여부 : {_item[i].SelectToken("isHoliday")}\r\n\r\n";
                }
            }
            else
            {
                output += $"{_item.SelectToken("dateName")}\r\n날짜 : {_item.SelectToken("locdate")}\r\n휴일여부 : {_item.SelectToken("isHoliday")}\r\n\r\n";
            }

            return output;
        }

        /// <summary>
        /// 로또 당첨확인
        /// </summary>
        /// <returns></returns>
        public static string GetLottoPrizeNumber()
        {
            string output = string.Empty;
            ApiClient api = new ApiClient();

            var obj = api.GetLottoPrizeNumber();

            if (obj.Item1 == "OK")
            {
                var _data = (JObject)JsonConvert.DeserializeObject(obj.Item2);

                if ("success".Equals(_data["returnValue"].ToString()))
                {
                    output += $"회차 : {_data["drwNo"]}\r\n";
                    output += $"날짜 : {_data["drwNoDate"]}\r\n";
                    output += $"1등 당첨금 (1인) : {Convert.ToDecimal(_data["firstWinamnt"]).ToString("###,##0.########")}\r\n";
                    output += $"1등 당첨 인원 : {_data["firstPrzwnerCo"]}\r\n";
                    output += $"로또 번호 : [{_data["drwtNo1"]}] [{_data["drwtNo2"]}] [{_data["drwtNo3"]}] [{_data["drwtNo4"]}] [{_data["drwtNo5"]}] [{_data["drwtNo6"]}] 보너스 번호 : [{_data["bnusNo"]}]\r\n";
                }
                else
                {
                    output = "Error 발생=" + obj.Item1;
                }
                Console.WriteLine(output);
            }
            return output;
        }

        /// <summary>
        /// 네이버 검색
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetNaverSearch(string str)
        {
            var output = string.Empty;
            ApiClient api = new ApiClient();
            var obj = api.GetNaverSearch(str);
            if (obj.Item1 == "OK")
            {
                var _data = (JObject)JsonConvert.DeserializeObject(obj.Item2);
                var item = _data["items"];
                if ("[]".Equals(item.ToString()) || string.IsNullOrEmpty(item.ToString()))
                {
                    output = "검색 결과가 없습니다.";
                }

                for (var i = 0; i < item.Count(); i++)
                {
                    output += $"[{ToReplace(item[i].SelectToken("title").ToString())}]({item[i].SelectToken("link")})\r\n";
                    output += $"{ToReplace(item[i].SelectToken("description").ToString())}\r\n";
                    output += $"포스팅 날짜 : {item[i].SelectToken("postdate")}\r\n\r\n";
                }
            }
            else
            {
                output = "Error 발생=" + obj.Item1;
            }
            Console.WriteLine(output);

            return output;
        }

        /// <summary>
        /// 로또번호 생성
        /// </summary>
        /// <returns></returns>
        public static string GetLottoNumbers(int num)
        {
            int[] lotto = new int[6];
            Random r = new Random();
            string output = string.Empty;
            for (var x = 0; x < num; x++)
            {
                int random = 0;
                for (int i = 0; i < lotto.Length; i++)
                {
                    random = r.Next(1, 46);
                    if (true == CheckSame(i, random))
                    {
                        lotto[i] = random;
                    }
                    else
                    {
                        i--;
                    }
                }
                Array.Sort(lotto);
                foreach (int i in lotto)
                {
                    output += $"[{i}] ";
                }
                output += "\r\n";
            }

            bool CheckSame(int index, int value)
            {
                for (int i = 0; i < index; i++)
                {
                    if (value == lotto[i])
                    {
                        return false;
                    }
                }
                return true;
            }

            Console.WriteLine(output);
            return output;
        }

        /// <summary>
        /// 오늘의 운세
        /// </summary>
        /// <returns></returns>
        public static string GetFortune()
        {
            var output = string.Empty;
            ApiClient api = new ApiClient();

            for (var i = 1; i < 13; i++)
            {
                (string obj1, string obj2) = api.GetFortune(i.ToString());
                if (obj1 == "OK")
                {
                    var _data = (JObject)JsonConvert.DeserializeObject(obj2);
                    var item = _data["list"];
                    if (i == 1)
                    {
                        output += "오늘의 운세\r\n";
                        output += "공통 : " + _data["summary"];
                    }

                    output += "\r\n\r\n";
                    //쥐(1),소(2),호랑이(3),토끼(4),용(5),뱀(6),말(7),양(8),원숭이(9),닭(10),개(11),돼지(12)
                    //🐁🐂🐅🐇🐉🐍🐎🐑🐒🐓🐕🐖
                    string[] arr = new string[] { "🐭", "🐮", "🐯", "🐰", "🐉", "🐍", "🐴", "🐑", "🐵", "🐔", "🐶", "🐷" };
                    //switch (_data["animal"].ToString())
                    //{
                    //    case "1": output += "🐁"; break;
                    //    case "2": output += "🐂"; break;
                    //    case "3": output += "🐅"; break;
                    //    case "4": output += "🐇"; break;
                    //    case "5": output += "🐉"; break;
                    //    case "6": output += "🐍"; break;
                    //    case "7": output += "🐎"; break;
                    //    case "8": output += "🐑"; break;
                    //    case "9": output += "🐒"; break;
                    //    case "10": output += "🐓"; break;
                    //    case "11": output += "🐕"; break;
                    //    case "12": output += "🐖"; break;
                    //}
                    output += arr[i - 1];

                    foreach (var items in item)
                    {
                        output += $"\r\n{items["year"]} / ";
                        output += $"{items["description"]}";
                    }                    
                }
                else
                {
                    output = "Error 발생=" + obj1;
                }
                //Console.WriteLine(output);
            }

            return output;
        }
        #region replace
        public static string ToReplace(string str)
        {
            return str.Replace("<b>", "").Replace("</b>", "").Replace("[", "").Replace("]", "").Replace("(", "").Replace(")", "").Replace("*", "").Replace("_", "");
        }
        #endregion

        #region isInt
        public static bool isInt(string orgStr)
        {
            if (orgStr == null) return false;
            return System.Text.RegularExpressions.Regex.IsMatch(orgStr, @"^[+-]?\d*$");
        }
        #endregion

        #region 타이핑 모션
        /// <summary>
        /// 타이핑 모션
        /// </summary>
        /// <param name="message"></param>
        public static async void GetTypingMotion(Telegram.Bot.Types.Message message)
        {
            await Bot.SendChatActionAsync
            (
                message.Chat.Id,
                ChatAction.Typing
            );
        }
        #endregion    
    }
}
