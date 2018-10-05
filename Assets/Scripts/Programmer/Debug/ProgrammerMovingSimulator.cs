using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ProgrammerMovingSimulator : MonoBehaviour
{
    [SerializeField]
    private InputField xField;
    [SerializeField]
    private InputField yField;
    [SerializeField]
    private InputField zField;
    [SerializeField]
    private Button simulationButton;
    [SerializeField]
    private Programmer programmer;

    private void Start()
    {
        simulationButton.onClick.AddListener(() =>
        {
            float x = float.Parse(xField.text);
            float y = float.Parse(yField.text);
            float z = float.Parse(zField.text);

            programmer.Move(new Vector3(x, y, z));
        });
    }
}
