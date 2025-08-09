using Framework_Module.Interfaces;

namespace Framework_Module.Event.Ui
{
    public class DisplayScreenEvent : IGameEvent
    {
        public IGameScreen GameScreen;
        public DisplayScreenEvent(IGameScreen gameScreen)
        {
            GameScreen = gameScreen;
        }
    }
}