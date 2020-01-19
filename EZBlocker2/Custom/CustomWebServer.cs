using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace EZBlocker2
{
    class CustomWebServer
    {
        private HttpListener listener = null;

        public string Prefix { get; internal set; }

        public CustomWebServer()
        {
            listener = new HttpListener();
            Prefix = "http://127.0.0.1:5515/authorization/";
            listener.Prefixes.Add(Prefix);
        }

        public void Start()
        {
            if (!listener.IsListening)
            {
                listener.Start();
                GetContext();
            }
        }

        public void Stop()
        {
            if (listener.IsListening)
            {
                listener.Stop();
            }
        }

        private void GetContext()
        {
            while (Spotify.WebAPI.APIToken == null)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();

                    bool error = false;

                    if (context.Request.Url.Query != null)
                    {
                        NameValueCollection query = HttpUtility.ParseQueryString(context.Request.Url.Query);

                        if (query.Get("error") == null && query.Get("code") != null)
                        {
                            Spotify.WebAPI.Code = query.Get("code");
                            Spotify.WebAPI.GetToken();
                        }
                    }

                    if (Spotify.WebAPI.APIToken == null)
                        error = true;

                    context.Response.StatusCode = 200;

                    using (var response = new StreamWriter(context.Response.OutputStream))
                    {
                        if (error)
                            response.Write(Properties.Resources.AuthorizationError.Replace("{PRODUCT_NAME}", Program.ProductName).Replace("{AUTHORIZE_URL}", Spotify.WebAPI.AuthorizeUrl));
                        else
                            response.Write(Properties.Resources.AuthorizationSuccess.Replace("{PRODUCT_NAME}", Program.ProductName));
                    }
                }
                catch { }
            }
            Stop();
        }
    }
}
