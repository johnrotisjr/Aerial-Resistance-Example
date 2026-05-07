using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    /// <summary>
    /// </summary>

    [CreateAssetMenu(fileName = "AvatarDatabase", menuName = "Scriptable Objects/AvatarDatabase")]
    public class AvatarDatabase : ScriptableObject
    {
        [SerializeField] private AvatarDefinition[] database;
        public IReadOnlyCollection<AvatarDefinition> Database => database;

        public void OnValidate()
        {
            var set = new HashSet<AvatarType>();
            foreach (var avatarDefinition in database)
            {
                bool success = set.Add(avatarDefinition.AvatarType);
                if(!success)
                    DebugLogger.Log($"Multiple entries for avatar type {avatarDefinition.AvatarType} detected.",
                        LogCategory.Framework, LogLevel.Warning);
            }
        }
    }
}
