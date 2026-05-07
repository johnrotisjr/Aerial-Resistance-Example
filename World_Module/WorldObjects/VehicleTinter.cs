using System.Collections;
using Framework_Module.Core;
using Framework_Module.Interfaces;
using UnityEngine;

namespace World_Module.WorldObjects
{
    public class VehicleTinter : MonoBehaviour, IVehicleTinter
    {
        [SerializeField] private SpriteRenderer[] renderers;
        private Color[] originalSpriteColors;

        private void Awake()
        {
            originalSpriteColors = new Color[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
            {
                originalSpriteColors[i] = renderers[i].color;
            }
        }

        public void Tint(Color color)
        {
            foreach (var sr in renderers)
            {
                sr.color = color;
            }
        }

        public void ResetTint()
        {
            StopAllCoroutines();
            for (var i = 0; i < renderers.Length; i++)
            {
                renderers[i].color = originalSpriteColors[i];
            }
        }

        public void TintOverTime(Color color)
        {
            for (var i = 0; i < renderers.Length; i++)
            {
                StartCoroutine(Tinting(color, renderers[i], originalSpriteColors[i]));
            }
        }
        
        private IEnumerator Tinting(Color end, SpriteRenderer sr, Color originalSpriteColor)
        {
            var startColor = originalSpriteColor;
            var endColor = end;
            yield return CoroutineRunner.FadeColor((color) => sr.color = color, startColor, endColor, .1f);
            startColor = endColor;
            endColor = originalSpriteColor;
            yield return CoroutineRunner.FadeColor((color) => sr.color = color, startColor, endColor, .1f);
            sr.color = endColor;
        }
    }
}