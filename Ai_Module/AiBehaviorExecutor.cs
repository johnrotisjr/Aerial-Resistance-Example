using System;
using System.Collections.Generic;
using Framework_Module.Definitions;
using Framework_Module.Definitions.BehaviorGroup;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Ai_Module
{
    /// <summary>
    /// Executes the current AI pattern by invoking associated movement and attack behaviors.
    /// Bridges AI logic flow to actual behavior execution.
    /// </summary>

    public class AiBehaviorExecutor
    {
        private readonly IVehicleController vehicleController;
        private readonly IAttackAiBehaviorFactory attackAiBehaviorFactory;
        private readonly IMovementAiBehaviorFactory movementAiBehaviorFactory;
        private IPlayerController playerController;
        private AiBehaviorGroup behaviorGroup;
        public IPlayerController PlayerController => playerController ??= Services.Get<IPlayerController>();
        public delegate void CompletedCycle();
        public event CompletedCycle  OnCompletedMovementCycle;
        private List<IndependentBehavior> independentAttackBehaviors = new List<IndependentBehavior>();
        
#if UNITY_EDITOR
        public IMovementBehavior MovementBehavior => behaviorGroup?.MovementBehavior;
        public IAttackBehavior AttackBehavior => behaviorGroup?.AttackBehavior;
#endif

        public AiBehaviorExecutor(IAttackAiBehaviorFactory attackFactory, IMovementAiBehaviorFactory movementFactory, IVehicleController vehicleController)
        {
            this.vehicleController = vehicleController;
            this.attackAiBehaviorFactory = attackFactory;
            this.movementAiBehaviorFactory = movementFactory;
        }

        public void Apply(AiBehaviorGroupDefinition behaviorGroupDefinition, IReadOnlyList<IndependentAttackBehaviorDefinition> independentAttackBehaviorDefinitions)
        {
            var vehicle = vehicleController.ControlledVehicle;

            if(independentAttackBehaviorDefinitions != null)
            {
                int defIndex = 0;
                for (; defIndex < independentAttackBehaviorDefinitions.Count; defIndex++)
                {
                    if (defIndex == independentAttackBehaviors.Count)
                    {
                        independentAttackBehaviors.Add(new IndependentBehavior(attackAiBehaviorFactory));
                    }

                    var currentBehavior = independentAttackBehaviors[defIndex];
                    currentBehavior.AiBehavior?.End(vehicle);
                    currentBehavior.SetBehavior(independentAttackBehaviorDefinitions[defIndex], PlayerController, vehicleController);
                }

                for (int i = independentAttackBehaviors.Count - 1; i >= defIndex; i--)
                {
                    independentAttackBehaviors[i].AiBehavior?.End(vehicle);
                    independentAttackBehaviors.RemoveAt(i);
                }
            }

            behaviorGroup ??= new AiBehaviorGroup(attackAiBehaviorFactory, movementAiBehaviorFactory);
            
            behaviorGroup.MovementBehavior?.End(vehicle);
            behaviorGroup.AttackBehavior?.End(vehicle);
            
            if(behaviorGroup.MovementBehavior != null)
                behaviorGroup.MovementBehavior.OnCompletedMovementCycle -= CompletedMovementCycle;
            
            behaviorGroup.SetBehaviors(behaviorGroupDefinition);
            behaviorGroup.MovementBehavior?.Start(vehicle);
            behaviorGroup.AttackBehavior?.Start(vehicle);

            if(behaviorGroup.MovementBehavior != null)
                behaviorGroup.MovementBehavior.OnCompletedMovementCycle += CompletedMovementCycle;
        }

        private void CompletedMovementCycle()
        {
            OnCompletedMovementCycle?.Invoke();
        }

        public void Update()
        {
            var vehicle = vehicleController.ControlledVehicle;
            foreach (var iab in independentAttackBehaviors)
            {
                if(iab.AiTransitionCondition == null)
                    continue;

                switch (iab.CurrentState)
                {
                    case IndependentBehavior.IndependentAttackState.Idle:
                        if(iab.AiTransitionCondition.ConditionMet)
                            iab.SetState(IndependentBehavior.IndependentAttackState.WaitingToStart);
                        break;
                    case IndependentBehavior.IndependentAttackState.WaitingToStart:
                        if(!iab.AiTransitionCondition.ConditionMet)
                            iab.SetState(IndependentBehavior.IndependentAttackState.Idle);
                        else if(iab.CanStart)
                            iab.SetState(IndependentBehavior.IndependentAttackState.Starting);
                        break;
                    case IndependentBehavior.IndependentAttackState.Starting:
                        if(!iab.AiTransitionCondition.ConditionMet)
                            iab.SetState(IndependentBehavior.IndependentAttackState.WaitingToExit);
                        else
                        {
                            iab.AiBehavior?.Start(vehicle);
                            iab.SetState(IndependentBehavior.IndependentAttackState.Running);
                        }
                        break;
                    case IndependentBehavior.IndependentAttackState.Running:
                        if(!iab.AiTransitionCondition.ConditionMet)
                            iab.SetState(IndependentBehavior.IndependentAttackState.WaitingToExit);
                        else
                            iab.AiBehavior?.Tick(Time.deltaTime, vehicle);
                        break;
                    case IndependentBehavior.IndependentAttackState.WaitingToExit:
                        if (iab.AiTransitionCondition.ConditionMet)
                            iab.SetState(IndependentBehavior.IndependentAttackState.Running);
                        else if(iab.CanEnd)
                            iab.SetState(IndependentBehavior.IndependentAttackState.Exiting);
                        break;
                    case IndependentBehavior.IndependentAttackState.Exiting:
                        if(iab.AiTransitionCondition.ConditionMet)
                        {
                            iab.SetState(IndependentBehavior.IndependentAttackState.Running);
                        }
                        else
                        {
                            iab.AiBehavior?.End(vehicle);
                            iab.SetState(IndependentBehavior.IndependentAttackState.Idle);
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            behaviorGroup.AttackBehavior?.Tick(Time.deltaTime, vehicle);
        }

        public void FixedUpdate()
        {
            var vehicle = vehicleController.ControlledVehicle;
            behaviorGroup.MovementBehavior?.Tick(Time.fixedDeltaTime, vehicle);
        }
    }
}