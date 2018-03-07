using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using static EZBlocker2.Program;

namespace EZBlocker2
{
    internal class Spotilocal
    {
        private static Exception ex = new Exception();

        private static readonly string user_agent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:58.0) Gecko/20100101 Firefox/58.0";
        private static readonly string open_spotify = "https://open.spotify.com";
        private static readonly string host = "http://127.0.0.1";
        private static int port = 0;
        private static string oauth;
        private static string csrf;

        private static readonly string hooking = "hooking";
        public static string Hooking => hooking;

        public static CustomEmitter emitter = new CustomEmitter();

        public static string InnerMessage => GetMessage(true);

        private static string GetMessage(bool inner = false)
        {
            string message = "";
            if (ex != null)
            {
                if (!inner)
                    message = ex.Message;
                else
                {
                    if (ex.InnerException != null)
                        message = ex.InnerException.Message;
                }
            }
            return message;
        }

        public static bool GetPort()
        {
            WebRequest request;
            int timeout = 20;

            int i_port = 4370; // initial port
            int f_port = 4390; // final port

            while (i_port <= f_port)
            {
                try
                {
                    request = WebRequest.Create(host + ":" + i_port.ToString());
                    request.Timeout = timeout;
                    request.GetResponse().Close();
                }
                catch (WebException ex)
                {
                    if (ex.Status == WebExceptionStatus.ProtocolError)
                        break;
                    else
                        i_port++;
                }
            }

            if (i_port <= f_port)
            {
                port = i_port;
                return true;
            }
            else
                return false;
        }

        private static void GetCSRF()
        {
            if (csrf == null)
            {
                WebClient client = new WebClient();
                client.Headers.Add("Origin", open_spotify);
                // TODO: add timeout

                client.DownloadDataCompleted += CSRF_DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(host + ":" + port.ToString() + "/simplecsrf/token.json"));
            }
        }

        private static void CSRF_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                csrf = JsonConvert.DeserializeObject<CsrfToken>(Encoding.UTF8.GetString(e.Result)).Token;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private static void GetOAuth()
        {
            if (oauth == null)
            {
                WebClient client = new WebClient();
                client.Headers.Add("User-Agent", user_agent);
                // TODO: add timeout

                client.DownloadDataCompleted += OAuth_DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(open_spotify + "/token"));
            }
        }

        private static void OAuth_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                oauth = JsonConvert.DeserializeObject<OAuthToken>(Encoding.UTF8.GetString(e.Result)).T;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        public static void GetStatus()
        {
            if (GetMessage() != "" || GetMessage(true) != "") // clear exception
                ex = new Exception();

            try
            {
                if (port == 0 && !GetPort())
                    throw new Exception("No Spotify port found");

                if (csrf == null || oauth == null)
                {
                    GetCSRF();
                    GetOAuth();
                    throw new Exception("Hooking to Spotify...", new Exception(hooking));
                }

                WebClient client = new WebClient();
                client.Headers.Add("Origin", open_spotify);
                // TODO: add timeout

                client.DownloadDataCompleted += Status_DownloadDataCompleted;
                client.DownloadDataAsync(new Uri(host + ":" + port.ToString() + "/remote/status.json" + "?csrf=" + csrf + "&oauth=" + oauth));

            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        private static void Status_DownloadDataCompleted(object sender, DownloadDataCompletedEventArgs e)
        {
            try
            {
                emitter.Status = JsonConvert.DeserializeObject<SpotilocalStatus>(Encoding.UTF8.GetString(e.Result));

                if (emitter.Status.Error.Message.ToLower().Contains("csrf"))
                    csrf = null;
                if (emitter.Status.Error.Message.ToLower().Contains("oauth"))
                    oauth = null;
            }
            catch (Exception ex)
            {
                WriteLog(ex);
            }
        }

        public static void WriteLog(Exception ex)
        {
            Spotilocal.ex = ex;
            if (GetMessage(true) != hooking)
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
            }
            emitter.Status = new SpotilocalStatus(ex.Message);
        }
    }
}
