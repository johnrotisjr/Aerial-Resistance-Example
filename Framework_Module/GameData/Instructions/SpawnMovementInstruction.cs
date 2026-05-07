using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Framework_Module.GameData.Instructions
{
    [Serializable]
    public struct SpawnMovementInstruction
    {
        [SerializeField] private bool isEnabled;
        [FormerlySerializedAs("viewportX")] [SerializeField][Range(0, 1.0f)] private float viewportXPercent;
        [FormerlySerializedAs("viewportY")] [SerializeField][Range(0, 1.0f)] private float viewportYPercent;
        
        public bool IsEnabled => isEnabled;
        public float ViewportXPercent => viewportXPercent;
        public float ViewportYPercent => viewportYPercent;

        public SpawnMovementInstruction(bool isEnabled, float viewportXPercent = 0, float viewportYPercent = 0)
        {
            this.isEnabled = isEnabled;
            this.viewportXPercent = viewportXPercent;
            this.viewportYPercent = viewportYPercent;
        }
    }
}