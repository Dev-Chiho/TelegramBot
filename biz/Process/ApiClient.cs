using System;
using CustomBot.model;

namespace CustomBot.biz.Process
{
    public class ApiClient: ProcessApiClient
    {
        /// <summary>
        /// 시세 확인
        /// </summary>
        /// <param name="exchange"></param>
        /// <param name="coinCode"></param>
        /// <returns></returns>
        public string GetTicker(string exchange, string coinCode)
        {
            string url = string.Empty;

            if ("upbit".Equals(exchange))
            {
                url = GlobalData.Variable._callUpbitTickerUrl + coinCode;
            }
            else if ("bithumb".Equals(exchange))
            {
                url = GlobalData.Variable._callBithumbTickerUrl + coinCode;
            }

            return WebRequestGet(url);
        }

        /// <summary>
        /// 코인명 확인
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            var url = GlobalData.Variable._callUpbitCodeNameUrl;

            return WebRequestGet(url);
        }

        /// <summary>
        /// 강수확률, 습도 확인
        /// </summary>
        /// <param name="hour"></param>
        /// <returns></returns>
        public string GetWether(string hour)
        {
            var url = GlobalData.Variable._callWeatherUrl;
            string ymd = DateTime.Now.ToString("yyyyMMdd").ToString();
            string sKey = GlobalData.Variable._openDataServiceKey;

            url += $"serviceKey={sKey}&base_date={ymd}&base_time={hour}&nx=60&ny=127&numOfRows=10&pageNo=1&_type=json";

            return WebRequestGet(url);
        }

        /// <summary>
        /// 강수확률, 습도 확인
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="nx"></param>
        /// <param name="ny"></param>
        /// <returns></returns>
        public string GetWether(string hour, int nx, int ny)
        {
            var url = GlobalData.Variable._callWeatherUrl;
            string ymd = DateTime.Now.ToString("yyyyMMdd").ToString();
            string sKey = GlobalData.Variable._openDataServiceKey;

            url += $"serviceKey={sKey}&base_date={ymd}&base_time={hour}&nx={nx}&ny={ny}&numOfRows=10&pageNo=1&_type=json";

            return WebRequestGet(url);
        }

        /// <summary>
        /// 휴무일 확인
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        public string GetHoliDay(string year, string month)
        {
            string sKey = GlobalData.Variable._openDataServiceKey;
            var url = GlobalData.Variable._callHoliDayUrl;
            url += $"serviceKey={sKey}&solYear={year}&solMonth={month}&numOfRows=100&pageNo=1&_type=json";
            return WebRequestGet(url);
        }

        /// <summary>
        /// 로또 당첨 확인
        /// </summary>
        /// <returns></returns>
        public (string, string) GetLottoPrizeNumber()
        {
            const int turn = 864;
            const string turn_date = "2019-06-22";
            string today = string.Empty;
            string round = string.Empty;

            //if (DateTime.Today.AddDays(Convert.ToInt32(DayOfWeek.Saturday)) > DateTime.Today.AddDays(Convert.ToInt32(DayOfWeek.Saturday) - Convert.ToInt32(DateTime.Today.DayOfWeek)))
            //{
            //    today = DateTime.Today.AddDays(Convert.ToInt32(DayOfWeek.Saturday) - Convert.ToInt32(DateTime.Today.DayOfWeek) - 7).ToShortDateString();
            //}
            //else
            //{
            //    today = DateTime.Today.AddDays(Convert.ToInt32(DayOfWeek.Saturday)).ToShortDateString();
            //}

            today = DateTime.Now.ToShortDateString();

            DateTime T1 = DateTime.Parse(turn_date);
            DateTime T2 = DateTime.Parse(today);

            TimeSpan TS = T2 - T1;

            int diffDay = TS.Days;  //날짜의 차이 구하기

            if (diffDay < 1)
            {
                round = turn.ToString();
            }
            else
            {
                round = (diffDay / 7 + turn).ToString();
            }

            var url = GlobalData.Variable._lottoPrizeNumberURL;
            var postData = $"method=getLottoNumber&drwNo={round}";

            return WebRequestPost(url, postData, "");
        }

        /// <summary>
        /// 번역
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public (string, string) GetTranslate(string text)
        {
            string url = GlobalData.Variable._naverpapagoApiN2mtURL;
            string postData = string.Empty;

            char[] _charArr = text.ToCharArray();

            foreach (char c in _charArr)
            {
                if (char.GetUnicodeCategory(c) == System.Globalization.UnicodeCategory.OtherLetter)
                {
                    postData = "source=ko&target=en&text=" + text;
                }
                else
                {
                    postData = "source=en&target=ko&text=" + text;
                }
            }

            return WebRequestPost(url, postData, "papago");
        }

        /// <summary>
        /// 네이버 블로그 검색
        /// </summary>
        /// <param name="Data"></param>
        /// <returns></returns>
        public (string, string) GetNaverSearch(string Data)
        {
            string url = GlobalData.Variable._naverSearchURL + Data;

            return WebNaverRequestGet(url);
        }

        /// <summary>
        /// 단축 URL
        /// </summary>
        /// <param name="postData"></param>
        /// <returns></returns>
        public (string, string) GetShortURL(string postData)
        {
            string url = GlobalData.Variable._naverApiUrl;

            postData = "url=" + postData;

            return WebRequestPost(url, postData, "short");
        }

        /// <summary>
        /// 오늘의 운세
        /// </summary>
        /// <param name="animal"></param>
        /// <returns></returns>
        public (string, string) GetFortune(string animal)
        {
            string url = GlobalData.Variable._fortuneURL;
            string y = DateTime.Now.ToString("yyyy");
            string m = DateTime.Now.ToString("MM");
            string d = DateTime.Now.ToString("dd");

            url = url + $"targetYear={y}&targetMonth={m}&targetDay={d}&animal={animal}";

            return WebRequestTupleGet(url);
        }
    }
}
