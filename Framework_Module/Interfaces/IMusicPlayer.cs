using Framework_Module.Core;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface IMusicPlayer
    {
        void Inject(GameObjectPooler gameObjectPoolerService, IConfigDatabase configDatabaseService);
        void Start(AudioMusicType type, bool loop = true, float fadeIn = 0f);
        void Stop(float fadeOut = 0f);
        void Pause();
        void Resume();
        void SetVolume(float volume);
    }
}