using System;
using System.Runtime.CompilerServices;
using Ai_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.GameData.Ai;
using Framework_Module.GameData.Instructions;
using Framework_Module.Interfaces;
using UnityEngine;

[assembly: InternalsVisibleTo("Game.Tests")]

namespace World_Module.Vehicle_Controller
{
    /// <summary>
    /// Controls the execution of AI behavior trees for an vehicle, including pattern, routine, and phase transitions.
    /// Evaluates transition conditions and executes movement and attack behaviors each frame.
    /// </summary>
    public class AiController : VehicleController, IDebugInfo
    {
        private AiBehaviorFlowController flowController;

        internal AiBehaviorFlowController FlowController => flowController;
        private AiBehaviorExecutor behaviorExecutor;
        private Vector2 enterVelocity;
        private AiSpawnPositioner spawnPositioner;
        public AiState AiState { get; private set; }
        private readonly EventBus eventBus;

        public void InitializeAi(ScriptableObject tree, SpawnMovementInstruction aiSpawnMovementInstruction, IViewportBoundsProvider viewportBoundsProvider,
            IMovementAiBehaviorFactory movementAiBehaviorFactory, IAttackAiBehaviorFactory attackAiBehaviorFactory)
        {
            behaviorExecutor = new AiBehaviorExecutor(attackAiBehaviorFactory, movementAiBehaviorFactory, this);
            flowController = new AiBehaviorFlowController(tree as AiBehaviorSequenceConfig, this, behaviorExecutor);
            spawnPositioner = new AiSpawnPositioner(viewportBoundsProvider, this, aiSpawnMovementInstruction, OnSpawnComplete);
            AiState = AiState.Spawning;
            spawnPositioner.Begin();
        }

        private void OnSpawnComplete()
        {
            flowController.ResetAll();
            behaviorExecutor.Apply(flowController.CurrentBehaviorGroupDefinition, flowController.IndependentAttackBehaviorDefinitions);
            AiState = AiState.Active;
        }
        
        public override void Update()
        {
            if (ControlledVehicle == null)
                return;
            
            if (!ControlledVehicle.IsAlive)
                AiState = AiState.Dead;
            
            switch (AiState)
            {
                case AiState.Spawning:
                    break;
                case AiState.Active:
                    AiBehaviorFlowController.AiDataType aiTransitionMadeFlags = flowController.Update();
                    if (aiTransitionMadeFlags != 0)
                    {
                        var behaviorGroupDef = flowController.CurrentBehaviorGroupDefinition;
                        var independentAttackBehaviorDef = (aiTransitionMadeFlags & AiBehaviorFlowController.AiDataType.BehaviorGroup) != 0 ? null : flowController.IndependentAttackBehaviorDefinitions;
                        behaviorExecutor.Apply(behaviorGroupDef, independentAttackBehaviorDef);
                    }
                    behaviorExecutor.Update();
                    break;
                case AiState.Dead:
                    spawnPositioner.End();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        /// Called every physics frame (movement updates)
        public override void FixedUpdate()
        {
            if (ControlledVehicle == null)
                return;
            if (AiState == AiState.Active)
                behaviorExecutor.FixedUpdate();
        }

        public string GetDebugInfo()
        {
            return $"AiState: {AiState}";
        }
    }
}