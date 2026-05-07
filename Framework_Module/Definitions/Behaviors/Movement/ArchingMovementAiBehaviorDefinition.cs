using System;
using Framework_Module.Definitions.Behaviors.Attack;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions.Behaviors.Movement
{
    [Serializable]
    public class ArchingMovementAiBehaviorDefinition : MovementAiBehaviorDefinition
    {
        public override AiMovementType MovementType => AiMovementType.Arching;
        [Header("RevolutionsPerSecond will not change speed only size of the Arch")]
        [SerializeField] private float revolutionsPerSecond = 1f;  
        [SerializeField] private float arcWidthMultiplier = 1f;
        [SerializeField] private float arcHeightMultiplier = 1f;
        [SerializeField] private bool clockwise = false;
        [SerializeField][Range(0.0f, 359.0f)] private float startDegree = 0;
        [SerializeField][Range(0.0f, 359.0f)] private float endDegree = 359;
        
        public float RevolutionsPerSecond => revolutionsPerSecond;  
        public float ArcWidthMultiplier => arcWidthMultiplier;
        public float ArcHeightMultiplier => arcHeightMultiplier;
        public bool Clockwise => clockwise;
        public float StartDegree => startDegree;
        public float EndDegree => endDegree;
    }
}