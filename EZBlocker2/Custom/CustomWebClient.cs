using System;
using System.Net;
using System.Timers;

namespace EZBlocker2
{
    class CustomWebClient : WebClient
    {
        private Timer timer = null;

        public int Timeout { get; set; } = 5000;
        //public CookieContainer CookieContainer { get; set; }

        public CustomWebClient()
        {
            //CookieContainer = new CookieContainer();
            timer = new Timer(Timeout);
            timer.Elapsed += new ElapsedEventHandler((sender, e) => {
                timer.Stop();
                CancelAsync();
            });
        }
        
        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);
            //((HttpWebRequest)request).CookieContainer = CookieContainer;

            if (Timeout > 0)
            {
                // Sync timeout
                request.Timeout = Timeout;

                // Async timeout
                if (timer.Interval != Timeout)
                    timer.Interval = Timeout;

                timer.Start();
            }

            return request;
        }
    }
}
