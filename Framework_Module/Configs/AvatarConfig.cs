using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs
{
    /// <summary>
    /// </summary>

    [CreateAssetMenu(fileName = "AvatarConfig", menuName = "Scriptable Objects/AvatarConfig")]
    public class AvatarConfig : ScriptableObject
    {
        [SerializeField] private AvatarDefinition[] avatarDefinitions;
        public IReadOnlyCollection<AvatarDefinition> AvatarDefinitions => avatarDefinitions;

        public void OnValidate()
        {
            var set = new HashSet<AvatarType>();
            foreach (var avatarDefinition in avatarDefinitions)
            {
                bool success = set.Add(avatarDefinition.AvatarType);
                if(!success)
                    DebugLogger.Log($"Multiple entries for avatar type {avatarDefinition.AvatarType} detected.",
                        LogCategory.Framework, LogLevel.Warning);
            }
        }
    }
}
