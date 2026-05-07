using System;
using Framework_Module.Event;
using Framework_Module.Event.System;
using Framework_Module.Service;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module
{
    public class LoadingUi : MonoBehaviour
    {
        [SerializeField] private Image loadingBar;
        private EventBus eventBus;

        private void Awake()
        {
            eventBus = Services.Get<EventBus>();
        }

        void Start()
        {
            eventBus.Subscribe<LoadingProgressEvent>(OnLoadingProgress);
        }

        private void OnDestroy()
        {
            eventBus.Unsubscribe<LoadingProgressEvent>(OnLoadingProgress);
        }

        private void OnLoadingProgress(LoadingProgressEvent e)
        {
            loadingBar.fillAmount = e.Progress;
        }
    }
}
