namespace EZBlocker2
{
    internal class OpenGraphState
    {
        private string private_session = "false";

        public string Private_session { get => private_session; set => private_session = value.ToLower(); }
    }
}