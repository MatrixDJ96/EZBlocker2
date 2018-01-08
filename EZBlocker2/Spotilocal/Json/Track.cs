namespace EZBlocker2
{
    internal class Track
    {
        private Resource track_resource = new Resource();
        private Resource artist_resource = new Resource();
        private Resource album_resource = new Resource();

        public Resource Track_resource { get => track_resource; set => track_resource = value; }
        public Resource Artist_resource { get => artist_resource; set => artist_resource = value; }
        public Resource Album_resource { get => album_resource; set => album_resource = value; }

        public string Song => (Track_resource.Name == "") ? "UNKNOWN" : Track_resource.Name;
        public string Artist => (Artist_resource.Name == "") ? "UNKNOWN" : Artist_resource.Name;
        public string Album => (Album_resource.Name == "") ? "UNKNOWN" : Album_resource.Name;
    }
}
