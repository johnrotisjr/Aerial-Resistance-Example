namespace Framework_Module.Enums
{
    /// <summary>
    /// Enumerates the possible AI transition trigger types (e.g., time-based).
    /// </summary>

    public enum AiTransitionType
    {
        Time,
        HealthPercentBelow,
        PlayerInRange,
        CycleComplete,
        Custom,
    }
}