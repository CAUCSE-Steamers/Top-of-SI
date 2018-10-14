using UnityEngine;
using System.Collections;

public enum StateParameter
{
    Idle,
    SelectMove,
    Pause
}

public static class StateParameterConverter
{
    public static string ParameterToName(this StateParameter parameter)
    {
        switch (parameter)
        {
            case StateParameter.Idle:
                return "Idle";
            case StateParameter.SelectMove:
                return "SelectMove";
            case StateParameter.Pause:
                return "Pause";
            default:
                DebugLogger.LogWarningFormat("StateParameterConverter::ParameterToName => 알 수 없는 매개변수 {0}이 주어졌습니다.", parameter);
                return string.Empty;
        }
    }
}