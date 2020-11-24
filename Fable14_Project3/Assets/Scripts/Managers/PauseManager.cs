using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseManager : MonoBehaviour
{
    private SoundEffectsManager soundEffectsManager;

    [SerializeField] private AudioClip togglePauseSound = null;

    /// <summary>
    /// Action to set the pause state of the game.
    /// </summary>
    public static Action<bool> PauseGame;
    /// <summary>
    /// When invoked, invokes PauseGame with the opposite of the current pause state.
    /// </summary>
    public static Action TogglePause;

    private bool isPaused = false;
    private bool pauseAllowed = true;

    private void Awake()
    {
        soundEffectsManager = FindObjectOfType<SoundEffectsManager>();

        PauseGame += OnGamePaused;
        TogglePause += OnTogglePaused;
        CustomerManager.LastCustomerDequeued += OnLastCustomerDequeued;
    }
    private void OnDestroy()
    {
        //Unpause whenever this is destroyed, so the game can't get stuck in 0 time scale.
        SetPauseAllowance(true);
        OnGamePaused(false);

        PauseGame -= OnGamePaused;
        TogglePause -= OnTogglePaused;
        CustomerManager.LastCustomerDequeued -= OnLastCustomerDequeued;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause?.Invoke();
        }
    }

    private void OnLastCustomerDequeued()
    {
        IEnumerator DelaySet()
        {
            yield return new WaitForSeconds(1.5f);
            PauseGame?.Invoke(false);
            OnGamePaused(true);
            pauseAllowed = false;
        }
        StartCoroutine(DelaySet());
    }
    private void SetPauseAllowance(bool canPause)
    {
        if (!canPause) { PauseGame?.Invoke(false); }
        pauseAllowed = canPause;
    }

    private void OnTogglePaused()
    {
        if (pauseAllowed)
        {
            soundEffectsManager.PlaySound(togglePauseSound);
            PauseGame?.Invoke(!isPaused);
        }
    }
    private void OnGamePaused(bool paused)
    {
        //No need to do anything here if the game is already in the state we want it in, or pausing isn't allowed.
        if (!pauseAllowed || isPaused == paused) { return; }

        isPaused = paused;

        //If the game is supposed to be paused, set time scale to 1.
        //If not, set it to 0. (Unpause if paused, pause if unpaused)
        Time.timeScale = isPaused ? 0 : 1;
    }
}
