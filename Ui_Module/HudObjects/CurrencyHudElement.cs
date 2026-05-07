using Framework_Module.Event.Ui;
using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Displays the player's current currency value in the HUD.
    /// Automatically updates when the player earns or spends currency.
    /// </summary>

    internal class CurrencyHudElement : HudElement
    {
        [SerializeField] private TMP_Text cashText;
        private IGameData gameData;
        private void Awake()
        {
            cashText.text = "$0";
            gameData = Services.Get<IGameData>();
        }

        public override void UpdateHud()
        {
            cashText.text = $"${gameData.PersistentPlayerData.Cash}";
        }

        public override void ResetHud()
        {
            cashText.text = "$0";
        }
    }
}