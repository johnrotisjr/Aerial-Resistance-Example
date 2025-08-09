using UnityEngine;

namespace Framework_Module.Interfaces
{
    /// <summary>
    /// Interface for input commands triggered by axis-based (Vector2) input actions.
    /// </summary>

    public interface IAxisInputCommand
    {
        void Execute(Vector2 axisValue);
    }
}