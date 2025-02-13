using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logger.Scripts
{
    public class Logger : MonoBehaviour
    {
        [Header("UI Components")]
        public TextMeshProUGUI uiLogText;
        public Toggle logToggle, warningToggle, errorToggle;
        public Button clearButton;
        public ScrollRect scrollRect;

        [Header("Logger Data")]
        public LoggerData logData;

        private readonly ConcurrentQueue<string> _logQueue = new();
        private string _logCache = "";
        private bool _isProcessingLogs;

        private void Awake()
        {
            LoadPreferences();
        }

        private void Start()
        {
            ApplyLoggerSettings();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += QueueLogMessage;
            clearButton.onClick.AddListener(ClearLog);
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= QueueLogMessage;
            clearButton.onClick.RemoveListener(ClearLog);
        }

        private void QueueLogMessage(string message, string stackTrace, LogType type)
        {
            _logQueue.Enqueue(FormatLogMessage(message, type));
            if (!_isProcessingLogs)
                ProcessLogQueue();
        }

        private async void ProcessLogQueue()
        {
            try
            {
                _isProcessingLogs = true;

                while (_logQueue.TryDequeue(out var logEntry))
                {
                    _logCache += logEntry;
                    await Task.Yield();
                }

                UpdateLogUI();
                _isProcessingLogs = false;
            }
            catch (Exception e)
            {
                Debug.LogError($"Error processing log queue: {e}");
            }
        }

        private void UpdateLogUI()
        {
            if (uiLogText == null) return;
            
            uiLogText.text = _logCache;
            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }

        private string FormatLogMessage(string message, LogType type)
        {
            int logTypeIndex = type switch
            {
                LogType.Log => 0,
                LogType.Warning => 1,
                _ => 2
            };

            if (!IsLogTypeEnabled(logTypeIndex)) return string.Empty;
            return $"[{DateTime.Now:HH:mm:ss}] <sprite={logTypeIndex}><color={logData.Colors[logTypeIndex]}> {message}</color>\n\n";
        }

        private bool IsLogTypeEnabled(int logTypeIndex)
        {
            return logTypeIndex switch
            {
                0 => logToggle.isOn,
                1 => warningToggle.isOn,
                2 => errorToggle.isOn,
                _ => false
            };
        }

        private void ClearLog()
        {
            _logCache = "";
            uiLogText.text = "";
        }

        private void ApplyLoggerSettings()
        {
            logToggle.isOn = logData.logToggle;
            warningToggle.isOn = logData.warningToggle;
            errorToggle.isOn = logData.errorToggle;

            logToggle.onValueChanged.AddListener(value => UpdateLogSetting("logToggle", value));
            warningToggle.onValueChanged.AddListener(value => UpdateLogSetting("warningToggle", value));
            errorToggle.onValueChanged.AddListener(value => UpdateLogSetting("errorToggle", value));
        }

        private void UpdateLogSetting(string key, bool value)
        {
            PlayerPrefs.SetInt(key, value ? 1 : 0);
            PlayerPrefs.Save();
        }
        private void LoadPreferences()
        {
            logData.logToggle = PlayerPrefs.GetInt("logToggle", 1) == 1;
            logData.warningToggle = PlayerPrefs.GetInt("warningToggle", 1) == 1;
            logData.errorToggle = PlayerPrefs.GetInt("errorToggle", 1) == 1;
        }
    }
}
