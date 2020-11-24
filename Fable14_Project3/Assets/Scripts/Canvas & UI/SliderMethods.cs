using UnityEngine;
using UnityEngine.UI;

public class SliderMethods : MonoBehaviour
{
    [SerializeField]
    private MusicManager musicManager;
    [SerializeField]
    private SoundEffectsManager soundEffectsManager;

    private AudioSource musicManagerPrefabAudioSource;
    private AudioSource soundEffectsManagerPrefabAudioSource;

    private void Awake()
    {
        if (!musicManager)
            musicManager = FindObjectOfType<MusicManager>();
        if (!soundEffectsManager)
            soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
    }

    private void Start()
    {
        if (musicManager)
            musicManagerPrefabAudioSource = musicManager.musicManagerPrefab.GetComponent<AudioSource>();
        if (soundEffectsManager)
            soundEffectsManagerPrefabAudioSource = soundEffectsManager.soundEffectsManagerPrefab.GetComponent<AudioSource>();
    }

    public void SetMusicVolume(Slider slider)
    {
        musicManager.SetVolume(slider.value);
        musicManagerPrefabAudioSource.volume = slider.value;
    }

    public void SetSoundEffectVolume(Slider slider)
    {
        soundEffectsManager.SetVolume(slider.value);
        soundEffectsManagerPrefabAudioSource.volume = slider.value;
    }
}
