using Framework_Module.Interfaces;

namespace Framework_Module.Event.Ui
{
    public class HideScreenEvent : IGameEvent
    {
        public IGameScreen GameScreen;
        public HideScreenEvent(IGameScreen gameScreen)
        {
            GameScreen = gameScreen;
        }
    }
}