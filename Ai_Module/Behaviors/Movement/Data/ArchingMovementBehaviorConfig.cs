using Framework_Module.Configs.Ai;
using Framework_Module.Enums;
using UnityEngine;

namespace Ai_Module.Behaviors.Movement.Data
{
    /// <summary>
    /// Parameters describing an arcing flight path such as rate,
    /// ellipse ratio and total arc length.
    /// </summary>
    [CreateAssetMenu(fileName = "ArchingMovementPatternData", menuName = "Scriptable Objects/Ai/Movement/Patterns/ArchingMovementPatternData")]
    public class ArchingMovementBehaviorConfig : AiMovementBehaviorConfig
    {
        public override AiMovementType MovementType  => AiMovementType.Arching;
        public float rotationRate = 1f; 
        public float arcRatioX = 1f;  
        public float arcRatioY = 1f;
        public bool clockwise = false;
        public float startDegree = 0;
        public float distanceInDegrees = 90;
    }
}