using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.GameData.Databases
{
    [CreateAssetMenu(fileName = "AudioDatabase", menuName = "Scriptable Objects/Audio/AudioDatabase")]
    public class AudioDatabase : ScriptableObject
    {
        [SerializeField] private AudioMusicDefinition[] musicDatabase;
        [SerializeField] private AudioSfxDefinition[] sfxDatabase;

        public AudioMusicDefinition[] MusicDatabase => musicDatabase;
        public AudioSfxDefinition[] SfxDatabase => sfxDatabase;
        
        private void OnValidate()
        {
            HashSet<AudioMusicType> audioMusicTypes = new();
            foreach (var data in musicDatabase)
            {
                if(!audioMusicTypes.Add(data.Type))
                    DebugLogger.Log("Duplicate AudioMusicType found", LogCategory.Framework, LogLevel.Warning);
            }
            
            HashSet<AudioSfxType> audioSfxTypes = new();
            foreach (var data in sfxDatabase)
            {
                if(!audioSfxTypes.Add(data.Type))
                    DebugLogger.Log("Duplicate AudioSfxType found", LogCategory.Framework, LogLevel.Warning);
            }
        }
    }
}
