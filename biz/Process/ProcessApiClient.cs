using System.IO;
using System.Net;
using System.Text;
using CustomBot.model;

namespace CustomBot.biz.Process
{
    public class ProcessApiClient
    {
        private static ProcessApiClient _instance;
        public ProcessApiClient()
        {

        }

        public static ProcessApiClient Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new ProcessApiClient();
                }

                return _instance;
            }
        }

        //public virtual string GetTicker(string url, string coinCode)
        //{
        //    return string.Empty;
        //}

        //public virtual string GetName()
        //{
        //    return string.Empty;
        //}

        //public virtual string GetWether(string hour)
        //{
        //    return string.Empty;
        //}

        //public virtual string GetWether(string hour, int nx, int ny)
        //{
        //    return string.Empty;
        //}

        public string WebRequestGet(string url)
        {
            WebRequest webRequest = WebRequest.Create(url);
            webRequest.Method = "GET";

            WebResponse webResponse = null;
            try
            {
                webResponse = webRequest.GetResponse();
            }
            catch (WebException ex)
            {
                webResponse = ex.Response;
            }

            StreamReader sr = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8);
            return sr.ReadToEnd();
        }

        public (string, string) WebRequestTupleGet(string url)
        {
            string text = string.Empty;
            string status = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            status = response.StatusCode.ToString();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();

            return (status, text);
        }

        public (string, string) WebNaverRequestGet(string url)
        {
            string text = string.Empty;
            string status = string.Empty;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Headers.Add("X-Naver-Client-Id", GlobalData.Variable._naverSearchClientID); // 개발자센터에서 발급받은 Client ID
            request.Headers.Add("X-Naver-Client-Secret", GlobalData.Variable._naverSearchClientSecret); // 개발자센터에서 발급받은 Client Secret
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            status = response.StatusCode.ToString();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();            

            return (status, text);
        }

        public (string, string) WebRequestPost(string url, string postData, string kind)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            
            if ("papago".Equals(kind))
            {
                request.Headers.Add("X-Naver-Client-Id", GlobalData.Variable._naverClientID3); // 개발자센터에서 발급받은 Client ID
                request.Headers.Add("X-Naver-Client-Secret", GlobalData.Variable._naverClientSecret3); // 개발자센터에서 발급받은 Client Secret
            }
            else if ("short".Equals(kind))
            {
                request.Headers.Add("X-Naver-Client-Id", GlobalData.Variable._naverClientID); // 개발자센터에서 발급받은 Client ID
                request.Headers.Add("X-Naver-Client-Secret", GlobalData.Variable._naverClientSecret); // 개발자센터에서 발급받은 Client Secret
            }
            else { 
                request.ContentType = "application/x-www-form-urlencoded";
                request.Headers.Add("Authorization", "Bearer " + ""); //사용시 추가
            }

            request.Method = "POST";

            string text = string.Empty;
            string status = string.Empty;
            
            byte[] byteDataParams = Encoding.UTF8.GetBytes(postData);

            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteDataParams.Length;
            Stream st = request.GetRequestStream();
            st.Write(byteDataParams, 0, byteDataParams.Length);
            st.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream stream = response.GetResponseStream();
            status = response.StatusCode.ToString();
            response.StatusCode.ToString();
            StreamReader reader = new StreamReader(stream, Encoding.UTF8);
            text = reader.ReadToEnd();
            stream.Close();
            response.Close();
            reader.Close();

            return (status, text);
        }
    }
}
