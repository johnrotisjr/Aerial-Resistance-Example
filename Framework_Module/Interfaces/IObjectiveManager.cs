using System.Collections.Generic;

namespace Framework_Module.Interfaces
{
    public interface IObjectiveManager : IGameService
    {
        public IReadOnlyList<IObjectiveState> Primary { get; }
        public IReadOnlyList<IObjectiveState> Secondary{ get; }
        public IReadOnlyList<IObjectiveState> Hidden { get; }
        public IReadOnlyList<IObjectiveState> AllObjectives { get; }

        public void ResetData(int index);
        public void UpdateDistanceTraveled(float distance);
    }
}
