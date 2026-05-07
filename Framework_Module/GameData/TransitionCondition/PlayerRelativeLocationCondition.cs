using System;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.TransitionCondition
{
    public sealed class PlayerRelativeLocationCondition : AiTransitionCondition
    {
        private readonly PlayerRelativeLocationConditionDefinition def;
        private float cosHalfAngle = 0;
        private Vector2 directionVector;

        public PlayerRelativeLocationCondition(PlayerRelativeLocationConditionDefinition def)
        {
            this.def = def;
            cosHalfAngle = Mathf.Cos(def.HalfAngle * Mathf.Deg2Rad);
            switch (def.Direction)
            {
                case DirectionType.Down:
                    directionVector = Vector2.down;
                    break;
                case DirectionType.Right:
                    directionVector = Vector2.right;
                    break;
                case DirectionType.Up:
                    directionVector = Vector2.up;
                    break;
                case DirectionType.Left:
                    directionVector = Vector2.left;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public override AiTransitionType TransitionType => AiTransitionType.PlayerRelativeLocation;
        public override bool ConditionMet 
        {
            get
            {
                if (def == null)
                {
                    DebugLogger.Log("No definition provided, condition can never be met!", LogCategory.Ai, LogLevel.Warning);
                    return false;
                }
                
                Vector2 directionToPlayer = (PlayerController.ControlledVehicle.Position - AiVehicleController.ControlledVehicle.Position).normalized;
                bool isInRelativeLocation = Vector2.Dot(directionToPlayer, directionVector) >= cosHalfAngle;

                if (def.FlipLogic)
                    return !isInRelativeLocation;
                
                return isInRelativeLocation;
            }
        }
 
    }
}