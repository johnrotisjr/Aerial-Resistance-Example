using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Framework_Module.Extensions;
using UnityEngine;

namespace Framework_Module.Core
{
    public class CoroutineRunner : SingletonMonoBehavior<CoroutineRunner>
    {
        private static readonly List<Coroutine> Coroutines = new();

        protected override void OnDestroy()
        {
            base.OnDestroy();
            for (var i = Coroutines.Count-1; i >= 0; i--)
            {
                StopCoroutine(Coroutines[i]);
                Coroutines.Remove(Coroutines[i]);
            }
            Coroutines.Clear();
        }
        
        public static Coroutine Begin(IEnumerator coroutine)
        {
            var routine = Instance.StartCoroutine(coroutine);
            Coroutines.Add(routine);
            return routine;
        }
        
        public static Coroutine WaitForSeconds(float seconds, Action callback)
        {
            return Instance.StartCoroutine(WaitAndInvoke(seconds, callback));
        }
        
        private static IEnumerator WaitAndInvoke(float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            callback?.Invoke();
        }     
        
        public static Coroutine FadeColor(Action<Color> setColor, Color startColor, Color endColor, float seconds, Action callback = null)
        {
            return Instance.StartCoroutine(FadeColorWithCallback(setColor, startColor, endColor, seconds, callback));
        }
        
        private static IEnumerator FadeColorWithCallback(Action<Color> setColor, Color startColor, Color endColor, float seconds, Action callback = null)
        {
            float time = 0f;

            while (time < seconds)
            {
                var newColor = Color.Lerp(startColor, endColor, time / seconds);
                setColor(newColor);

                time += Time.deltaTime;
                yield return null;
            }
            
            setColor(endColor);

            callback?.Invoke();
        }
        
        public static void End(Coroutine coroutine)
        {
            if (coroutine == null || Instance == null)
                return;
            
            Coroutines.Remove(coroutine);
            Instance.StopCoroutine(coroutine);
        }
        
        public static Task WaitForSecondsAsync(float seconds, Action callback = null)
        {
            var tcs = new TaskCompletionSource<bool>();
            Instance.StartCoroutine(Wait(seconds, tcs, callback));
            return tcs.Task;

            static IEnumerator Wait(float time, TaskCompletionSource<bool> tcs, Action callback)
            {
                yield return new WaitForSeconds(time);
                tcs.SetResult(true);
                callback?.Invoke();
            }
        }

        public static Task WaitForEndOfFrameAsync()
        {
            var tcs = new TaskCompletionSource<bool>();
            Instance.StartCoroutine(Wait(tcs));
            return tcs.Task;

            static IEnumerator Wait(TaskCompletionSource<bool> tcs)
            {
                yield return new WaitForEndOfFrame();
                tcs.SetResult(true);
            }
        }
    }
}