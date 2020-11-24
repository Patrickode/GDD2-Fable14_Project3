using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetInitialOptionValue : MonoBehaviour
{
    [SerializeField] private bool isMusic = false;

    private AudioSource musicPrefabAudioSource;
    private AudioSource sfxPrefabAudioSource;

    private void Start()
    {
        if (isMusic)
        {
            musicPrefabAudioSource = FindObjectOfType<MusicManager>(
                ).musicManagerPrefab.GetComponent<AudioSource>();
        }
        else
        {
            sfxPrefabAudioSource = FindObjectOfType<SoundEffectsManager>(
                ).soundEffectsManagerPrefab.GetComponent<AudioSource>();
        }

        if (TryGetComponent(out Toggle checkbox))
        {
            checkbox.isOn = isMusic ? musicPrefabAudioSource.mute : sfxPrefabAudioSource.mute;
        }
        else if (TryGetComponent(out Slider volumeSlider))
        {
            volumeSlider.value = isMusic ? musicPrefabAudioSource.volume : sfxPrefabAudioSource.volume;
        }
    }
}
