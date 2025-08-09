using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct DialogEntryDefinition
    {
        [SerializeField] private AvatarType avatarType;
        public AvatarType AvatarType => avatarType;
        
        [SerializeField] private string text;
        public string Text => text;
    }
}