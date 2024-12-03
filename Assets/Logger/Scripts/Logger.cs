using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Logger.Scripts{
    public class Logger : MonoBehaviour
    {
        public TextMeshProUGUI uiLogText;

        private readonly string[] _colors = new string[3]{
            "#aaaaaa", // White
            "#ffdd33", // Yellow
            "#ff6666"  // Red
        };

        private void OnEnable() {
            Application.logMessageReceived += LogCallback;
        }

        // built in command 
        private void LogCallback(string message, string stackTrace, LogType type)
        {
            var logTypeIndex = type switch
            {
                LogType.Log => 0,
                LogType.Warning => 1,
                _ => 2
            };

            uiLogText.text += $"<sprite={logTypeIndex}><color={_colors[logTypeIndex]}> {message}</color>\n\n";
        }
        private void OnDisable() {
            Application.logMessageReceived += LogCallback;
        }
    }
}
