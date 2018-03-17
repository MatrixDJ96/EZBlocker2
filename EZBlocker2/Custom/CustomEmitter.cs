using System;

namespace EZBlocker2
{
    internal class CustomEmitter
    {
        private SpotilocalStatus status;
        public SpotilocalStatus Status
        {
            get { return status; }
            set
            {
                status = value;
                OnNewStatus(status);
            }
        }
        public event Action<SpotilocalStatus> NewStatus;

        public void OnNewStatus(SpotilocalStatus status)
        {
            NewStatus(status);
        }
    }
}
