using System;
using System.IO;
using System.Net;
using System.Threading;

namespace WindowsFormsApp1
{
    class MyWebServer
    {
        private HttpListener listener = null;
        private Thread thread = null;
        private bool running = false;

        private int port = -1;

        public readonly string Address = "";
        public string Port { get => port.ToString(); }

        public MyWebServer(int port)
        {
            listener = new HttpListener();

            Address = "http://127.0.0.1";
            this.port = port;

            listener.Prefixes.Add(Address + ":" + Port + "/");
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

                                string request = GetRequest(context.Request);
                                SetResponse(context.Response, request);

                                File.AppendAllText("request.log", request + "----------------------" + Environment.NewLine);
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
            if (running)
            {
                running = false;
                listener.Stop();
            }
        }

        private static string GetRequest(HttpListenerRequest request)
        {
            string result = "";

            if (request.HttpMethod == "GET")
            {
                foreach (string item in request.QueryString)
                {
                    result += item + "=" + request.QueryString.Get(item) + Environment.NewLine;
                }
            }
            else
            {
                StreamReader sr = new StreamReader(request.InputStream);

                while (sr.Peek() != -1)
                {
                    result += sr.ReadLine() + Environment.NewLine;
                }

                sr.Close();
            }

            return result;
        }

        private void SetResponse(HttpListenerResponse response, string data)
        {
            StreamWriter sw = new StreamWriter(response.OutputStream);
            sw.Write(data);
            sw.Close();
        }
    }
}
