using System;
using Framework_Module.Enums;
using Framework_Module.GameData.Configuration;
using UnityEngine.Serialization;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct DialogCueDefinition
    {
        public WorldStateType worldStateType;
        [FormerlySerializedAs("dialogTrackConfig")] [FormerlySerializedAs("dialogSequenceConfig")] public DialogTrackData dialogTrackData;
    }
}