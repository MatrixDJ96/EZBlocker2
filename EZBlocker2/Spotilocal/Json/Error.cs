namespace EZBlocker2
{
    internal class Error
    {
        private string message;

        public Error(string message) => this.message = message ?? "";

        public string Message { get => message; set => message = value; }
    }
}