using EZBlocker2.Spotify.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;
using static EZBlocker2.Program;

namespace EZBlocker2.Spotify
{
    static class WebAPI
    {
        private static CustomWebClient client = new CustomWebClient();

        public static string ResponseType { get; } = "code";
        public static string Scope { get; } = "user-read-currently-playing";
        public static string ClientID { get; } = Properties.Settings.Default.ClientId;
        public static string ClientSecret { get; } = Properties.Settings.Default.ClientSecret;

        public static string RedirectUri { get; set; }

        public static string TokenContentType { get; } = "application/x-www-form-urlencoded";
        public static string StatusContentType { get; } = "application/json";

        public static string Authorization { get; } =
            "Basic " +
            System.Convert.ToBase64String(
                System.Text.Encoding.GetEncoding("ISO-8859-1")
                    .GetBytes(ClientID + ":" + ClientSecret)
            );

        public static string AuthorizeUrl { get; set; }
        public static string APIUrl { get; } = "https://accounts.spotify.com/api/token";
        public static string PlayingUrl { get; } = "https://api.spotify.com/v1/me/player/currently-playing";

        public static APIToken APIToken { get; set; } = null;

        public static string Code { get; set; } = null;

        public static event Action OnNewStatus;

        private static Status status;
        public static Status Status
        {
            get => status;
            set
            {
                status = value;

                // call with the correct thread
                if (OnNewStatus != null)
                    OnNewStatus.Invoke();
            }
        }

        public static void GetToken(bool refresh = false)
        {
            client.Headers[HttpRequestHeader.ContentType] = TokenContentType;
            client.Headers[HttpRequestHeader.Authorization] = Authorization;

            NameValueCollection data = HttpUtility.ParseQueryString(string.Empty);

            if (!refresh)
            {
                data.Add("grant_type", "authorization_code");
                data.Add("redirect_uri", RedirectUri);
                data.Add("code", Code);
            }
            else
            {
                data.Add("grant_type", "refresh_token");
                data.Add("refresh_token", APIToken.Refresh_Token);
            }

            try
            {
                byte[] result = client.UploadValues(APIUrl, "POST", data);

                APIToken token = JsonConvert.DeserializeObject<APIToken>(Encoding.UTF8.GetString(result));

                string refresh_token = token?.Refresh_Token ?? APIToken?.Refresh_Token;

                APIToken = token;

                if (refresh_token != null && APIToken != null && APIToken.Refresh_Token != refresh_token)
                    APIToken.Refresh_Token = refresh_token;

                if (refresh)
                    GetStatus();
            }
            catch (WebException ex)
            {
                if (APIToken != null)
                    Status = new Status { Error = ex.Message };

                WriteLog(ex);
            }
        }

        public static async void GetStatus()
        {
            try
            {
                client.Headers[HttpRequestHeader.ContentType] = StatusContentType;
                client.Headers[HttpRequestHeader.Authorization] = APIToken.Token_Type + " " + APIToken.Access_Token;

                byte[] result = await client.DownloadDataTaskAsync(PlayingUrl);

                if (result != null && result.Length > 0)
                    Status = JsonConvert.DeserializeObject<Status>(Encoding.UTF8.GetString(result));
                else
                    Status = new Status { Is_Private = true };
            }
            catch (Exception ex)
            {
                if (ex is WebException wex)
                {

                    if (!(wex.Response is HttpWebResponse response && response.StatusCode == (HttpStatusCode)429))
                    {
                        GetToken(true);
                    }
                    else
                    {
                        Status = new Status { Retry_After = Convert.ToInt32(response.Headers.Get("Retry-After") ?? "0") * 1000 };
                    }
                }

                WriteLog(ex);
            }
        }

        public static void WriteLog(Exception ex)
        {
            try
            {
                List<string> lines = new List<string>
                {
                    DateTime.Now.ToString(),
                    "error = " + ex.Message,
                    "code = " + (Code ?? ""),
                    "access_token = " + (APIToken?.Access_Token ?? ""),
                    "token_type = " + (APIToken?.Token_Type ?? ""),
                    "scope = " +(APIToken?.Scope ?? ""),
                    "refresh_token = " + (APIToken?.Refresh_Token ?? ""),
                    "--------------------------------------------------"
                };
                File.OpenWrite(ezBlockerLog).Close();
                lines.InsertRange(lines.Count, File.ReadAllLines(ezBlockerLog));
                File.WriteAllLines(ezBlockerLog, lines);
            }
            catch { }
        }
    }
}
