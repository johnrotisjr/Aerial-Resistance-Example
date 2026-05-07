using System;
using UnityEngine;

namespace World_Module
{
    public class ParallaxScroller : MonoBehaviour
    {
        [SerializeField] private float globalScrollSpeed = 1f;
        [SerializeField] private ParallaxLayer[] layers;

        private void LateUpdate()
        {
            float deltaTime = Time.deltaTime;

            foreach (var layer in layers)
                layer.Scroll(globalScrollSpeed, deltaTime);
        }
    }
}