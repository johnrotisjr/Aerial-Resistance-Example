using System.Collections;
using Framework_Module.Event.Ui;
using UnityEngine;
using UnityEngine.UI;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Displays the player's health using a fillable UI bar.
    /// Smoothly animates health changes using easing transitions.
    /// </summary>

    internal class HealthbarHudElement : HudElement
    {
        [SerializeField] private Image healthBar;
        
        [SerializeField] private EasingType fillEasingType = EasingType.Constant;
        [SerializeField] private float fillAnimationSpeed = 1f;
        
        private enum EasingType
        {
            Constant,
            Exponential
        }

        private Coroutine updateHealthAnimation;

        public override void UpdateHud()
        {
            if(updateHealthAnimation != null)
                StopCoroutine(updateHealthAnimation);
            
            updateHealthAnimation = StartCoroutine(HealthChangeAnimation());
        }

        private IEnumerator HealthChangeAnimation()
        {
            if (PlayerController.ControlledVehicle == null)
                yield break;
            
            var healthPercentageTarget = Mathf.Clamp01(PlayerController.ControlledVehicle.Health / PlayerController.ControlledVehicle.MaxHealth);
            bool isComplete = false;
            while (!isComplete)
            {
                float currentPercent = healthBar.fillAmount;
                float updatedWidth = fillEasingType switch
                {
                    EasingType.Constant => Mathf.MoveTowards(currentPercent, healthPercentageTarget, Time.deltaTime * fillAnimationSpeed),
                    EasingType.Exponential => Mathf.Lerp(currentPercent, healthPercentageTarget, Time.deltaTime * fillAnimationSpeed),
                    _ => healthPercentageTarget
                };

                healthBar.fillAmount = updatedWidth;
                isComplete = Mathf.Abs(updatedWidth - healthPercentageTarget) < 0.001f;
                yield return null;
            }

            healthBar.fillAmount = healthPercentageTarget;
        }

        public override void ResetHud()
        {
            healthBar.fillAmount = 1;
        }
    }
}