namespace EZBlocker2.Spotify.JSON
{
    class Info
    {
        public string Name { get; set; }
    }

    class Item
    {
        public string Name { get; set; }
        public Info Album { get; set; }
        public Info[] Artists { get; set; }
        public int Duration_ms { get; set; }
    }

    class Status
    {
        public int Progress_ms { get; set; }
        public Item Item { get; set; }
        public bool Is_Playing { get; set; }
        
        public bool IsPrivateSession { get; set; } = false;
        public bool IsAds { get => Item == null; } 
    }
}
