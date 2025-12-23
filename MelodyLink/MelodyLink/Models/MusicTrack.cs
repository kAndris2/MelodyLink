using System.Text.RegularExpressions;

namespace MelodyLink.Models
{
    public class MusicTrack
    {
        private readonly string title;
        public string Title 
        { 
            get
            {
                return Regex.Replace(title, @"\s*\([^)]*\)\s*$", "");
            }
        }

        public string[] Artists { get; protected set; }

        public string FullName
        {
            get
            {
                return string.Join(", ", Artists) + " - " + Title;
            }
        }

        public MusicTrack(string title)
        {
            this.title = title;
        }

        public MusicTrack(string title, string[] artists)
        {
            this.title = title;
            Artists = artists;
        }
    }
}