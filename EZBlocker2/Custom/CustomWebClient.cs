using System;
using System.Net;
using System.Timers;

namespace EZBlocker2
{
    class CustomWebClient : WebClient
    {
        private int timeout;

        public int Timeout
        {
            get => timeout;
            set
            {
                timeout = value;

                Timer timer = new Timer(timeout);
                ElapsedEventHandler handler = null;

                handler = ((sender, args) =>
                {
                    timer.Elapsed -= handler;
                    CancelAsync();
                });

                timer.Elapsed += handler;
                timer.Enabled = true;
            }
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (timeout >= 0)
                request.Timeout = timeout;

            return request;
        }
    }
}
