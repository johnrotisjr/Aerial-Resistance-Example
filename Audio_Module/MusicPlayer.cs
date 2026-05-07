using System.Collections;
using Debug_Module;
using Framework_Module.Core;
using Framework_Module.Definitions;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Audio_Module
{
    public class MusicPlayer : IMusicPlayer
    {
        private AudioSource audioSource;
        private GameObjectPooler gameObjectPooler;
        private IConfigDatabase configDatabase;
        
        public void Inject(GameObjectPooler gameObjectPoolerService, IConfigDatabase configDatabaseService)
        {
            gameObjectPooler = gameObjectPoolerService;
            configDatabase = configDatabaseService;
            audioSource = GetAudioSource();
        }

        private AudioSource GetAudioSource()
        {
            audioSource = gameObjectPooler.Get<AudioSource>(PrefabKey.AudioSource.ToString());
            if(audioSource == null)
            {
                DebugLogger.Log("Unable to get AudioSource", LogCategory.Audio, LogLevel.Warning);
                return null;
            }
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            return audioSource;
        }

        public void Start(AudioMusicType type, bool loop = true, float fadeIn = 0f)
        {
            configDatabase.GetMusicDefinition(type, out AudioMusicDefinition data);

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = data.Clip;
            audioSource.loop = loop;

            if (fadeIn > 0f)
                CoroutineRunner.Begin(FadeIn(fadeIn));
            else
                audioSource.Play();
        }

        public void Stop(float fadeOut = 0f)
        {
            if (fadeOut > 0f)
                CoroutineRunner.Begin(FadeOut(fadeOut));
            else
                audioSource.Stop();
        }

        public void Pause() => audioSource.Pause();
        public void Resume() => audioSource.UnPause();
        public void SetVolume(float volume) => audioSource.volume = volume;

        private IEnumerator FadeIn(float duration)
        {
            float time = 0f;
            audioSource.volume = 0f;
            audioSource.Play();

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(0f, 1f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.volume = 1f;
        }

        private IEnumerator FadeOut(float duration)
        {
            float startVolume = audioSource.volume;
            float time = 0f;

            while (time < duration)
            {
                audioSource.volume = Mathf.Lerp(startVolume, 0f, time / duration);
                time += Time.deltaTime;
                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }
    }
}
