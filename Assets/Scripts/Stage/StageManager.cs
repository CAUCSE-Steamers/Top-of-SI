using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System;
using Model;

using Random = UnityEngine.Random;

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
    private Programmer programmerTemplate;

    [SerializeField]
    private AbstractProject bossTemplate;
    private StageUiPresenter uiPresenter;

    private void Awake()
    {
        if (Instance != null)
        {
            DebugLogger.LogWarning("StageManager::Awake => 이미 초기화된 StageManager가 메모리에 존재합니다.");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    private void Start()
    {
        Programmers = new List<Programmer>();

        if (fieldSpawner == null)
        {
            DebugLogger.LogError("StageManager::Start => 필드를 생성할 Spawner가 null입니다.");
        }
    }

    public void RefreshPresenter(StageUiPresenter stageUiPresenter)
    {
        Dispose();

        uiPresenter = stageUiPresenter;
        SetStage();
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

    public ICollection<Programmer> Programmers
    {
        get; private set;
    }

    public AbstractProject Boss
    {
        get; private set;
    }

    public void SetStage()
    {
        CommonLogger.Log("StageManager::SetStage => 초기화 시작");
        CurrentStage = LobbyManager.Instance.SelectedStage;
        StageField = fieldSpawner.SpawnField();
        
        Status.InitializeStageStatus(maximumDayLimit: CurrentStage.ElapsedDayLimit, unitManager: Unit);

        InitializeProgrammers();
        InitializeBoss();

        Unit.SetUnits(Programmers, Boss, StageField);
        Status.RegisterEventAfterInit(unitManager: Unit);

        Status.OnStageDirectionChanged += AdjustStageDirectionView;
    }

    private void InitializeProgrammers()
    {
        Programmers.Clear();
        HashSet<Vector3> programmerPositions = new HashSet<Vector3>();

        foreach (var programmerSpec in CurrentStage.ProgrammerSpecs)
        {
            var newProgrammer = Instantiate(programmerTemplate);
            var randomVector = StageField.GetRandomVector();

            while (programmerPositions.Contains(randomVector))
            {
                randomVector = StageField.GetRandomVector();
            }

            newProgrammer.transform.position = randomVector;
            newProgrammer.Ability = programmerSpec.Ability;
            newProgrammer.Status = programmerSpec.Status;

            programmerPositions.Add(randomVector);
            Programmers.Add(newProgrammer);
        }
    }

    private void InitializeBoss()
    {
        var newBoss = Instantiate(bossTemplate);
        Boss = newBoss;
        Boss.Status = CurrentStage.Boss.Status;
        Boss.Ability = CurrentStage.Boss.Ability;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Status.StageDirection = Status.StageDirection == Direction.Left ?
                                    Direction.Right :
                                    Direction.Left;
        }
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
        Boss.transform.Translate(new Vector3(bossDeltaX, 0f, 0f), Space.World);
        Boss.transform.Rotate(Vector3.up * 180.0f, Space.World);
    }

    public void Dispose()
    {
        Unit.DisposeRegisteredEvents();
        Status.DisposeRegisteredEvents();
    }
}