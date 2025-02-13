using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logger.Scripts
{
    public class Logger : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI uiLogText;
        public Toggle logToggle, warningToggle, errorToggle;
        public Button clearButton;
        public ScrollRect scrollRect;

        public LoggerData logData;
        private string _logCache = "";

        private void Start()
        {
            SetLogDataToUI();
        }

        private void OnEnable()
        {
            Application.logMessageReceived += LogCallback;
            clearButton.onClick.AddListener(ClearLog);
        }

        private void OnDisable()
        {
            Application.logMessageReceived -= LogCallback;
            clearButton.onClick.RemoveListener(ClearLog);
        }

        private void LogCallback(string message, string stackTrace, LogType type)
        {
            int logTypeIndex = type switch
            {
                LogType.Log => 0,
                LogType.Warning => 1,
                _ => 2
            };

            if ((logTypeIndex == 0 && !logToggle.isOn) ||
                (logTypeIndex == 1 && !warningToggle.isOn) ||
                (logTypeIndex == 2 && !errorToggle.isOn))
            {
                return;
            }

            string logEntry = $"[{DateTime.Now:HH:mm:ss}] <sprite={logTypeIndex}><color={logData.Colors[logTypeIndex]}> {message}</color>\n\n";
            _logCache += logEntry;
            uiLogText.text = _logCache;

            Canvas.ForceUpdateCanvases();
            scrollRect.verticalNormalizedPosition = 0;
        }

        private void ClearLog()
        {
            _logCache = "";
            uiLogText.text = "";
        }
        private void SetLogDataToUI()
        {
            logToggle.isOn = logData.logToggle;
            warningToggle.isOn = logData.warningToggle;
            errorToggle.isOn = logData.errorToggle;
            
            logToggle.onValueChanged.AddListener(value => logData.logToggle = value);
            warningToggle.onValueChanged.AddListener(value => logData.warningToggle = value);
            errorToggle.onValueChanged.AddListener(value => logData.errorToggle = value);
        }
    }
}