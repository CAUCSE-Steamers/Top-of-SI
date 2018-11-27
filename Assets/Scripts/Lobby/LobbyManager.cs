using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Model;

public class LobbyManager : MonoBehaviour, IEventDisposable
{
    public Player CurrentPlayer
    {
        get; private set;
    }

    public static LobbyManager Instance
    {
        get; private set;
    }

    private GameStage selectedStage;

    public GameStage SelectedStage
    {
        get
        {
            return selectedStage;
        }
        set
        {
            if (value != null)
            {
                selectedStage = value;
                OnChangeStage(value, isFirstProject(value), isLastProject(value));
            }
        }
    }

    private List<GameStage> currentAvailableStages;
    private List<GameStage> allStages;

    private IEnumerable<GameStage> MainStages
    {
        get
        {
            return allStages.Where(stage => stage.MainStage == true);
        }
    }

    private IEnumerable<GameStage> SubStages
    {
        get
        {
            return allStages.Where(stage => stage.MainStage == false);
        }
    }

    private LobbyUiPresenter lobbyUi;

    public event Action<GameStage, bool, bool> OnChangeStage = delegate { };

    private void Awake()
    {
        if (Instance != null)
        {
            DebugLogger.LogWarning("LobbyManager::Awake => 이미 초기화된 LobbyManager가 메모리에 존재합니다.");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void TempLobby()
    {
        allStages = new List<GameStage>{
            new GameStage
            {
                Title = "Test Project",
                ElapsedDayLimit = 5,
                Boss = new ProjectSpec()
                {
                    Ability = new TestProject().Ability,
                    Status = new TestProject().Status
                },
                Reward = 100,
                IconName = "Cpp"
            },
            new GameStage
            {
                Title = "Web Project",
                ElapsedDayLimit = 4,
                Boss = new ProjectSpec()
                {
                    Ability = new TestProject().Ability,
                    Status = new TestProject().Status
                },
                Reward = 200,
                IconName = "CSharp",
                MainStage = true
            },
            new GameStage
            {
                Title = "iOS Project",
                ElapsedDayLimit = 3,
                Boss = new ProjectSpec()
                {
                    Ability = new TestProject().Ability,
                    Status = new TestProject().Status
                },
                Reward = 300,
                IconName = "Java"
            }
        };

        allStages[0].AddObjectives(new List<IStageObjective>
            {
                new ElapsedDayObjective(allStages[0]),
                new StringObjective("테스트 목표 1"),
                new StringObjective("테스트 목표 2"),
                new StringObjective("테스트 목표 3"),
            });

        allStages[1].AddObjectives(new List<IStageObjective>
        {
            new ElapsedDayObjective(allStages[1]),
            new StringObjective("테스트 목표"),
        });
        allStages[2].AddObjectives(new List<IStageObjective>
        {
            new ElapsedDayObjective(allStages[2]),
            new StringObjective("테스트 목표"),
        });
    }

    public void RefreshPresenter(LobbyUiPresenter presenter)
    {
        DisposeRegisteredEvents();
        
        lobbyUi = presenter;

        if (CurrentPlayer.Money < 0)
        {
            CommonLogger.Log("++++++++++++++++++++++++ GAME OVER ++++++++++++++++++++++++++++");
        }

        lobbyUi.UpdateMoney(CurrentPlayer.Money);
        OnChangeStage += lobbyUi.UpdateProject;

        currentAvailableStages = new List<GameStage>();
        
        if (CurrentPlayer.MainStageLevel < MainStages.Count())
        {
            currentAvailableStages.Add(MainStages.ElementAt(CurrentPlayer.MainStageLevel));
        }

        currentAvailableStages.AddRange(
            SubStages.Where(stage => CurrentPlayer.ClearedStageNames.Contains(stage.Title) == false)
        );

        if (currentAvailableStages.Count == 0)
        {
            CommonLogger.Log("++++++++++++++++++++++++ GAME CLEAR ++++++++++++++++++++++++++++");
        }

        SelectedStage = currentAvailableStages.FirstOrDefault();
    }

    private void Start()
    {
        CurrentPlayer = new Player();
        currentAvailableStages = new List<GameStage>();
        allStages = new List<GameStage>();

        var lobbyUiObject = GameObject.Find("LobbyUi");
        if (lobbyUiObject != null)
        {
            RefreshPresenter(GameObject.Find("LobbyUi").GetComponent<LobbyUiPresenter>());
        }
    }

    public void LoadPlayer()
    {
        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Player.xml";

        if (File.Exists(savePath))
        {
            var rootElement = XElement.Parse(File.ReadAllText(savePath));
            CurrentPlayer.RecoverStateFromXml(rootElement.ToString());
        }
    }

    public void SavePlayer()
    {
        var savePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Player.xml";

        File.WriteAllText(savePath, CurrentPlayer.ToXmlElement().ToString());
    }

    public void LoadStages()
    {
        var projectContent = ResourceLoadUtility.LoadData("Stage").text;

        var rootElement = XElement.Parse(projectContent);

        foreach (var stageElement in rootElement.Elements("Stage"))
        {
            var newStage = new GameStage();
            newStage.RecoverStateFromXml(stageElement.ToString());
            allStages.Add(newStage);
        }
    }

    public void SaveStages()
    {
        var projectPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/Stage.xml";
        var stageRootElement = new XElement("Stages",
            allStages.Select(stage => stage.ToXmlElement()));

        File.WriteAllText(projectPath, stageRootElement.ToString());
    }

    public void ChangeToNextProject()
    {
        var index = currentAvailableStages.IndexOf(selectedStage);

        if (index == (currentAvailableStages.Count - 1))
        {
            return;
        }
        else
        {
            SelectedStage = currentAvailableStages[index + 1];
        }
    }

    public void ChangeToPreviousProject()
    {
        var index = currentAvailableStages.IndexOf(selectedStage);

        if (index == 0)
        {
            return;
        }
        else
        {
            SelectedStage = currentAvailableStages[index - 1];
        }
    }

    bool isFirstProject(GameStage stage)
    {
        if (currentAvailableStages.IndexOf(stage) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool isLastProject(GameStage stage)
    {
        if (currentAvailableStages.IndexOf(stage) == (currentAvailableStages.Count - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void DisposeRegisteredEvents()
    {
        OnChangeStage = delegate { };
    }
}
