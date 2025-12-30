using System.Text.RegularExpressions;

namespace MelodyLink.Models
{
    public class MusicTrack
    {
        private readonly string _title;
        public string Title 
        { 
            get
            {
                return Regex.Replace(_title, @"\s*\([^)]*\)\s*$", "");
            }
        }

        public string[] Artists { get; protected set; }

        private readonly string? _bracket;
        public string? Bracket 
        { 
            get
            {
                return ValidateBracket();
            }
        }

        public string FullName
        {
            get
            {
                return string.Join(", ", Artists) + " - " + Title + (Bracket == null ? "" : $" ({Bracket})");
            }
        }

        public MusicTrack(string title)
        {
            _title = title;
        }

        public MusicTrack(string title, string[] artists, string? bracket = null)
        {
            _title = title;
            _bracket = bracket;
            Artists = artists;
        }

        private string? ValidateBracket()
        {
            if (_bracket == null) return null;

            return Artists.Any(artist => _bracket.Contains(artist, StringComparison.OrdinalIgnoreCase)) ? null : _bracket;
        }
    }
}