namespace Framework_Module.Enums
{
    /// <summary>
    /// Enumerates the possible AI transition trigger types (e.g., time-based).
    /// </summary>

    public enum AiTransitionType
    {
        Time=0,
        HealthPercentBelow=1,
        PlayerInRange=2,
        CycleComplete=3,
        PlayerRelativeLocation=4,
    }
}