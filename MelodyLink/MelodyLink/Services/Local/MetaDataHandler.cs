using System.Text.RegularExpressions;
using MelodyLink.Models;

namespace MelodyLink.Services.Local
{
    public class MetaDataHandler
    {
        public MusicTrack GetMusicTrack(string filePath)
        {
            var file = TagLib.File.Create(filePath);
            var title = file.Tag.Title;
            var contributors = file.Tag.Performers;
            var bracket = GetBracketContent(filePath);

            return new MusicTrack(title, contributors, bracket);
        }

        private string GetBracketContent(string filePath)
        {
            string fileName = Path.GetFileNameWithoutExtension(filePath);
            var match = Regex.Match(fileName, @"\((.*?)\)");
            return match.Success ? match.Groups[1].Value : null;
        }
    }
}