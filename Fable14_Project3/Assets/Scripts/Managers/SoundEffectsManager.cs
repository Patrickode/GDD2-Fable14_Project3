using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class SoundEffectsManager : MonoBehaviour
{
    private AudioSource audioSource;
    public GameObject soundEffectsManagerPrefab;

    private bool playingLoop = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.mute = PlayerPrefs.GetInt("MuteSFX", 0) == 1;
        audioSource.volume = PlayerPrefs.GetFloat("SFXVolume", 0.5f);
    }

    public void PlaySound(AudioClip sound)
    {
        if (audioSource)
            audioSource.PlayOneShot(sound);
    }

    public void StartLoop(AudioClip sound)
    {
        if (audioSource && !playingLoop)
        {
            playingLoop = true;

            audioSource.clip = sound;
            audioSource.loop = true;
            audioSource.Play();
        }
    }    

    public void StopLoop()
    {
        if (audioSource)
        {
            playingLoop = false;

            audioSource.loop = false;
            audioSource.clip = null;
        }
    }

    public void ToggleMute() { SetMute(!audioSource.mute); }

    public void SetMute(bool mute)
    {
        audioSource.mute = mute;

        //If muted, store 1. Otherwise, store 0. (There is no SetBool in PlayerPrefs.)
        PlayerPrefs.SetInt("MuteSFX", mute ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}
