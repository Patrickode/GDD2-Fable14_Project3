using UnityEngine;
using UnityEngine.UI;

public class CheckboxMethods : MonoBehaviour
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

    public void MuteMusic(Toggle checkbox)
    {
        musicManager.SetMute(checkbox.isOn);
        musicManagerPrefabAudioSource.mute = checkbox.isOn;
    }

    public void MuteSoundEffects(Toggle checkbox)
    {
        soundEffectsManager.SetMute(checkbox.isOn);
        soundEffectsManagerPrefabAudioSource.mute = checkbox.isOn;
    }
}
