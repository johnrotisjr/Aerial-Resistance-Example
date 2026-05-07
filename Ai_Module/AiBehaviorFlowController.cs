using System;
using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Definitions.BehaviorGroup;
using Framework_Module.Enums;
using Framework_Module.GameData;
using Framework_Module.GameData.Ai;
using Framework_Module.GameData.TransitionCondition;
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
        [Flags]
        public enum AiDataType
        {
            Phase = 1 << 0,
            Routine= 1 << 1,
            BehaviorGroup= 1 << 2
        }
        
        private readonly AiBehaviorSequenceConfig sequenceConfig;

        private int phaseIndex;
        private int routineIndex;
        private int behaviorGroupIndex;
        
        private AiPhaseDefinition CurrentPhaseDefinition => sequenceConfig.AiPhases[phaseIndex];
        
        private AiRoutineDefinition CurrentRoutineDefinition => CurrentPhaseDefinition.AiPhaseConfig.Routines[routineIndex];

        public AiBehaviorGroupDefinition CurrentBehaviorGroupDefinition =>
            CurrentRoutineDefinition.AiRoutineConfig.AiBehaviorGroups.Count != 0
                ? CurrentRoutineDefinition.AiRoutineConfig.AiBehaviorGroups[behaviorGroupIndex]
                : null;
        
        public IReadOnlyList<IndependentAttackBehaviorDefinition> IndependentAttackBehaviorDefinitions => CurrentRoutineDefinition.AiRoutineConfig.IndependentAttackBehaviors;

        private readonly IPlayerController playerController;
        private readonly IVehicleController aiVehicleController;
        private readonly AiBehaviorExecutor behaviorExecutor;
        
        private AiTransitionCondition phaseTransition = null;
        private AiTransitionCondition routineTransition = null;
        private AiTransitionCondition behaviorGroupTransition = null;

        public AiBehaviorFlowController(AiBehaviorSequenceConfig behaviorSequenceConfig, IVehicleController aiController, AiBehaviorExecutor aiBehaviorExecutor)
        {
            sequenceConfig = behaviorSequenceConfig ? behaviorSequenceConfig : throw new ArgumentNullException(nameof(behaviorSequenceConfig));
            playerController = Services.Get<IPlayerController>();
            aiVehicleController = aiController;
            behaviorExecutor = aiBehaviorExecutor;
            behaviorExecutor.OnCompletedMovementCycle += OnCompletedCycle;
            
            phaseIndex = 0;
            routineIndex = 0;
            behaviorGroupIndex = 0;

            ResetAll();
        }

        ~AiBehaviorFlowController()
        {
            behaviorExecutor.OnCompletedMovementCycle -= OnCompletedCycle;
        }
        
        private void OnCompletedCycle()
        {
            TryIncreaseCycle(behaviorGroupTransition);
        }


        public void ResetAll()
        {
            ResetPhase();
            ResetRoutine();
            ResetPattern();
        }
 
        public AiDataType Update()
        {
            AiDataType transitionMadeFlags = 0;

            if (IsTransitionConditionMet(AiDataType.Phase) && TryAdvancePhase())
            {
                transitionMadeFlags |=  AiDataType.Phase;
            }

            if (IsTransitionConditionMet(AiDataType.Routine) && TryAdvanceRoutine())
            {
                transitionMadeFlags |= AiDataType.Routine;
            }

            if (IsTransitionConditionMet(AiDataType.BehaviorGroup) && TryAdvanceBehaviorGroup())
            {
                transitionMadeFlags |= AiDataType.BehaviorGroup;
            }

            return transitionMadeFlags;
        }
        
        bool IsTransitionConditionMet(AiDataType type)
        {
            AiTransitionCondition instance;
            switch (type)
            {
                case AiDataType.Phase:
                    instance = phaseTransition;
                    break;
                case AiDataType.Routine:
                    instance = routineTransition;
                    break;
                case AiDataType.BehaviorGroup:
                    instance = behaviorGroupTransition;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }

            if (instance?.TransitionConditionFunc != null)
                return instance.TransitionConditionFunc.Invoke();
            
            return instance?.ConditionMet ?? false;
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
            {
                return false;
            }

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
                routineIndex = 0;
                TryIncreaseCycle(phaseTransition);
            }
            else
            {
                TryIncreaseCycle(phaseTransition, 1);
                return false;
            }
 
            ResetRoutine();
            ResetPattern();
            return true;
        }

        private bool TryAdvanceBehaviorGroup()
        {
 
            if (behaviorGroupIndex + 1 < CurrentRoutineDefinition.AiRoutineConfig.AiBehaviorGroups.Count)
                behaviorGroupIndex++;
            else if (CurrentRoutineDefinition.AiRoutineConfig.IsLooped)
            {
                behaviorGroupIndex = 0;
                TryIncreaseCycle(routineTransition);
            }
            else
            {
                TryIncreaseCycle(routineTransition, 1);
                return false;
            }

            ResetPattern();
            return true;
        }
        
        private void TryIncreaseCycle(AiTransitionCondition condition, int setCount = -1)
        {
            if (condition?.TransitionType == AiTransitionType.CycleComplete)
            {
                var cycleCondition = condition.ConditionToType<CycleCompleteCondition>();
                if(setCount == -1)
                    cycleCondition.IncreaseCycles();
                else
                    cycleCondition.SetCycles(setCount);
            }
        }
        
        private void SetupCondition(AiTransitionCondition aiTransitionCondition)
        {
            if (aiTransitionCondition == null)
                return;
            
            //aiTransitionCondition.Reset();
            aiTransitionCondition.Init(playerController, aiVehicleController);
        }

        private void ResetPhase()
        {
            routineIndex = 0;
            phaseTransition = CreateNewRuntimeInstance(CurrentPhaseDefinition.TransitionConditionDefinition);
            SetupCondition(phaseTransition);
        }

        private void ResetRoutine()
        {
            behaviorGroupIndex = 0;
            routineTransition = CreateNewRuntimeInstance(CurrentRoutineDefinition.TransitionConditionDefinition);
            SetupCondition(routineTransition);
        }

        private void ResetPattern()
        {
            if(CurrentBehaviorGroupDefinition == null)
                return;
            behaviorGroupTransition = CreateNewRuntimeInstance(CurrentBehaviorGroupDefinition.TransitionConditionDefinition);
            SetupCondition(behaviorGroupTransition);
        }

        private AiTransitionCondition CreateNewRuntimeInstance(AiTransitionConditionDefinition def)
        {
            if (def is TimeConditionDefinition tc)
            {
                return new TimeCondition(tc);
            }
            
            if(def is HealthPercentBelowConditionDefinition hpc)
            {
                return new HealthPercentBelowCondition(hpc);
            }
            
            if(def is CycleCompleteConditionDefinition ccc)
            {
                return new CycleCompleteCondition(ccc);
            }
            
            if(def is PlayerInRangeConditionDefinition prc)
            {
                return new PlayerInRangeCondition(prc);
            }
            
            if(def is PlayerRelativeLocationConditionDefinition plc)
            {
                return new PlayerRelativeLocationCondition(plc);
            }
            
            return null;
        }
         
        public string GetDebugInfo()
        {
            var movement = behaviorExecutor.MovementBehavior?.GetType().Name ?? "None";
            var attack = behaviorExecutor.AttackBehavior?.GetType().Name ?? "None";

            return
                "Tree:\n" +
                $" - {sequenceConfig.name}\n" +
                $"Phase {phaseIndex}:\n" +
                $" - {CurrentPhaseDefinition.AiPhaseConfig.name ?? "?"}\n" +
                $"Routine {routineIndex}:\n" +
                $" - {CurrentRoutineDefinition.AiRoutineConfig.name ?? "?"}\n" +
                $"Pattern {behaviorGroupIndex}:\n" +
                $" - Move: {movement}\n" +
                $" - Attack: {attack}";
        }
    }
}
