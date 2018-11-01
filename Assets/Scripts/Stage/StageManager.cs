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
    private UnitManager unitManager;
    [SerializeField]
    private FieldSpawner fieldSpawner;
    [SerializeField]
    private StageUiPresenter uiPresenter;

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

    public GameStage CurrentStage
    {
        get; set;
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
    private AbstractProject boss;
    private void TempSetStage()
    {
        var tempStage = new GameStage
        {
            Title = "Project Test - Title",
            ElapsedDayLimit = 1000,
        };

        tempStage.AddObjectives(new List<IStageObjective>
            {
                new ElapsedDayObjective(tempStage),
                new StringObjective("테스트 목표 1"),
                new StringObjective("테스트 목표 2"),
                new StringObjective("테스트 목표 3"),
            });

        SetStage(tempStage, programmers, boss);
    }

    public void SetStage(GameStage stage, IEnumerable<Programmer> programmers, AbstractProject boss)
    {
        CommonLogger.Log("StageManager::SetStage => 초기화 시작");
        CurrentStage = stage;
        StageField = fieldSpawner.SpawnField();
        
        Status.InitializeStageStatus(maximumDayLimit: CurrentStage.ElapsedDayLimit, unitManager: Unit);
        Unit.SetUnits(programmers, boss, StageField);
        Status.RegisterEventAfterInit(unitManager: Unit);

        Status.OnStageDirectionChanged += AdjustStageDirectionView;
    }

    private void AdjustStageDirectionView(Direction newStageDirection)
    {
        AdjustCameraView(newStageDirection);
        AdjustUnitView(newStageDirection);
    }

    private void AdjustCameraView(Direction stageDirection)
    {
        var sceneCamera = GameObject.Find("Main Camera");

        float cameraDeltaX = stageDirection == Direction.Left ? 8.0f : -8.0f;
        sceneCamera.transform.Translate(new Vector3(cameraDeltaX, 0f, 0f), Space.World);
    }

    private void AdjustUnitView(Direction stageDirection)
    {
        foreach (var programmer in Unit.Programmers)
        {
            programmer.transform.Rotate(Vector3.up * 180.0f, Space.World);
        }

        float bossDeltaX = stageDirection == Direction.Left ? 25.0f : -25.0f;
        boss.transform.Translate(new Vector3(bossDeltaX, 0f, 0f), Space.World);
        boss.transform.Rotate(Vector3.up * 180.0f, Space.World);
    }

    public void Dispose()
    {
        Unit.DisposeRegisteredEvents();
        Status.DisposeRegisteredEvents();
    }
}