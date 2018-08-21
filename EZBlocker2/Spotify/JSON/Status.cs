using System;

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

    class Error
    {
        public string Message { get; set; }
        public static implicit operator Error(string message) => new Error { Message = message };
    }

    class Status
    {
        public Error Error { get; set; }

        public int Progress_ms { get; set; }
        public Item Item { get; set; }
        public bool Is_Playing { get; set; }

        public bool Is_Private { get; set; } = false;
        public bool Is_Ads { get => Item == null; }
        
        public int Retry_After { get; set; } = 0;
    }
}
