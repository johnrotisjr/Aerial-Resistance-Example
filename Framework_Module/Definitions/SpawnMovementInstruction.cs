using System;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct SpawnMovementInstruction
    {
        [SerializeField] private bool isUsingEndPosition;
        [SerializeField] private Vector3 endPosition;
        
        public bool IsUsingEndPosition => isUsingEndPosition;
        public Vector3 EndPosition => endPosition;

        public SpawnMovementInstruction(bool isUsingEndPosition) : this(isUsingEndPosition, Vector3.zero) { }
        
        public SpawnMovementInstruction(bool isUsingEndPosition, Vector3 endPosition)
        {
            this.isUsingEndPosition = isUsingEndPosition;
            this.endPosition = endPosition;
        }
    }
}