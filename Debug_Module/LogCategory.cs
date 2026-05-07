
namespace Debug_Module
{
    /// <summary>
    /// Defines log categories for organizing and filtering debug output.
    /// Used by DebugLogger to separate concerns by system.
    /// </summary>

    public enum LogCategory
    {
        None = 0,
        Ai,
        Audio,
        Bootstrap,
        Data,
        Debug,
        Framework,
        Input,
        Story,
        Test,
        Tools,
        Ui,
        World
    }
}