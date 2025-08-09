using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct AudioMusicDefinition
    {
        [SerializeField] private AudioMusicType type;
        [SerializeField] private AudioClip clip;

        public AudioMusicType Type => type;
        public AudioClip Clip => clip;
    }
}