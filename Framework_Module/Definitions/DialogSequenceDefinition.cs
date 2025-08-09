using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct DialogSequenceDefinition
    {
        [SerializeField] private int missionIndex;
        [SerializeField] private DialogEntryDefinition[] dialogSequence;

        public int MissionIndex => missionIndex;
        public IReadOnlyList<DialogEntryDefinition> DialogSequence => dialogSequence;

        public DialogSequenceDefinition(int missionIndex, DialogEntryDefinition[] dialogSequence)
        {
            this.missionIndex = missionIndex;
            this.dialogSequence = dialogSequence;
        }
    }
}