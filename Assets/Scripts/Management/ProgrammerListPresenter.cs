using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.Collections.Generic;
using Model;
using System;

public class ProgrammerListPresenter : MonoBehaviour
{
    public event Action<GameObject, ProgrammerSpec> OnCellButtonClicked = delegate { };

    [SerializeField]
    private GameObject programmerCellTemplate;
    [SerializeField]
    private Transform programmerPanelObject;
    
    public void Present(IEnumerable<ProgrammerStatus> fixedStatus = null)
    {
        RemoveExistingCells();
        ConstructFixedCells(fixedStatus);
        ConstructCells();
    }

    private void ConstructFixedCells(IEnumerable<ProgrammerStatus> fixedStatus)
    {
        if (fixedStatus != null)
        {
            foreach (var programmerStatus in fixedStatus)
            {
                var createdCell = Instantiate(programmerCellTemplate, programmerPanelObject);
                SetCellUi(createdCell, new ProgrammerSpec() { Status = programmerStatus });
                SetCellBlocked(createdCell);
            }
        }
    }

    private void SetCellBlocked(GameObject cellObject)
    {
        var cellButton = cellObject.GetComponentInChildren<Button>();
        cellButton.enabled = false;
    }

    private void RemoveExistingCells()
    {
        foreach (var existingCell in programmerPanelObject.GetComponentsInChildren<Transform>())
        {
            if (existingCell != programmerPanelObject)
            {
                Destroy(existingCell.gameObject);
            }
        }
    }

    private void ConstructCells()
    {
        var currentPlayer = LobbyManager.Instance.CurrentPlayer;
        foreach (var programmerSpec in currentPlayer.ProgrammerSpecs)
        {
            var createdCell = Instantiate(programmerCellTemplate, programmerPanelObject);
            SetCellUi(createdCell, programmerSpec);
            SetCellEvent(createdCell, programmerSpec);
        }
    }
    
    private void SetCellUi(GameObject cellObject, ProgrammerSpec spec)
    {
        var cellImage = cellObject.GetComponentInChildren<Image>();
        cellImage.sprite = ResourceLoadUtility.LoadPortrait(spec.Status.PortraitName);
        cellImage.enabled = true;
    }

    private void SetCellEvent(GameObject cellObject, ProgrammerSpec programmerSpec)
    {
        var cellButton = cellObject.GetComponentInChildren<Button>();
        cellButton.onClick.AddListener(() => OnCellButtonClicked(cellObject, programmerSpec));
        cellButton.interactable = true;
        cellButton.enabled = true;
    }
}
