using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.Combat;
using Framework_Module.Event.Objective;
using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using EventBus = Framework_Module.Event.EventBus;

namespace World_Module.Mission.Objective
{
    /// <summary>
    /// Tracks progress on all mission objectives and publishes events when objectives are completed.
    /// Used to monitor mission success conditions.
    /// </summary>
    
    public class ObjectiveManager : IObjectiveManager
    {
        private readonly List<ObjectiveState> primaryObjectives = new();
        private readonly List<ObjectiveState> secondaryObjectives = new();
        private readonly List<ObjectiveState> hiddenObjectives = new();
        private readonly List<ObjectiveState> allObjectives = new();
        
        public IReadOnlyList<IObjectiveState> Primary => primaryObjectives;
        public IReadOnlyList<IObjectiveState> Secondary => secondaryObjectives;
        public IReadOnlyList<IObjectiveState> Hidden => hiddenObjectives;
        public IReadOnlyList<IObjectiveState> AllObjectives => allObjectives;

        private readonly Dictionary<ObjectiveTaskType, List<ObjectiveState>> objectiveLookup = new();

        private readonly EventBus eventBus;
        private readonly IConfigDatabase configDatabase;
        
        public ObjectiveManager(IConfigDatabase configDatabase, EventBus eventBus)
        {
            this.eventBus = eventBus;
            this.configDatabase = configDatabase;
        }

        public void Initialize()
        {
            eventBus.Subscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
            eventBus.Subscribe<BossDestroyedEvent>(OnBossDestroyed);
            eventBus.Subscribe<PickupCollectedEvent>(OnPickupCollected);
            Setup(0);
        }
        
        public void Shutdown()
        {
            if (eventBus != null)
            {
                eventBus.Unsubscribe<WorldObjectKilledEvent>(OnWorldObjectDestroyed);
                eventBus.Unsubscribe<BossDestroyedEvent>(OnBossDestroyed);
                eventBus.Unsubscribe<PickupCollectedEvent>(OnPickupCollected);
            }

            objectiveLookup.Clear();
        }

        public void ResetData(int index)
        {
            primaryObjectives.Clear();
            secondaryObjectives.Clear();
            hiddenObjectives.Clear();
            allObjectives.Clear();
            objectiveLookup.Clear();
            Setup(index);
        }

        private void AddToLookup(ObjectiveState state)
        {
            if (!objectiveLookup.TryGetValue(state.Objective.TaskType, out var list))
            {
                list = new List<ObjectiveState>();
                objectiveLookup.Add(state.Objective.TaskType, list);
            }
            list.Add(state);
        }

        private void Setup(int index)
        {
            var missionData = configDatabase.GetMissionDefinition(index);
            foreach (var objective in missionData.PrimaryObjectivesData)
            {
                var objectiveState = new ObjectiveState(objective, ObjectiveType.Primary);
                primaryObjectives.Add(objectiveState);
                AddToLookup(objectiveState);
            }
            
            foreach (var objective in missionData.SecondaryObjectivesData)
            {
                var objectiveState = new ObjectiveState(objective, ObjectiveType.Secondary);
                secondaryObjectives.Add(objectiveState);
                AddToLookup(objectiveState);
            }
            
            foreach (var objective in missionData.HiddenObjectivesData)
            {
                var objectiveState = new ObjectiveState(objective, ObjectiveType.Hidden);
                hiddenObjectives.Add(objectiveState);
                AddToLookup(objectiveState);
            }
            
            allObjectives.AddRange(primaryObjectives);
            allObjectives.AddRange(secondaryObjectives);
            allObjectives.AddRange(hiddenObjectives);
        }
        
        private void UpdateObjectives(ObjectiveTaskType type, float amount)
        {
            if (!objectiveLookup.TryGetValue(type, out var objectiveStates))
                return;
            
            foreach (var state in objectiveStates)
            {
                if (state.IsComplete)
                    return;
                state.UpdateObjective(amount);
                if (state.IsComplete)
                {
                    eventBus.Publish(new ObjectiveCompleteEvent(state));
                    eventBus.Publish(new UpdateHudEvent());
                }
            }
        }

        private void OnWorldObjectDestroyed(WorldObjectKilledEvent e)
        {
            if (e.WorldObject is IVehicle a && a.AlignmentType == AlignmentType.Foe)
            {
                UpdateObjectives(ObjectiveTaskType.KillEnemies, 1);
            }
        }
        
        private void OnBossDestroyed(BossDestroyedEvent e)
        {
            if (e.WorldObject is IVehicle a && a.AlignmentType == AlignmentType.Foe)
            {
                UpdateObjectives(ObjectiveTaskType.DestroyBoss, 1);
            }
        }

        public void UpdateDistanceTraveled(float distanceTraveled)
        {
            UpdateObjectives(ObjectiveTaskType.Reach, distanceTraveled);
        }
        
        private void OnPickupCollected(PickupCollectedEvent pickupCollectedEvent)
        {
            UpdateObjectives(ObjectiveTaskType.Collect, 1);
        }
 
    }
}
