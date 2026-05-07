using System;
using System.Collections.Generic;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct DialogTrackDefinition
    {
        [SerializeField] private DialogEntryDefinition[] dialogSequence;
        public IReadOnlyList<DialogEntryDefinition> DialogSequence => dialogSequence;

        public DialogTrackDefinition(DialogEntryDefinition[] dialogSequence)
        {
            this.dialogSequence = dialogSequence;
        }
    }
}