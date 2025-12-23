namespace MelodyLink.Interfaces
{
    public interface IFlowService
    {
        Task<IEnumerable<string>?> GetMusicTitles();
    }
}