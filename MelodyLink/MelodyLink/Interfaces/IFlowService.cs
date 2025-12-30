using MelodyLink.Models;

namespace MelodyLink.Interfaces
{
    public interface IFlowService
    {
        Task<IEnumerable<MusicTrack>?> GetMusicTracks();
    }
}