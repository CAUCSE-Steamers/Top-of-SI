﻿using UnityEngine;
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
    
    public void Present(IEnumerable<ProgrammerSpec> specs)
    {
        RemoveExistingCells();
        ConstructCells(specs);
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

    private void ConstructCells(IEnumerable<ProgrammerSpec> specs)
    {
        foreach (var programmerSpec in specs)
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
