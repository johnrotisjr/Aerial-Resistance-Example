using Framework_Module.Interfaces;
using Framework_Module.Service;
using TMPro;
using UnityEngine;

namespace Ui_Module.HudObjects
{
    internal class PowerLevelHudElement : HudElement
    {
        [SerializeField] private TMP_Text count;
        private IGameData gameData;

        private void Awake()
        {
            gameData = Services.Get<IGameData>();
        }
        public override void UpdateHud()
        {
            count.text = $"Lvl: {gameData.TransientPlayerData.FirePower.ToString()}";
        }
        
        public override void ResetHud()
        {
            count.text = string.Empty;
        }
    }
}
