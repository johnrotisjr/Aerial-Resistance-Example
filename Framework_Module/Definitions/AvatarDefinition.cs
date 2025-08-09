using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct AvatarDefinition
    {
        [SerializeField] private Sprite sprite;
        public Sprite Sprite => sprite;
        [SerializeField] private AvatarType avatarType;
        public AvatarType AvatarType => avatarType;
    }
}