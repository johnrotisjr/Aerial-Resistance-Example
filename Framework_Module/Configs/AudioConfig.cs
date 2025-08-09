using System;
using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Scriptable Objects/Audio/AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private AudioMusicDefinition[] musicData;
        [SerializeField] private AudioSfxDefinition[] sfxData;

        public AudioMusicDefinition[] MusicData => musicData;
        public AudioSfxDefinition[] SfxData => sfxData;
        
        private void OnValidate()
        {
            HashSet<AudioMusicType> audioMusicTypes = new();
            foreach (var data in musicData)
            {
                if(!audioMusicTypes.Add(data.Type))
                    DebugLogger.Log("Duplicate AudioMusicType found", LogCategory.Framework, LogLevel.Warning);
            }
            
            HashSet<AudioSfxType> audioSfxTypes = new();
            foreach (var data in sfxData)
            {
                if(!audioSfxTypes.Add(data.Type))
                    DebugLogger.Log("Duplicate AudioSfxType found", LogCategory.Framework, LogLevel.Warning);
            }
        }
    }
}
