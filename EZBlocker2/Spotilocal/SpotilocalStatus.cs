using System;

namespace EZBlocker2
{
    internal class SpotilocalStatus
    {
        private Error error;
        private string playing = "false";
        private string next_enabled = "true";
        private Track track;
        private OpenGraphState open_graph_state = new OpenGraphState();

        public SpotilocalStatus(string message) => error = new Error(message);

        public string Playing { get => playing; set => playing = value.ToLower(); }
        public string Next_enabled { get => next_enabled; set => next_enabled = value.ToLower(); }
        public Error Error { get => error; set => error = value; }
        public Track Track { get => track; set => track = value; }
        public OpenGraphState Open_graph_state { get => open_graph_state; set => open_graph_state = value; }

        public string Message => error.Message;
        public bool IsError => !error.Message.Equals("");
        public bool IsPlaying => playing.Equals("true");
        public bool IsAd => next_enabled.Equals("false");
        public bool IsPrivateSession => open_graph_state.Private_session.Equals("true");
    }
}
