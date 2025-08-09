using System;
using Framework_Module.Configs.Ai;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module
{
    /// <summary>
    /// Manages the logic flow through an AI behavior tree by handling transitions between phases, routines, and patterns.
    /// Tracks timing and looping logic for AI state changes.
    /// </summary>

    public class AiBehaviorFlowController : IDebugInfo
    {
        private enum AiDataType
        {
            Phase,
            Routine,
            PatternPair
        }
        
        private readonly AiBehaviorSequenceConfig sequenceConfig;
        private readonly AiBehaviorExecutor behaviorExecutor;

        private int phaseIndex;
        private int routineIndex;
        private int patternIndex;

        private float phaseExpiration;
        private float routineExpiration;
        private float patternExpiration;

        private int routinePatternsCycles;
        private int phaseRoutinesCycles;
        
        private AiPhaseDefinition CurrentPhaseDefinition => sequenceConfig.AiPhases[phaseIndex];
        private AiRoutineDefinition CurrentRoutineDefinition => CurrentPhaseDefinition.AiPhaseConfig.Routines[routineIndex];
        public AiPatternPairDefinition CurrentPatternDefinition => CurrentRoutineDefinition.AiRoutineConfig.PatternPairs[patternIndex];
        
        private readonly IPlayerController playerController;
        private readonly IVehicleController aiVehicleController;

        public AiBehaviorFlowController(AiBehaviorSequenceConfig behaviorSequenceConfig, IVehicleController aiController, AiBehaviorExecutor aiBehaviorExecutor)
        {
            sequenceConfig = behaviorSequenceConfig ? behaviorSequenceConfig : throw new ArgumentNullException(nameof(behaviorSequenceConfig));
            behaviorExecutor = aiBehaviorExecutor;
            playerController = Services.Instance.Get<IPlayerController>();
            aiVehicleController = aiController;
            
            phaseIndex = 0;
            routineIndex = 0;
            patternIndex = 0;

            ResetAll();
        }

        public void ResetAll()
        {
            ResetPhase();
            ResetRoutine();
            ResetPattern();
        }

        public bool Update()
        {
            bool patternChanged = false;

            if (EvaluateCondition(AiDataType.Phase))
            {
                patternChanged |= TryAdvancePhase();
            }

            if (EvaluateCondition(AiDataType.Routine))
            {
                patternChanged |= TryAdvanceRoutine();
            }

            if (EvaluateCondition(AiDataType.PatternPair))
            {
                patternChanged |= TryAdvancePattern();
            }

            return patternChanged;
        }
        
        bool EvaluateCondition(AiDataType type)
        {
            AiTransitionConditionDefinition definition;
            float expiration;
            int completedCycles = 0;
            
            switch (type)
            {
                case AiDataType.Phase:
                    definition = CurrentPhaseDefinition.ConditionDefinition;
                    expiration = phaseExpiration;
                    completedCycles = phaseRoutinesCycles;
                    break;
                case AiDataType.Routine:
                    definition = CurrentRoutineDefinition.ConditionDefinition;
                    expiration = routineExpiration;
                    completedCycles = routinePatternsCycles;
                    break;
                case AiDataType.PatternPair:
                    definition = CurrentPatternDefinition.ConditionDefinition;
                    expiration = patternExpiration;
                    completedCycles = behaviorExecutor.MovementCycles;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
 
            switch (definition.Type)
            {
                case AiTransitionType.Time:
                    return Time.time > expiration;
                case AiTransitionType.HealthPercentBelow:
                    var aiHealth = aiVehicleController.ControlledVehicle.Health;
                    var targetHealth = definition.TransitionValue * .01 * aiVehicleController.ControlledVehicle.MaxHealth;
                    return aiHealth < targetHealth;
                case AiTransitionType.PlayerInRange:
                    return Vector2.Distance(aiVehicleController.ControlledVehicle.Position, playerController.ControlledVehicle.Position) < definition.TransitionValue;
                case AiTransitionType.Custom:
                    return definition.TransitionCondition?.Invoke() ?? false;
                case AiTransitionType.CycleComplete:
                    return definition.TransitionValue <= completedCycles;
                default:
                    return false;
            }
        }

        private bool TryAdvancePhase()
        {
            if (phaseIndex + 1 < sequenceConfig.AiPhases.Count)
            {
                phaseIndex++;
            }
            else if (sequenceConfig.IsLooped)
            {
                phaseIndex = 0;   
            }
            else
                return false;

            ResetAll();
            return true;
        }

        private bool TryAdvanceRoutine()
        {
            if (routineIndex + 1 < CurrentPhaseDefinition.AiPhaseConfig.Routines.Count)
            {
                routineIndex++;   
            }
            else if (CurrentPhaseDefinition.AiPhaseConfig.IsLooped)
            {
                phaseRoutinesCycles++;
                routineIndex = 0;
            }
            else
                return false;
 
            ResetRoutine();
            ResetPattern();
            return true;
        }

        private bool TryAdvancePattern()
        {
            if (patternIndex + 1 < CurrentRoutineDefinition.AiRoutineConfig.PatternPairs.Count)
                patternIndex++;
            else if (CurrentRoutineDefinition.AiRoutineConfig.IsLooped)
            {
                routinePatternsCycles++;
                patternIndex = 0;
            }
            else
                return false;

            ResetPattern();
            return true;
        }

        private void ResetPhase()
        {
            routineIndex = 0;
            phaseRoutinesCycles = 0;
            phaseExpiration = Time.time + CurrentPhaseDefinition.ConditionDefinition.TransitionValue;
        }

        private void ResetRoutine()
        {
            patternIndex = 0;
            routinePatternsCycles = 0;
            routineExpiration = Time.time + CurrentRoutineDefinition.ConditionDefinition.TransitionValue;
        }

        private void ResetPattern()
        {
            patternExpiration = Time.time + CurrentPatternDefinition.ConditionDefinition.TransitionValue;
        }
        
        public string GetDebugInfo()
        {
            var pattern = CurrentPatternDefinition.AiPatternPairConfig;
            var movement = pattern?.aiMovementBehaviorConfig?.name ?? "None";
            var attack = pattern?.aiAttackBehaviorConfig?.name ?? "None";

            return
                $"Tree: {sequenceConfig.name}\n" +
                $"Phase {phaseIndex}: {CurrentPhaseDefinition.AiPhaseConfig?.name ?? "?"}\n" +
                $"Routine {routineIndex}: {CurrentRoutineDefinition.AiRoutineConfig?.name ?? "?"}\n" +
                $"Pattern {patternIndex}:" +
                $" - Move: {movement}" +
                $" - Attack: {attack}";
        }
    }
}
