namespace WindowsFormsApp1.JSON
{
    class Authorization
    {
        public string Access_token { get; set; } = null;
        public string Token_type { get; set; } = null;
        public string Scope { get; set; } = null;
        public int Expires_in { get; set; } = -1;
        public string Refresh_token { get; set; } = null;
    }
}
