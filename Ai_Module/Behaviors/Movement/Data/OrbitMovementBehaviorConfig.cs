using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Configuration for circular or elliptical motion around an origin
    /// point.
    /// </summary>
    [CreateAssetMenu(fileName = "OrbitMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/OrbitMovementPatternData")]
    public class OrbitMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Orbit;
        public float rotationRate = 1f; 
        public float arcRatioX = 1f;  
        public float arcRatioY = 1f;
        public bool clockwise = false;
        public float startDegree = 0;
    }
}