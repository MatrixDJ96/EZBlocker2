using EZBlocker2.Spotify.JSON;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Windows.Forms;

namespace EZBlocker2.Spotify
{
    static class WebAPI
    {
        private static CustomWebClient client;
        
        public static string ResponseType { get; } = "code";
        public static string Scope { get; } = "user-read-currently-playing";
        public static string ClientID { get; } = "b6058737c9a048ea88a7282b25374c2c";

        public static string RedirectUri { get; set; }

        public static string ContentType { get; } = "application/x-www-form-urlencoded";
        public static string Authorization { get; } = "Basic YjYwNTg3MzdjOWEwNDhlYTg4YTcyODJiMjUzNzRjMmM6NzIyMDMzZDA4YTk0NGU2MmI1ODI2ZjFhOTkzNzczNTg=";

        public static string APIUrl { get; } = "https://accounts.spotify.com/api/token";

        public static event Action<Status> NewStatus;

        private static string code = null;
        public static string Code
        {
            get => code;
            set
            {
                code = value;
                Task.Factory.StartNew(() => GetToken());
            }
        }
        public static APIToken APIToken { get; set; } = null;

        public static string GrantType(bool refresh)
        {
            return refresh ? "refresh_token" : "authorization_code";
        }

        private static void GetToken(bool refresh = false)
        {
            client = new CustomWebClient();
            client.Headers[HttpRequestHeader.ContentType] = ContentType;
            client.Headers[HttpRequestHeader.Authorization] = Authorization;

            NameValueCollection data = HttpUtility.ParseQueryString(string.Empty);

            if (!refresh)
            {
                data.Add("grant_type", GrantType(false));
                data.Add("redirect_uri", RedirectUri);
                data.Add("code", Code);
            }
            else
            {
                data.Add("grant_type", GrantType(true));
                data.Add("refresh_token", APIToken.Refresh_Token);
            }

            try
            {
                byte[] result = client.UploadValues(APIUrl, "POST", data);
                APIToken = JsonConvert.DeserializeObject<APIToken>(Encoding.UTF8.GetString(result));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static async void GetStatus()
        {
            try
            {
                client.Headers[HttpRequestHeader.Authorization] = APIToken.Token_Type + " " + APIToken.Access_Token;
                byte[] result = await client.DownloadDataTaskAsync("https://api.spotify.com/v1/me/player/currently-playing");
                
                Status status = null;
                if (result != null && result.Length > 0)
                    status = JsonConvert.DeserializeObject<Status>(Encoding.UTF8.GetString(result));
                else
                    status = new Status() { IsPrivateSession = true };

                NewStatus.Invoke(status);
            }
            catch (WebException ex)
            {
                if (ex.Response is HttpWebResponse response && response.StatusCode == HttpStatusCode.Unauthorized)
                    GetToken(true);
                else
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
