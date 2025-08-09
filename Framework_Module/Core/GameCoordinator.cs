using Framework_Module.Enums;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Service;
using UnityEngine;

namespace Framework_Module.Core
{
    /// <summary>
    /// Orchestrates the setup and teardown of all global and scene-specific services.
    /// Acts as the game's root lifecycle coordinator.
    /// </summary>

    internal class GameCoordinator : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Services.Instance.InitializeServices();
            Services.Instance.Get<EventBus>().Publish(new ServiceInitCompleteEvent());
        }

        private void OnDestroy()
        {
            Services.Instance.Shutdown();
        }
    }
}