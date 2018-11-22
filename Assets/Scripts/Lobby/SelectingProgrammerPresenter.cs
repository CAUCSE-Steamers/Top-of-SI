using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public class SelectingProgrammerPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] selectedProgrammerCells;
    [SerializeField]
    private ProgrammerListPresenter programmerListPresenter;

    private ICollection<ProgrammerSpec> selectedSpecs;

    private void Start()
    {
        selectedSpecs = new List<ProgrammerSpec>();

        foreach (var selectedProgrammerCell in selectedProgrammerCells)
        {
            selectedProgrammerCell.SetActive(false);
        }

        programmerListPresenter.OnCellButtonClicked += (sender, spec) =>
        {
            var selectedRepresentImage = sender.GetComponentsInChildren<Image>(includeInactive: true)
                                               .Last();

            var willBeSelected = !selectedRepresentImage.gameObject.activeSelf;
            selectedRepresentImage.gameObject.SetActive(willBeSelected);

            if (willBeSelected)
            {
                selectedSpecs.Add(spec);
            }
            else
            {
                selectedSpecs.Remove(spec);
            }

            PresentSelectedPanel();
        };
    }
    
    private void PresentSelectedPanel()
    {
        for (int i = 0; i < selectedProgrammerCells.Length; ++i)
        {
            if (i < selectedSpecs.Count)
            {
                selectedProgrammerCells[i].SetActive(true);

                selectedProgrammerCells[i].GetComponentsInChildren<Image>().Last().sprite 
                    = ResourceLoadUtility.LoadPortrait(selectedSpecs.ElementAt(i).Status.PortraitName);

                selectedProgrammerCells[i].GetComponentsInChildren<Text>().Last().text
                    = selectedSpecs.ElementAt(i).Status.Name;
            }
            else
            {
                selectedProgrammerCells[i].SetActive(false);
            }
        }
    }

    public void StoreSelectedProgrammers()
    {
        LobbyManager.Instance.SelectedStage.ProgrammerSpecs = selectedSpecs;
    }
}
