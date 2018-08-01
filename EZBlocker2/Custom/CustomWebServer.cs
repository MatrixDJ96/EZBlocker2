using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;

namespace EZBlocker2
{
    class CustomWebServer
    {
        private HttpListener listener = null;
        private Thread thread = null;
        private bool running = false;

        public string Prefix { get; internal set; }

        public CustomWebServer()
        {
            listener = new HttpListener();
            Prefix = "http://127.0.0.1:5515/authorization/";
            listener.Prefixes.Add(Prefix);
        }

        public void Start()
        {
            if (!running)
            {
                running = true;

                listener.Start();

                if (thread != null && thread.ThreadState == ThreadState.Running)
                    throw new Exception("Thread is already running");

                if (thread == null || (thread != null && thread.ThreadState == ThreadState.Stopped))
                {
                    thread = new Thread(() =>
                    {
                        while (running)
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

                                StreamWriter response = new StreamWriter(context.Response.OutputStream);

                                if (error)
                                    response.Write(Properties.Resources.AuthorizationError.Replace("{AUTHORIZE_URL}", Spotify.WebAPI.AuthorizeUrl));
                                else
                                    response.Write(Properties.Resources.AuthorizationSuccess);
                                
                                response.Close();
                            }
                            catch { }
                        }
                    });
                    thread.Start();
                }
            }
        }

        public void Stop()
        {
            if (running || thread.ThreadState == ThreadState.Running)
            {
                running = false;
                listener.Stop();
                listener.Close();
            }
        }
    }
}
