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
        float volumeValue = 0.0f;

        audioMixer.GetFloat(volumeParameterName, out volumeValue);
        slider.value = volumeValue;

        slider.onValueChanged.AddListener(value =>
        {
            audioMixer.SetFloat(volumeParameterName, value);
        });
    }
}
