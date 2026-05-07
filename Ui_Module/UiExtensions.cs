using System;
using System.Runtime.CompilerServices;
 
using Framework_Module.Enums;
using Framework_Module.Interfaces;
 

[assembly: InternalsVisibleTo("Game.Tests")]
namespace Ui_Module
{
    internal static class UiExtensions
    {
        public static string GetObjectiveText(this IObjectiveState state)
        {
            var ratioText = $"{state.CurrentValue.ToString()}/{state.Objective.TargetValue.ToString()}";
            
            switch (state.Objective.TaskType)
            {
                case ObjectiveTaskType.KillEnemies:
                    return $"Kill {state.Objective.TargetValue.ToString()} enemies {(state.IsComplete ? $"Completed" : ratioText)}";
                case ObjectiveTaskType.DestroyBoss:
                    return $"Destroy {state.Objective.TargetValue.ToString()} boss {(state.IsComplete ? $"Completed" : ratioText)}";
                case ObjectiveTaskType.Collect:
                    return $"Collect {state.Objective.TargetValue.ToString()} Pickups {(state.IsComplete ? $"Completed" : ratioText)}";
                case ObjectiveTaskType.Reach:
                    return $"Reach {state.Objective.TargetValue.ToString()} miles {(state.IsComplete ? $"Completed" : ratioText)}";
                default:
                    throw new ArgumentOutOfRangeException(nameof(state.Objective.TaskType), state.Objective.TaskType, null);
            }
        }

        public static RewardType ObjectiveTypeToRewardType(this ObjectiveType type)
        {
            switch (type)
            {
                case ObjectiveType.Primary:
                    return RewardType.PrimaryObjective;
                case ObjectiveType.Secondary:
                    return RewardType.SecondaryObjective;
                case ObjectiveType.Hidden:
                    return RewardType.HiddenObjective;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}