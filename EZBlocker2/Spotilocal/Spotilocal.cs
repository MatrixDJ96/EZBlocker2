using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    internal class Spotilocal
    {
        private static readonly string ezBlockerLog = Path.GetFileNameWithoutExtension(ezBlockerFullExe) + ".log";

        private static readonly string user_agent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/62.0.3202.94 Safari/537.36";
        private static readonly string open_spotify = "https://open.spotify.com";
        private static readonly string host = "http://127.0.0.1";
        private static readonly int timeout = 2000;
        private static int port = 0; // to initialize with GetPort()

        private static string oauth;
        private static string csrf;

        public static string User_Agent => user_agent;
        public static int Timeout => timeout;

        public static bool GetPort()
        {
            WebRequest request;
            int timeout = 20;

            int port = 4370; // default port
            int f_port = 4390; // final port

            while (port <= f_port)
            {
                try
                {
                    request = WebRequest.Create(host + ":" + port.ToString());
                    request.Timeout = timeout;
                    request.GetResponse().Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        break;
                    else
                        port++;
                }
            }

            if (port <= f_port)
            {
                Spotilocal.port = port;
                return true;
            }
            else
                return false;
        }

        private static string GetCSRF()
        {
            if (port == 0)
                if (!GetPort())
                    throw new Exception("No Spotify port found");

            WebRequest request = WebRequest.Create(host + ":" + port.ToString() + "/simplecsrf/token.json");
            request.Headers.Add("Origin", open_spotify);
            request.Timeout = timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string json = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            csrf = JsonConvert.DeserializeObject<CsrfToken>(json).Token;
            return csrf;
        }

        private static string GetOAuth()
        {
            WebRequest request = WebRequest.Create(open_spotify + "/token");
            ((HttpWebRequest)request).UserAgent = user_agent;
            request.Timeout = timeout;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            Stream dataStream = response.GetResponseStream();
            StreamReader reader = new StreamReader(dataStream);
            string json = reader.ReadToEnd();

            reader.Close();
            dataStream.Close();
            response.Close();

            oauth = JsonConvert.DeserializeObject<OAuthToken>(json).T;
            return oauth;
        }

        public static SpotilocalStatus GetStatus()
        {
            try
            {
                if (csrf == null)
                    GetCSRF();
                if (oauth == null)
                    GetOAuth();

                WebRequest request = WebRequest.Create(host + ":" + port.ToString() + "/remote/status.json" + "?csrf=" + csrf + "&oauth=" + oauth);
                request.Headers.Add("Origin", open_spotify);
                request.Timeout = timeout;

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                StreamReader reader = new StreamReader(dataStream);
                string json = reader.ReadToEnd();

                reader.Close();
                dataStream.Close();
                response.Close();

                SpotilocalStatus status = JsonConvert.DeserializeObject<SpotilocalStatus>(json);

                if (status.Error.Message.ToLower().Contains("csrf"))
                    csrf = null;
                if (status.Error.Message.ToLower().Contains("oauth"))
                    oauth = null;

                return status;
            }
            catch (Exception ex)
            {
                try
                {
                    List<string> lines = new List<string>
                    {
                        DateTime.Now.ToString(),
                        "port=" + port.ToString(),
                        "csrf=" + csrf,
                        "oauth=" + oauth,
                        "error=" + ex.Message,
                        "-------------------"
                    };
                    File.AppendAllLines(ezBlockerLog, contents: lines);
                }
                catch { }
                return new SpotilocalStatus(ex.Message);
            }
        }
    }
}
