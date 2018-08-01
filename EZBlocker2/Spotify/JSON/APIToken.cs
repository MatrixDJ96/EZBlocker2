using System;

namespace EZBlocker2.Spotify.JSON
{
    class APIToken
    {
        public string Access_Token { get; set; }
        public string Token_Type { get; set; }
        public string Scope { get; set; }
        public int Expires_In { get; set; }

        private string refresh_token = null; 
        public string Refresh_Token
        {
            get => refresh_token;
            set { if (!String.IsNullOrEmpty(value)) refresh_token = value; }
        }
    }
}
