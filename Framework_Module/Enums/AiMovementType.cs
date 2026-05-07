namespace Framework_Module.Enums
{
    /// <summary>
    /// Enumerates the different types of movement behaviors that an AI can execute.
    /// Used by AiPattern to define movement logic.
    /// </summary>

    public enum AiMovementType
    {
        None,
        Linear,
        Wobble,
        Orbit,
        Stalker,
        Kamikaze,
        MoveToPosition,
        Arching,
        Maintain,
        Patrol,
        Teleport
    }
}