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
        [SerializeField, Range(0.0f, 2.0f)] private float volume;
        
        public AudioSfxType Type => type;
        public AudioClip Clip => clip;
        public float Volume => volume;
    }
}