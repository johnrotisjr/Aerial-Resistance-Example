using System;
using System.Collections;
using System.Threading.Tasks;
using Framework_Module.Core;
using UnityEngine;

namespace Framework_Module.Scenes
{
    public class SceneTransitionController : MonoBehaviour
    {
        [SerializeField] private CanvasGroup faderCanvasGroup;
        [SerializeField] private float fadeDuration = 1f;

        public async Task FadeOutAsync()
        {
            await Fade(1);
        }

        public async Task FadeInAsync()
        {
            await Fade(0);
        }

        private Task Fade(float targetAlpha)
        {
            var tcs = new TaskCompletionSource<bool>();
            CoroutineRunner.Begin(FadeCoroutine(targetAlpha, tcs));
            return tcs.Task;
        }

        private IEnumerator FadeCoroutine(float target, TaskCompletionSource<bool> tcs)
        {
            faderCanvasGroup.blocksRaycasts = true;

            float start = faderCanvasGroup.alpha;
            float time = 0f;

            while (time < fadeDuration)
            {
                time += Time.deltaTime;
                faderCanvasGroup.alpha = Mathf.Lerp(start, target, time / fadeDuration);
                yield return null;
            }

            faderCanvasGroup.alpha = target;
            faderCanvasGroup.blocksRaycasts = target != 0;
            tcs.SetResult(true);
        }
    }
}