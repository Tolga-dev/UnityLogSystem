using System;
using Logger.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class Demo : MonoBehaviour
{
    public Button createLog;
    public void Start()
    {
        createLog.onClick.AddListener(() =>
        {
            LogError();
            LogInfo();
            LogWarning();
        });
    }

    private void LogInfo()
    {
        Debug.Log("This is a normal log message");
    }

    private void LogWarning()
    {
        Debug.LogWarning("Warning message");
    }

    private void LogError()
    {
        Debug.LogError("Error: Just kidding :)");
    }
}

