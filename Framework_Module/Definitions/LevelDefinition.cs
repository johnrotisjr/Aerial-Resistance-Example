using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct LevelDefinition
    {
        [SerializeField] private int[] unlockRequirementsIndices;
        [SerializeField] string displayName;
        public string DisplayName => displayName;        
        public IReadOnlyCollection<int> UnlockRequirementsIndices => unlockRequirementsIndices;

        public LevelDefinition(string displayName, int[] unlockRequirementsIndices)
        {
            this.displayName = displayName;
            this.unlockRequirementsIndices = unlockRequirementsIndices;
        }
    }
}