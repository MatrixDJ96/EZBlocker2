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
        private static int port = 0;

        private static string oauth;
        private static string csrf;

        public static int Timeout => timeout;

        public static CustomEmitter emitter = new CustomEmitter();

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

        private static void CSRF_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                csrf = JsonConvert.DeserializeObject<CsrfToken>(e.Result).Token;

                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", user_agent);
                // TODO: add timeout

                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(OAuth_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(open_spotify + "/token"));
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        private static void OAuth_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                oauth = JsonConvert.DeserializeObject<OAuthToken>(e.Result).T;

                WebClient client = new WebClient();
                client.Headers.Add("Origin", open_spotify);
                // TODO: add timeout

                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(Status_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(host + ":" + port.ToString() + "/remote/status.json" + "?csrf=" + csrf + "&oauth=" + oauth));
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        private static void Status_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            try
            {
                emitter.Status = JsonConvert.DeserializeObject<SpotilocalStatus>(e.Result);

                if (emitter.Status.Error.Message.ToLower().Contains("csrf"))
                    csrf = null;
                if (emitter.Status.Error.Message.ToLower().Contains("oauth"))
                    oauth = null;
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        public static void GetStatus()
        {
            try
            {
                if (port == 0 && !GetPort())
                    throw new Exception("No Spotify port found");

                WebClient client = new WebClient();
                client.Headers.Add("Origin", open_spotify);
                // TODO: add timeout

                client.DownloadStringCompleted += new DownloadStringCompletedEventHandler(CSRF_DownloadStringCompleted);
                client.DownloadStringAsync(new Uri(host + ":" + port.ToString() + "/simplecsrf/token.json"));
            }
            catch (Exception ex)
            {
                WriteLog(ex.Message);
            }
        }

        public static void WriteLog(string message)
        {
            try
            {
                List<string> lines = new List<string>
                    {
                        DateTime.Now.ToString(),
                        "port=" + port.ToString(),
                        "csrf=" + csrf,
                        "oauth=" + oauth,
                        "error=" + message,
                        "-------------------"
                    };
                File.AppendAllLines(ezBlockerLog, contents: lines);
            }
            catch { }
            emitter.Status = new SpotilocalStatus(message);
        }
    }
}
