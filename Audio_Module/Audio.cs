using System;
using Framework_Module.Core;
using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Interfaces;
using UnityEngine;

namespace Audio_Module
{
    public class Audio : MonoBehaviour, IAudio
    {
        [SerializeField] private GameObject audioSourcePrefab;
        public ISfxPlayer SfxPlayer { get; private set; }
        public IMusicPlayer MusicPlayer { get; private set; }
        private GameObjectPooler gameObjectPooler;
        private EventBus eventBus;
        private IConfigDatabase config;

        public void Inject(EventBus eventBusService, GameObjectPooler gameObjectPoolerService, IMusicPlayer musicPlayer,
            ISfxPlayer sfxPlayer, IConfigDatabase configDatabase)
        {
            eventBus = eventBusService;
            gameObjectPooler = gameObjectPoolerService;
            config = configDatabase;
            gameObjectPooler.Register(PrefabKey.AudioSource.ToString(), audioSourcePrefab);
            SfxPlayer = sfxPlayer;
            MusicPlayer = musicPlayer;
            
            SfxPlayer.Inject(gameObjectPoolerService, config);
            MusicPlayer.Inject(gameObjectPoolerService, config);
        }

        public void Initialize()
        {
            MusicPlayer.SetVolume(1f);
            eventBus.Subscribe<PlaySfxEvent>(OnPlaySfxEvent);
        }

        private void OnPlaySfxEvent(PlaySfxEvent e)
        {
            SfxPlayer.PlayOneShot(e.Type);
        }

        public void Shutdown()
        {
            eventBus.Unsubscribe<PlaySfxEvent>(OnPlaySfxEvent);
            MusicPlayer.Stop();
            SfxPlayer.StopAll();
            gameObjectPooler.UnRegister(PrefabKey.AudioSource.ToString());
        }
    }
}
