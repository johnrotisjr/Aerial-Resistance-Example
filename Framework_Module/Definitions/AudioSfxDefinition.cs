using System;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Definitions
{
    [Serializable]
    public struct AudioSfxDefinition
    {
        [SerializeField] private AudioSfxType type;
        [SerializeField] private AudioClip clip;
        
        public AudioSfxType Type => type;
        public AudioClip Clip => clip;
    }
}