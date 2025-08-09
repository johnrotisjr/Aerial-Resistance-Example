using Framework_Module.Enums;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;

namespace Framework_Module.Event.State
{
    /// <summary>
    /// Event triggered when the global game state changes (e.g., from Main Menu to Gameplay).
    /// Used to synchronize UI and systems with state transitions.
    /// </summary>

    //TODO: Cache Events
    public class GameStateChangeEvent : IGameEvent
    {
        public GameStateType OldState;
        public GameStateType NewState;
            
        public GameStateChangeEvent(GameStateType oldGameState, GameStateType newGameState)
        {
            OldState = oldGameState;
            NewState = newGameState;
        }
    }
}