using Framework_Module.Event.Objective;
using TMPro;
using UnityEngine;
using Framework_Module.Core;
using Framework_Module.Extensions;

namespace Ui_Module.HudObjects
{
    /// <summary>
    /// Represents a single objective status UI element.
    /// Supports animated fade-in and status transitions.
    /// </summary>

    internal class ObjectiveToastElement : HudElement
    {
        [SerializeField] private TMP_Text objectiveText;

        private Coroutine coroutine;
        
        private void Awake()
        {
            objectiveText.text = "";
        }

        private void Start()
        {
            EventBus.Subscribe<ObjectiveCompleteEvent>(OnObjectiveComplete);
        }

        private void OnDestroy()
        {
            EventBus.Unsubscribe<ObjectiveCompleteEvent>(OnObjectiveComplete);
            if(coroutine != null)
                StopCoroutine(coroutine);
        }

        private void OnObjectiveComplete(ObjectiveCompleteEvent e)
        {
            objectiveText.text = e.State.GetObjectiveText();
            FadeIn();
        }

        private void FadeIn()
        {
            var startColor = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 0);
            var endColor = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 1);
            coroutine = CoroutineRunner.FadeColor(color => objectiveText.color = color, startColor, endColor, 2, DisplayText);
        }

        public override void UpdateHud()
        {
            
        }

        private void DisplayText()
        {
            coroutine = CoroutineRunner.WaitForSeconds(2, FadeOut);
        }
        
        private void FadeOut()
        {
            var startColor = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 1);
            var endColor = new Color(objectiveText.color.r, objectiveText.color.g, objectiveText.color.b, 0);
            coroutine = CoroutineRunner.FadeColor(color => objectiveText.color = color, startColor, endColor, 2, ()=>objectiveText.text = "");
        }

        public override void ResetHud()
        {
            objectiveText.text = "";
        }
    }
}