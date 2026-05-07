using System;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module.HudObjects
{
    internal class WeaponHudElement : HudElement
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text count;
        private IPlayerController playerController;

        private void Awake()
        {
            playerController = Services.Get<IPlayerController>();
        }

        public override void UpdateHud()
        {
            icon.sprite = playerController.CurrentlyEquippedWeapon()?.Icon;
            count.text = "xInf";
        }
        
        public override void ResetHud()
        {
            icon.sprite = null;
            count.text = string.Empty;
        }
    }
}
