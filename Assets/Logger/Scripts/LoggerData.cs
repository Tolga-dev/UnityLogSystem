using UnityEngine;
using UnityEngine.Serialization;

namespace Logger.Scripts
{
    [CreateAssetMenu(fileName = "LoggerData", menuName = "So/Logger/LoggerData", order = 0)]
    public class LoggerData : ScriptableObject
    {
        [Header("UI Set")]
        public bool logToggle, warningToggle, errorToggle;
        
        [Header("Log Set")]
        public readonly string[] Colors = { "#aaaaaa", "#ffdd33", "#ff6666" };
    }
}