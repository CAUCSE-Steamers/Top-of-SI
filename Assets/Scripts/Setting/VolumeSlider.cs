using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using System.Collections;

public class VolumeSlider : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private AudioMixer audioMixer;
    [SerializeField]
    private string volumeParameterName;

    private void Start()
    {
        slider.onValueChanged.AddListener(value =>
        {
            audioMixer.SetFloat(volumeParameterName, value);
        });
    }
}
