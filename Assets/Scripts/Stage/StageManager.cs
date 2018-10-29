using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;

public class StageManager : MonoBehaviour, IDisposable
{
    public static StageManager Instance
    {
        get; private set;
    }

    [SerializeField]
    private StageStatusManager statusManager;
    [SerializeField]
    private StageUiPresenter uiPresenter;
    [SerializeField]
    private UnitManager unitManager;
    [SerializeField]
    private FieldSpawner fieldSpawner;

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
    
    private void Start()
    {
        if (fieldSpawner == null)
        {
            DebugLogger.LogError("StageManager::Start => 필드를 생성할 Spawner가 null입니다.");
        }

        TempSetStage();
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

    public StageUiPresenter StageUi
    {
        get
        {
            if (uiPresenter == null)
            {
                DebugLogger.LogError("StageUiPresenter::Ui => StageUiPresenter가 Null입니다!");
            }

            return uiPresenter;
        }
    }

    public Field StageField
    {
        get; private set;
    }

    [SerializeField]
    private Programmer[] programmers;
    [SerializeField]
    private Programmer boss; // TODO: Changed to boss
    private void TempSetStage()
    {
        SetStage(programmers, boss);
    }

    public void SetStage(IEnumerable<Programmer> programmers, Programmer boss)
    {
        // TODO : Remove hard-coding
        CommonLogger.Log("StageManager::SetStage => 초기화 시작");
        StageField = fieldSpawner.SpawnField();

        Status.InitializeStageStatus(maximumDayLimit: 10, unitManager: Unit);
        Unit.SetUnits(programmers, boss, StageField);
    }
    
    public void Dispose()
    {
        Unit.DisposeRegisteredEvents();
        Status.DisposeRegisteredEvents();
    }
}