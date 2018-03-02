namespace EZBlocker2
{
    internal class Track
    {
        private Resource track_resource;
        private Resource artist_resource;
        private Resource album_resource;

        public Resource Track_resource { get => track_resource; set => track_resource = value; }
        public Resource Artist_resource { get => artist_resource; set => artist_resource = value; }
        public Resource Album_resource { get => album_resource; set => album_resource = value; }

        public string Song => track_resource.Name ?? "UNKNOWN";
        public string Artist => artist_resource.Name ?? "UNKNOWN";
        public string Album => album_resource.Name ?? "UNKNOWN";
    }
}
