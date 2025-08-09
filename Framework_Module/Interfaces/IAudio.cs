namespace Framework_Module.Interfaces
{
    public interface IAudio : IGameService
    {
        public ISfxPlayer SfxPlayer { get; }
        public IMusicPlayer MusicPlayer { get; }
    }
}