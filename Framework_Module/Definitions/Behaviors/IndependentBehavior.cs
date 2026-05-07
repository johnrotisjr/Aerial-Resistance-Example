using System;
using Framework_Module.GameData.TransitionCondition;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.Definitions
{
    public class IndependentBehavior
    {
        public enum IndependentAttackState
        {
            Idle,
            WaitingToStart,
            Starting,
            Running,
            WaitingToExit,
            Exiting
        }
        public IAiBehavior<IVehicle> AiBehavior => aiBehavior;
        public AiTransitionCondition AiTransitionCondition => aiTransitionCondition;
        public bool CanStart => timeAtEnter + enterTimeDelay <= Time.time;
        public bool CanEnd => timeAtExit + exitTimeDelay <= Time.time;
        public IndependentAttackState CurrentState => independentAttackState;

        private float timeAtEnter;
        private float timeAtExit;
        private IndependentAttackState independentAttackState;
        private IAiBehavior<IVehicle> aiBehavior;
        private AiTransitionCondition aiTransitionCondition;
        private readonly IBehaviorFactory<IVehicle> aiBehaviorFactory;
        private float enterTimeDelay;
        private float exitTimeDelay;

        public IndependentBehavior(IBehaviorFactory<IVehicle> aiBehaviorFactory)
        {
            this.aiBehaviorFactory = aiBehaviorFactory;
        }

        public void SetState(IndependentAttackState state)
        {
            if (independentAttackState == state)
                return;

            independentAttackState = state;
            
            timeAtEnter = 0;
            timeAtExit = 0;
            
            switch (state)
            {
                case IndependentAttackState.WaitingToStart:
                    timeAtEnter = Time.time;
                    break;
                case IndependentAttackState.WaitingToExit:
                    timeAtExit = Time.time;
                    break;
                case IndependentAttackState.Starting:
                case IndependentAttackState.Running:
                case IndependentAttackState.Exiting:
                case IndependentAttackState.Idle:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }

        public void SetBehavior(IndependentAttackBehaviorDefinition def, IPlayerController playerController, IVehicleController aiController)
        {
            independentAttackState = IndependentAttackState.Idle;
            aiBehavior = def.AttackBehaviorDefinition == null ? null: aiBehaviorFactory.GetBehavior(def.AttackBehaviorDefinition);
            aiTransitionCondition = CreateNewRuntimeInstance(def.TransitionConditionDefinition);
            enterTimeDelay = def.EnterTimeDelay;
            exitTimeDelay = def.ExitTimeDelay;
            aiTransitionCondition?.Init(playerController, aiController);
            timeAtEnter = 0;
            timeAtExit = 0;
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
    }
}