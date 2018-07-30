namespace WindowsFormsApp1.JSON.Spotify
{ 
    class Item
    {
        //public bool Is_local { get; set; }
        public string Name { get; set; }
        public Info Album { get; set; }
        public Info[] Artists { get; set; }
        public int Duration_ms { get; set; }
    }
}