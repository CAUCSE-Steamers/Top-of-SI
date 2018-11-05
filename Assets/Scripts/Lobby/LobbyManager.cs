using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Model;

public class LobbyManager : MonoBehaviour
{
    Player player;

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

    List<GameStage> stages;

    [SerializeField]
    private LobbyUiPresenter lobbyUi;

    public event Action<GameStage, bool, bool> OnChangeStage = delegate { };

    private void TempLobby()
    {
        stages = new List<GameStage>{
            new GameStage
            {
                Title = "Test Project",
                ElapsedDayLimit = 5,
            },
            new GameStage
            {
                Title = "Web Project",
                ElapsedDayLimit = 4,
            },
            new GameStage
            {
                Title = "iOS Project",
                ElapsedDayLimit = 3,
            }
        };


        stages[0].AddObjectives(new List<IStageObjective>
            {
                new ElapsedDayObjective(stages[0]),
                new StringObjective("테스트 목표 1"),
                new StringObjective("테스트 목표 2"),
                new StringObjective("테스트 목표 3"),
            });

        stages[1].AddObjectives(new List<IStageObjective>
        {
            new ElapsedDayObjective(stages[1]),
            new StringObjective("테스트 목표"),
        });
        stages[2].AddObjectives(new List<IStageObjective>
        {
            new ElapsedDayObjective(stages[2]),
            new StringObjective("테스트 목표"),
        });

        selectedStage = stages[0];
    }

    // Use this for initialization
    void Start()
    {
        TempLobby();
        OnChangeStage += lobbyUi.updateProject;
        SelectedStage = stages[0];
    }

    bool isFirstProject(GameStage stage)
    {
        if (stages.IndexOf(stage) == 0)
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
        if (stages.IndexOf(stage) == (stages.Count - 1))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
