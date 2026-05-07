using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Ui_Module
{
    public class DebugUi : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI fpsText;
        [SerializeField] private TextMeshProUGUI logText;
        [SerializeField] private int maxLogs = 50;

        private float deltaTime;
        private readonly Queue<string> logs = new();

        void Awake()
        {
            Application.logMessageReceived += HandleLog;
        }

        void OnDestroy()
        {
            Application.logMessageReceived -= HandleLog;
        }

        void Update()
        {
            deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsText.text = $"FPS: {Mathf.Ceil(fps)}";
        }

        private void HandleLog(string logString, string stackTrace, LogType type)
        {
            if (logs.Count >= maxLogs)
                logs.Dequeue();

            logs.Enqueue(logString);
            logText.text = string.Join("\n", logs);
        }
    }
}