using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class DebugLogger
{
    public static void Log(object message)
    {
#if UNITY_EDITOR
        CommonLogger.Log(message);
#endif
    }

    public static void LogFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        CommonLogger.LogFormat(format, args);
#endif
    }

    public static void LogWarning(object message)
    {
#if UNITY_EDITOR
        CommonLogger.LogWarning(message);
#endif
    }

    public static void LogWarningFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        CommonLogger.LogWarningFormat(format, args);
#endif
    }

    public static void LogError(object message)
    {
#if UNITY_EDITOR
        CommonLogger.LogError(message);
#endif
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
#if UNITY_EDITOR
        CommonLogger.LogErrorFormat(format, args);
#endif
    }
}
