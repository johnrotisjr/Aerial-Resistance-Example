using System;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Framework_Module.GameData.TransitionCondition
{
    [Serializable]
    public abstract class AiTransitionCondition
    {
        public abstract AiTransitionType TransitionType { get; }
        public abstract bool ConditionMet { get; }
 
        public Func<bool> TransitionConditionFunc;
        public T ConditionToType<T>() where T : AiTransitionCondition => this as T;
        
        public void SetTransitionCondition(Func<bool> transitionCondition) => TransitionConditionFunc = transitionCondition;
        
        protected IPlayerController PlayerController;
        protected IVehicleController AiVehicleController;
        public void Init(IPlayerController player, IVehicleController ai)
        {
            PlayerController = player;
            AiVehicleController = ai;
        }
    }
}