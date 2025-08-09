using Framework_Module.Core;
using Framework_Module.Enums;
using UnityEngine;

namespace Framework_Module.Interfaces
{
    public interface ISfxPlayer
    {
        void Inject(GameObjectPooler gameObjectPoolerService, IConfigDatabase configDatabaseService);
        void PlayOneShot(AudioSfxType type, float volume = 1f);
        void StopAll();
    }
}