namespace MelodyLink.Models
{
    public class SpotifyMusicTrack : MusicTrack
    {
        public SpotifyMusicTrack(string title, string[] artists) 
            : base(title)
        {
            Artists = CollectArtists(artists);
        }

        private string[] CollectArtists(string[] artists)
        {
            var result = new List<string>();

            foreach (var artist in artists)
            {
                var splitValue = artist.Split("; ");
                result.AddRange(splitValue);
            }

            return [.. result];
        }
    }
}