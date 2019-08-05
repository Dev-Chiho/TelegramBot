using System;
using System.Configuration;

namespace CustomBot.model
{
    /// <summary>
    /// 멤버 변수 설정
    /// </summary>
    public class GlobalData
    {
        /// <summary>
        /// 싱글톤 Lazy
        /// </summary>
        private static readonly Lazy<GlobalData> _global = new Lazy<GlobalData>(() => new GlobalData());

        public static GlobalData Variable { get { return _global.Value; } }


        public string _telegramUrl = string.Empty;
        public string _telegramToken = string.Empty;
        public string _telegramChatId = string.Empty;

        public bool _telegramUseFlag = true;
        public bool _telegramTimeFlag = true;
        public bool _telegramTickerFlag = true;
        public bool _telegramWeatherFlag = true;

        public string _upbitApiAKey = string.Empty;
        public string _upbitApiSKey = string.Empty;

        public string _callUpbitCodeNameUrl = string.Empty;
        public string _callUpbitTickerUrl = string.Empty;
        public string _callBithumbTickerUrl = string.Empty;

        public string _callWeatherUrl = string.Empty;
        public string _openDataServiceKey = string.Empty;

        public string _naverApiUrl = string.Empty;
        public string _naverClientID = string.Empty;
        public string _naverClientSecret = string.Empty;

        public string _naverPapagoApiUrl = string.Empty;
        public string _naverPapagoClientID = string.Empty;
        public string _naverPapagoClientSecret = string.Empty;

        public string _callHoliDayUrl = string.Empty;

        public string _naverClientID3 = string.Empty;
        public string _naverClientSecret3 = string.Empty;

        public string _naverSearchURL = string.Empty;
        public string _naverpapagoApiN2mtURL = string.Empty;

        public string _naverSearchClientID = string.Empty;
        public string _naverSearchClientSecret = string.Empty;

        public string _lottoPrizeNumberURL = string.Empty;

        public string _fortuneURL = string.Empty;

        private GlobalData()
        {
            #region App Info         
            _telegramUrl = ReadConfig<string>("TelegramBot_url");
            _telegramToken = ReadConfig<string>("TelegramBot_token");
            _telegramChatId = ReadConfig<string>("TelegramBot_chatId");
            _telegramUseFlag = ReadConfig<bool>("TelegramBot_useFlag");
            _telegramTimeFlag = ReadConfig<bool>("TelegramBot_timeFlag");
            _telegramTickerFlag = ReadConfig<bool>("TelegramBot_tickerFlag");
            _telegramWeatherFlag = ReadConfig<bool>("TelegramBot_weatherFlag");

            _callUpbitCodeNameUrl = ReadConfig<string>("UpbitCodeNameUrl");

            _callUpbitTickerUrl = ReadConfig<string>("UpbitTickerUrl");
            _callBithumbTickerUrl = ReadConfig<string>("BIthumbTickerUrl");

            _naverApiUrl = ReadConfig<string>("NaverApiUrl");
            _naverClientID = ReadConfig<string>("Naver_clientID");
            _naverClientSecret = ReadConfig<string>("Naver_clientSecret");

            _naverPapagoApiUrl = ReadConfig<string>("NaverPapagoApiUrl");
            _naverPapagoClientID = ReadConfig<string>("Naver_Papago_clientID");
            _naverPapagoClientSecret = ReadConfig<string>("Naver_Papago_clientSecret");

            _upbitApiAKey = ReadConfig<string>("UpbitApi_accessKey");
            _upbitApiSKey = ReadConfig<string>("UpbitApi_secretKey");

            _callWeatherUrl = ReadConfig<string>("WeatherUrl");
            _openDataServiceKey = ReadConfig<string>("OpenData_serviceKey");

            _callHoliDayUrl = ReadConfig<string>("HoliDayUrl");
            
            _naverClientID3 = ReadConfig<string>("Naver_clientID_3");
            _naverClientSecret3 = ReadConfig<string>("Naver_clientSecret_3");

            _naverSearchURL = ReadConfig<string>("NaverSearchUrl");
            _naverpapagoApiN2mtURL = ReadConfig<string>("NaverPapagoApiN2mtUrl");

            _naverSearchURL = ReadConfig<string>("NaverSearchUrl");
            _naverpapagoApiN2mtURL = ReadConfig<string>("NaverPapagoApiN2mtUrl");

            _naverSearchClientID = ReadConfig<string>("Naver_search_clientID");
            _naverSearchClientSecret = ReadConfig<string>("Naver_search_clientSecret");

            _lottoPrizeNumberURL = ReadConfig<string>("LottoDetailsUrl");

            _fortuneURL = ReadConfig<string>("FortuneUrl");
            #endregion
        }

        /// <summary>
        /// appSettings 설정값 가져오기
        /// </summary>
        T ReadConfig<T>(string strKey, string defaultValue = "")
        {
            object value = ConfigurationManager.AppSettings[strKey] ?? defaultValue;
            return (T)Convert.ChangeType(value, typeof(T));
        }
    }
}
