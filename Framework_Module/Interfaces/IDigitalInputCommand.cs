namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for input commands triggered by a digital (button) input action.
    /// </summary>

    public interface IDigitalInputCommand
    {
        void Execute(bool isPressed);
    }
}