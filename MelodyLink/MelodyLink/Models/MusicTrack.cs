namespace MelodyLink.Models
{
    public class MusicTrack
    {
        public string Title { get; private set; }
        public string[] Artists { get; protected set; }
        public string FullName
        {
            get
            {
                return string.Join(", ", Artists) + Title;
            }
        }

        public MusicTrack(string title)
        {
            Title = title;
        }

        public MusicTrack(string title, string[] artists)
        {
            Title = title;
            Artists = artists;
        }
    }
}