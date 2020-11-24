using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    private AudioSource audioSource;

    public GameObject musicManagerPrefab;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        audioSource.mute = PlayerPrefs.GetInt("MuteMusic", 0) == 1;
        audioSource.volume = PlayerPrefs.GetFloat("MusicVolume", 0.5f);
    }

    public void ToggleMute() { SetMute(!audioSource.mute); }

    public void SetMute(bool mute)
    {
        audioSource.mute = mute;

        //If muted, store 1. Otherwise, store 0. (There is no SetBool in PlayerPrefs.)
        PlayerPrefs.SetInt("MuteMusic", audioSource.mute ? 1 : 0);
    }

    public void SetVolume(float volume)
    {
        audioSource.volume = volume;

        PlayerPrefs.SetFloat("MusicVolume", volume);
    }
}
