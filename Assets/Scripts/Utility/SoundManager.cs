using UnityEngine;
using UnityEngine.Audio;
using System.Collections.Generic;
using System.Linq;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance
    {
        get; private set;
    }

    [SerializeField]
    private int createdPoolSize;
    [SerializeField]
    private AudioMixerGroup mixerGroup;

    private IList<AudioSource> sourcePool;

    private void Awake()
    {
        if (Instance != null)
        {
            DebugLogger.LogWarning("SoundManager::Awake => 이미 초기화된 SoundManager가 메모리에 존재합니다.");
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void Start()
    {
        sourcePool = new List<AudioSource>();
        
        for (int i = 0; i < createdPoolSize; ++i)
        {
            sourcePool.Add(CreateNewSource());
        }
    }

    private AudioSource CreateNewSource()
    {
        var newSource = new GameObject();
        var audioComponent = newSource.AddComponent<AudioSource>();
        audioComponent.outputAudioMixerGroup = mixerGroup;
        audioComponent.spatialBlend = 0f;

        newSource.transform.parent = this.gameObject.transform;

        return audioComponent;
    }

    public AudioSource FetchAvailableSource()
    {
        var availableSources = sourcePool.Where(source => source.isPlaying == false);
        return availableSources.FirstOrDefault();
    }
}
