using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace Debug_Module
{
    /// <summary>
    /// Centralized logging utility with category and level filtering.
    /// Used for development-time diagnostics and runtime logging.
    /// </summary>

    public static class DebugLogger
    {
        public const LogLevel CurrentLogLevel = LogLevel.Log;
        private static ILogger _logger = Debug.unityLogger;

        public static void EnableCategory(LogCategory category) => EnabledCategories.Add(category);
        public static void DisableCategory(LogCategory category) => EnabledCategories.Remove(category);
        
        /// <summary>
        /// Replace the logger (e.g., for file logging or unit testing).
        /// </summary>
        public static void SetLogger(ILogger customLogger)
        {
            _logger = customLogger ?? Debug.unityLogger;
        }
        
        private static readonly HashSet<LogCategory> EnabledCategories = new()
        {
            LogCategory.None,
            LogCategory.Ai,
            LogCategory.Audio,
            LogCategory.Bootstrap,
            LogCategory.Data,
            LogCategory.Debug,
            LogCategory.Framework,
            LogCategory.Input,
            LogCategory.Story,
            LogCategory.Test,
            LogCategory.Tools,
            LogCategory.Ui,
            LogCategory.World,
        };

        [Conditional("DEBUG")]
        public static void Log(string message, LogCategory category = LogCategory.None, LogLevel level = LogLevel.Log)
        {
            if (!EnabledCategories.Contains(category)) return;
            if (level < CurrentLogLevel) return;

            string prefix = $"[{category}][{level}]";
            string fullMessage = $"{prefix} {message}";
            
            switch (level)
            {
                case LogLevel.Log:
                    _logger.Log(LogType.Log, fullMessage);
                    break;
                case LogLevel.Warning:
                    _logger.Log(LogType.Warning, fullMessage);
                    break;
                case LogLevel.Error:
                    _logger.Log(LogType.Error, fullMessage);
                    break;
            }
        }
    }
}