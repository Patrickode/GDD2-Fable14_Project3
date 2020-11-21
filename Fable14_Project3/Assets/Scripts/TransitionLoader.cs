using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionLoader : MonoBehaviour
{
    [SerializeField] private Animator transitionAnimator = null;
    [SerializeField] private AnimationClip outClip = null;
    [SerializeField] private float clipDurationOffset = -0.01f;
    [Space(10)]
    //[SerializeField] private SoundEffectsManager sfxManager = null;
    [SerializeField] private AudioClip transitionSound = null;

    public static float OutLength { get; private set; } = 0;

    public static Action<int> TransitionLoad;
    public static Action TransitionReload;
    public static Action TransitionInOut;

    private void Start()
    {
        OutLength = outClip.length;
        //if (!sfxManager) { sfxManager = FindObjectOfType<SoundEffectsManager>(); }

        TransitionLoad += OnTransitionLoad;
        TransitionInOut += OnTransitionInOut;

        StartCoroutine(DelaySetTrigger("In", 1));
    }
    private void OnDestroy()
    {
        TransitionLoad -= OnTransitionLoad;
        TransitionInOut -= OnTransitionInOut;
    }

    private void OnTransitionLoad(int index)
    {
        if (index < 0) { index = -index + SceneManager.GetActiveScene().buildIndex; }
        StartCoroutine(PerformTransitionLoad(index));
    }
    private void OnTransitionInOut()
    {
        StartCoroutine(PerformTransitionInOut());
    }
    private void OnTransitionReload()
    {
        StartCoroutine(PerformTransitionLoad(SceneManager.GetActiveScene().buildIndex));
    }

    private IEnumerator PerformTransitionLoad(int index)
    {
        //sfxManager.PlaySound(transitionSound);

        transitionAnimator.SetTrigger("Out");

        float waitTime = Mathf.Max(outClip.length, transitionSound.length);
        yield return new WaitForSecondsRealtime(waitTime + clipDurationOffset);

        SceneManager.LoadScene(index);
    }
    private IEnumerator PerformTransitionInOut()
    {
        //sfxManager.PlaySound(transitionSound);

        transitionAnimator.SetTrigger("Out");
        yield return new WaitForSecondsRealtime(outClip.length);
        transitionAnimator.SetTrigger("In");
    }

    private IEnumerator DelaySetTrigger(string trigger, int delayInFrames)
    {
        for (int i = 0; i < delayInFrames; i++) { yield return new WaitForEndOfFrame(); }
        transitionAnimator.SetTrigger(trigger);
    }
}
