using System;
using System.Net;
using System.Timers;

namespace EZBlocker2
{
    class CustomWebClient : WebClient
    {
        private int timeout = 0;
        private Timer timer = null;
        private ElapsedEventHandler handler = null;

        public int Timeout
        {
            get => timeout;
            set => timeout = value;
        }

        private void Timer_Timeout(object sender, ElapsedEventArgs e)
        {
            timer = null;
            handler = null;
            CancelAsync();
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            WebRequest request = base.GetWebRequest(address);

            if (timeout > 0)
            {
                // Sync timeout
                request.Timeout = timeout;

                // Async timeout
                timer = new Timer(timeout);
                handler = Timer_Timeout;

                timer.Elapsed += handler;
                timer.Enabled = true;
            }

            return request;
        }
    }
}
