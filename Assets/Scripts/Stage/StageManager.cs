using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;

public class StageManager : MonoBehaviour, IDisposable
{
    public static StageManager Instance = null;

    [SerializeField]
    private StageStatusManager statusManager;
    [SerializeField]
    private UnitManager unitManager;

    private void Awake()
    {
        if (Instance != null)
        {
            DebugLogger.LogWarning("StageManager::Awake => 이미 초기화된 StageManager가 메모리에 존재합니다.");
            Destroy(this.gameObject);
        }

        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public StageStatusManager Status
    {
        get
        {
            if (statusManager == null)
            {
                DebugLogger.LogError("StageManager::Status => StatusManager가 Null입니다!");
            }

            return statusManager;
        }
    }

    public UnitManager Unit
    {
        get
        {
            if (unitManager == null)
            {
                DebugLogger.LogError("UnitManager::Unit => UnitManager가 Null입니다!");
            }

            return unitManager;
        }
    }

    [SerializeField]
    private Programmer[] programmers;
    [SerializeField]
    private Programmer boss; // TODO: Changed to boss
    private void Start()
    {
        SetStage(programmers, boss);
    }

    public void SetStage(IEnumerable<Programmer> programmers, Programmer boss)
    {
        // TODO : Remove hard-coding
        CommonLogger.Log("StageManager::SetStage => 초기화 시작");

        Status.InitializeStageStatus(maximumDayLimit: 10, unitManager: Unit);
        Unit.SetUnits(programmers, boss);
    }

    public void Dispose()
    {
        Unit.DisposeRegisteredEvents();
        Status.DisposeRegisteredEvents();
    }
}