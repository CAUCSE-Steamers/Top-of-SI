using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using Model;

public class AlternativeProgrammerSelectPresenter : MonoBehaviour
{
    [SerializeField]
    private GameObject[] selectedProgrammerCells;
    [SerializeField]
    private ProgrammerListPresenter programmerListPresenter;
    [SerializeField]
    private ModalPresenter modalPresenter;

    private ICollection<ProgrammerSpec> selectedSpecs;
    private int blankProgrammers;

    public IEnumerable<Programmer> DeadProgrammers
    {
        get; set;
    }

    public void Present(IEnumerable<Programmer> programmers)
    {
        selectedSpecs = new List<ProgrammerSpec>();
        blankProgrammers = LobbyManager.Instance.CurrentPlayer.ProgrammerSpecs.Count() -
                           programmers.Where(programmer => programmer.IsAlive).Count();

        programmerListPresenter.OnCellButtonClicked += (sender, spec) =>
        {
            var selectedRepresentImage = sender.GetComponentsInChildren<Image>(includeInactive: true)
                                               .Last();

            var willBeSelected = !selectedRepresentImage.gameObject.activeSelf;

            if (willBeSelected && selectedSpecs.Count < blankProgrammers)
            {
                selectedRepresentImage.gameObject.SetActive(true);
                selectedSpecs.Add(spec);
            }
            else
            {
                selectedRepresentImage.gameObject.SetActive(false);
                selectedSpecs.Remove(spec);
            }

            PresentSelectedPanel(programmers);
        };

        var usedProgrammerSpecs
            = from spec in LobbyManager.Instance.CurrentPlayer.ProgrammerSpecs
              from status in StageManager.Instance.Unit.Programmers.Select(programmer => programmer.Status)
              where spec.Status.Equals(status)
              select spec;

        programmerListPresenter.Present(LobbyManager.Instance.CurrentPlayer.ProgrammerSpecs.Except(usedProgrammerSpecs));
        PresentSelectedPanel(programmers);
    }

    private void PresentSelectedPanel(IEnumerable<Programmer> programmers)
    {
        foreach (var selectedProgrammerCell in selectedProgrammerCells)
        {
            selectedProgrammerCell.SetActive(false);
        }

        for (int i = 0; i < Mathf.Min(blankProgrammers, selectedProgrammerCells.Length); ++i)
        {
            selectedProgrammerCells[i].SetActive(true);
            selectedProgrammerCells[i].GetComponentsInChildren<Image>().Last().color = new Color(1, 1, 1, 0f);
            selectedProgrammerCells[i].GetComponentsInChildren<Text>().Last().text = string.Empty;

            if (i < selectedSpecs.Count)
            {
                selectedProgrammerCells[i].GetComponentsInChildren<Image>().Last().color = new Color(1, 1, 1, 1f);
                selectedProgrammerCells[i].GetComponentsInChildren<Image>().Last().sprite
                    = ResourceLoadUtility.LoadPortrait(selectedSpecs.ElementAt(i).Status.PortraitName);

                selectedProgrammerCells[i].GetComponentsInChildren<Text>().Last().text
                    = selectedSpecs.ElementAt(i).Status.Name;
            }
        }
    }

    public void FA()
    {
        //StageManager.Instance.MakeProgrammers(selectedSpecs);

        modalPresenter.ResetClickAction();
        modalPresenter.SetActive(true);

        StageManager.Instance.StageUi.RenderPaymentText(DeadProgrammers);

        this.gameObject.SetActive(false);
    }
}
