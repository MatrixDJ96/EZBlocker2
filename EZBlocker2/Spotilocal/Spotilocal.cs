using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Timers;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    internal class Spotilocal
    {
        private static readonly string user_agent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0";
        private static readonly string open_spotify = "https://open.spotify.com";
        private static readonly string localhost = "http://127.0.0.1";

        private static int port = 0;

        private static string oauth;
        private static string csrf;

        public static CustomEmitter emitter = new CustomEmitter();

        private static void Client_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e, Type type = null)
        {
            try
            {
                if (e.Error == null)
                {
                    string result = Encoding.UTF8.GetString(e.Result);

                    if (type == typeof(CsrfToken) || type == typeof(OAuthToken))
                    {
                        if (type == typeof(CsrfToken))
                            csrf = JsonConvert.DeserializeObject<CsrfToken>(result).Token;
                        else if (type == typeof(OAuthToken))
                            oauth = JsonConvert.DeserializeObject<OAuthToken>(result).T;
                        GetStatus();
                    }
                    else if (type == typeof(SpotilocalStatus))
                        emitter.Status = JsonConvert.DeserializeObject<SpotilocalStatus>(result);
                    else
                        throw new Exception("Unknown request type...");
                }
                else
                    throw e.Error;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private static async void GetPort()
        {
            CustomWebClient client = new CustomWebClient();

            int i_port = 4370;
            int f_port = 4390;

            while (i_port <= f_port)
            {
                try
                {
                    client.Timeout = 10;
                    var page = await client.DownloadDataTaskAsync(new Uri(localhost + ":" + i_port.ToString()));
                }
                catch (Exception ex)
                {
                    WebException webException = (WebException)ex;
                    if (webException.Status == WebExceptionStatus.ProtocolError)
                        break;
                    else
                        i_port++;
                }
            }

            if (i_port <= f_port)
            {
                port = i_port;
                GetStatus();
            }
            else
                WriteLog(new Exception("No Spotify port found..."));
        }

        private static void GetCSRF()
        {
            WebClient client = new WebClient();
            client.Headers.Add("Origin", open_spotify);
            // TODO: add timeout

            client.DownloadDataCompleted += (sender, e) => Client_DownloadDataCompleted(sender, e, typeof(CsrfToken));
            client.DownloadDataAsync(new Uri(localhost + ":" + port.ToString() + "/simplecsrf/token.json"));
        }

        private static void GetOAuth()
        {
            WebClient client = new WebClient();
            client.Headers.Add("User-Agent", user_agent);
            // TODO: add timeout

            client.DownloadDataCompleted += (sender, e) => Client_DownloadDataCompleted(sender, e, typeof(OAuthToken));
            client.DownloadDataAsync(new Uri(open_spotify + "/token"));
        }

        public static void GetStatus()
        {
            if (port == 0 || csrf == null || oauth == null)
            {
                if (port == 0)
                    GetPort();
                else if (csrf == null)
                    GetCSRF();
                else if (oauth == null)
                    GetOAuth();
                return;
            }

            WebClient client = new WebClient();
            client.Headers.Add("Origin", open_spotify);
            // TODO: add timeout

            client.DownloadDataCompleted += (sender, e) => Client_DownloadDataCompleted(sender, e, typeof(SpotilocalStatus));
            client.DownloadDataAsync(new Uri(localhost + ":" + port.ToString() + "/remote/status.json" + "?csrf=" + csrf + "&oauth=" + oauth));
        }

        public static void WriteLog(Exception ex)
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
            emitter.Status = new SpotilocalStatus(ex.Message);
        }
    }
}
