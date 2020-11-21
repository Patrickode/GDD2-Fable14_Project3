using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    //private SoundEffectsManager soundEffectsManager;

    //[SerializeField] private AudioClip togglePauseSound = null;

    /// <summary>
    /// Action to set the pause state of the game.
    /// </summary>
    public static Action<bool> PauseGame;
    /// <summary>
    /// When invoked, invokes PauseGame with the opposite of the current pause state.
    /// </summary>
    public static Action TogglePause;

    private bool isPaused = false;

    private void Awake()
    {
        //soundEffectsManager = FindObjectOfType<SoundEffectsManager>();

        PauseGame += OnGamePaused;
        TogglePause += OnTogglePaused;
    }
    private void OnDestroy()
    {
        //Unpause whenever this is destroyed, so the game can't get stuck in 0 time scale.
        OnGamePaused(false);

        PauseGame -= OnGamePaused;
        TogglePause -= OnTogglePaused;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause?.Invoke();
        }
    }

    private void OnTogglePaused()
    {
        //soundEffectsManager.PlaySound(togglePauseSound);
        PauseGame?.Invoke(!isPaused);
    }
    private void OnGamePaused(bool paused)
    {
        //No need to do anything here if the game is already in the state we want it in.
        if (isPaused == paused) { return; }

        isPaused = paused;

        //If the game is supposed to be paused, set time scale to 1.
        //If not, set it to 0. (Unpause if paused, pause if unpaused)
        Time.timeScale = isPaused ? 0 : 1;
    }
}
