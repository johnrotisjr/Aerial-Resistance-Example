namespace Framework_Module.Enums
{
    /// <summary>
    /// Enumerates the different types of movement behaviors that an AI can execute.
    /// Used by AiPattern to define movement logic.
    /// </summary>

    public enum AiMovementType
    {
        None,
        Straight,
        Retreat,
        Wobble,
        Orbit,
        Stalker,
        Hover,
        Kamikaze,
        MoveToPosition,
        Arching,
        Maintain
    }
}