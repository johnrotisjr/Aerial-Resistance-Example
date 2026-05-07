using System;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using UnityEngine;

namespace Framework_Module.Debug
{
    public class RuntimeDebugger : MonoBehaviour
    {
        [SerializeField] private float displaySeconds = 2f;

        private float showUntil = -1f;
        private string message = String.Empty;
        private GUIStyle style;
        private bool isPlayerInvincible = false;
        private IPlayerController playerController;
        
        private void Awake()
        {
            playerController = Services.Get<IPlayerController>();
        }
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                isPlayerInvincible = !isPlayerInvincible;
                message = $"Invincibility = {isPlayerInvincible}!";
                showUntil = Time.time + displaySeconds;
                SetPlayerInvincibility(isPlayerInvincible);
            }
        }

        private void OnGUI()
        {
            style ??= new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter
            };
            
            if (Time.time <= showUntil)
            {
                float w = 200f, h = 40f;
                var rect = new Rect((Screen.width - w) * 0.5f, 20f, w, h);
                GUI.Box(rect, message, style);
            }
        }

        private void SetPlayerInvincibility(bool isInvincible)
        {
            playerController?.ControlledVehicle?.EnableCollider(!isInvincible);
        }
    }
}