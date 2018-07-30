using WindowsFormsApp1.JSON.Spotify;

namespace WindowsFormsApp1.JSON
{
    class Response
    {
        public int Progress_ms { get; set; }
        public Item Item { get; set; }
        public bool Is_playing { get; set; }
    }
}
