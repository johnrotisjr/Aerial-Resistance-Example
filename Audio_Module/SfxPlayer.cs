using System.Collections.Generic;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Audio_Module
{
    public class SfxPlayer : ISfxPlayer
    {
        private GameObjectPooler gameObjectPooler;
        private IConfigDatabase configDatabase;
        private readonly List<AudioSource> activeAudioSources = new();

        public void Inject(GameObjectPooler gameObjectPoolerService, IConfigDatabase configDatabaseService)
        {
            gameObjectPooler = gameObjectPoolerService;
            configDatabase = configDatabaseService;
        }

        private AudioSource GetNewAudioSource()
        {
            var audioSource = gameObjectPooler.Get<AudioSource>(PrefabKey.AudioSource.ToString());
            if(audioSource == null)
            {
                DebugLogger.Log("Unable to get AudioSource", LogCategory.Audio, LogLevel.Warning);
                return null;
            }
            activeAudioSources.Add(audioSource);
            audioSource.loop = false;
            audioSource.playOnAwake = false;
            return audioSource;
        }
        
        private void Cleanup(AudioSource audioSource)
        {
            if (audioSource.isPlaying)
            {
                DebugLogger.Log("audio is still playing after waiting.", LogCategory.Audio, LogLevel.Warning);
                audioSource.Stop();
            }

            activeAudioSources.Remove(audioSource);
            gameObjectPooler.Release(PrefabKey.AudioSource.ToString(), audioSource.gameObject);
        }

        public async void PlayOneShot(AudioSfxType type)
        {
            configDatabase.GetSfxDefinition(type, out AudioSfxDefinition data);
            var newAudioSource = GetNewAudioSource();
            if (newAudioSource != null && data.Clip != null)
                newAudioSource.PlayOneShot(data.Clip, data.Volume);
            
            await CoroutineRunner.WaitForSecondsAsync(data.Clip.length+1);
            Cleanup(newAudioSource);
        }

        public void StopAll()
        {
            foreach (var audioSource in activeAudioSources)
            {
                audioSource.Stop();
                gameObjectPooler.Release(PrefabKey.AudioSource.ToString(), audioSource.gameObject);
            }
            activeAudioSources.Clear();
        }

    }
}
