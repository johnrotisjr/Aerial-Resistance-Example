using Framework_Module.Enums;
using Framework_Module.Game_State;
using Framework_Module.Interfaces;

namespace Framework_Module.Event.State
{
    /// <summary>
    /// Event triggered when the world game state changes (e.g., from Gameplay to Pause).
    /// Used to synchronize UI and systems with state transitions.
    /// </summary>

    //TODO: Cache Events
    public class WorldStateChangeEvent : IGameEvent
    {
        public WorldStateType OldState;
        public WorldStateType NewState;
            
        public WorldStateChangeEvent(WorldStateType oldGameState, WorldStateType newGameState)
        {
            OldState = oldGameState;
            NewState = newGameState;
        }
    }
}