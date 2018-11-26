using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using UnityEngine.UI;
using UnityEngine;

public class ObjectiveUiTemplate : MonoBehaviour
{
    [SerializeField]
    private Text descriptionText;

    public void Render(IStageObjective objective)
    {
        descriptionText.text = objective.Description;
    }

    public void Render(string naiveText)
    {
        descriptionText.text = naiveText;
    }
}