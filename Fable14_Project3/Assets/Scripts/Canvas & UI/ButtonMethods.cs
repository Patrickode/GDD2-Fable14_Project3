using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonMethods : MonoBehaviour
{
    [Tooltip("The current active menu screen. Changes when menus are swapped. Assigned automatically when " +
        "needed if null. Set in the inspector to set a \"default\" menu screen.")]
    [SerializeField] private GameObject currentMenuScreen = null;
    //[SerializeField] private AudioClip buttonClickSound = null;

    private GameObject defaultMenuScreen;
    //private SoundEffectsManager soundEffectsManager;

    private void Awake()
    {
        if (currentMenuScreen) { defaultMenuScreen = currentMenuScreen; }
        TryInitCurrentMenuScreen();

        //soundEffectsManager = FindObjectOfType<SoundEffectsManager>();

        PauseManager.PauseGame += OnPauseGame;
    }
    private void OnDestroy()
    {
        PauseManager.PauseGame -= OnPauseGame;
    }

    //--- Button Methods ---//

    /// <summary>
    /// Loads a scene by index. If <paramref name="index"/> is negative, loads the 
    /// scene (<paramref name="index"/> * -1) scenes ahead of the current scene.
    /// </summary>
    /// <param name="index"></param>
    public void LoadSceneIndex(int index) { TransitionLoader.TransitionLoad?.Invoke(index); }

    public void ReloadScene() { TransitionLoader.TransitionReload?.Invoke(); }

    public void SetPauseState(bool isActive) { PauseManager.PauseGame?.Invoke(isActive); }

    public void TogglePauseState() { PauseManager.TogglePause?.Invoke(); }

    public void QuitGame() { TransitionLoader.TransitionQuit?.Invoke(); }

    /// <summary>
    /// Swaps the currently active menu to another menu.
    /// </summary>
    /// <param name="destinationMenuScreen"></param>
    public void SwapMenu(GameObject destinationMenuScreen)
    {
        if (destinationMenuScreen)
        {
            TryInitCurrentMenuScreen();

            currentMenuScreen.SetActive(false);
            destinationMenuScreen.SetActive(true);
            currentMenuScreen = destinationMenuScreen;
        }
    }

    //--- Helper Methods ---//

    private void OnPauseGame(bool paused)
    {
        SetCurrentMenuActive(paused);
        currentMenuScreen = defaultMenuScreen;
    }

    private void SetCurrentMenuActive(bool isActive) { SetMenuActive(isActive); }
    /// <summary>
    /// Calls SetActive on <paramref name="menuTransform"/>'s GameObject, or calls SetActive on the current 
    /// active menu if <paramref name="menuTransform"/> is null.
    /// </summary>
    /// <param name="isActive">Whether to enable or disable <paramref name="menuTransform"/>'s GameObject.</param>
    /// <param name="menuTransform">The transform of the menu to set active.</param>
    public void SetMenuActive(bool isActive, RectTransform menuTransform = null)
    {
        if (!menuTransform)
        {
            TryInitCurrentMenuScreen();

            if (currentMenuScreen)
            {
                currentMenuScreen.SetActive(isActive);
            }
            else
            {
                Debug.LogWarning("ButtonMethods: SetMenuActive was not given a transform to set active " +
                    "and currentMenuScreen was not found. Supply a transform to set active, or ensure a " +
                    "currentMenuScreen can be found.");
            }
        }
        else
        {
            menuTransform.gameObject.SetActive(isActive);
        }
    }

    /// <summary>
    /// Goes through all of this object's children, and sets currentMenu equal to the first active menu.
    /// This will only happen if currentMenu is not assigned.
    /// </summary>
    /// <param name="transformToCheck">The transform to look in. Defaults to this script's transform.</param>
    /// <returns>Whether currentMenu was assigned or not.</returns>
    private bool TryInitCurrentMenuScreen(Transform transformToCheck = null)
    {
        //Only get the current menu if currentMenu is uninitialized / null.
        if (!currentMenuScreen)
        {
            //If transformToCheck is null, use this script's transform instead.
            //Otherwise, just use transformToCheck.
            transformToCheck = transformToCheck ? transformToCheck : transform;

            //For each child of this object,
            foreach (Transform child in transformToCheck)
            {
                //Check if its active. (We don't need to check its children, because they'll be inactive if this
                //is inactive.)
                if (child.gameObject.activeInHierarchy)
                {
                    //If it is, and it's a menu, we found the first active menu. Set currentMenu equal to it.
                    if (child.CompareTag("MenuScreen"))
                    {
                        currentMenuScreen = child.gameObject;
                        return true;
                    }
                    //If it's not a menu, it's a container; check all of its children, and if we find
                    //a menu when we do, return true.
                    else
                    {
                        if (TryInitCurrentMenuScreen(child)) { return true; }
                    }
                }
            }
        }

        //If we made it this far, currentMenu is already assigned, or no active menu was found.
        return false;
    }

    //// calculatorClick.wav by MAbdurrahman at freesound.org
    //// https://freesound.org/people/MAbdurrahman/sounds/425187/
    //public void PlayClickSound()
    //{
    //    soundEffectsManager.PlaySound(buttonClickSound);
    //}
}