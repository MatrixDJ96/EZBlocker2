using System;
using System.Net;
using System.Timers;

namespace EZBlocker2
{
    class CustomWebClient : WebClient
    {
        private int timeout = 60000;
        private Timer timer = null;
        private ElapsedEventHandler handler = null;

        public CustomWebClient()
        {
            timer = new Timer(timeout);
            handler = Timer_Timeout;
            timer.Elapsed += handler;
        }

        public int Timeout
        {
            get => timeout;
            set => timeout = value;
        }

        private void Timer_Timeout(object sender, ElapsedEventArgs e)
        {
            timer.Stop();
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
                if (timer.Interval != timeout)
                    timer.Interval = timeout;
                
                timer.Start();
            }

            return request;
        }
    }
}
