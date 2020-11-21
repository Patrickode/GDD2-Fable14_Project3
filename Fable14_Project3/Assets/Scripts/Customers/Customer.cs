using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private ScoreManager scoreManager;

    private Animation animator;

    public PotionType potionRequested;

    public SpriteRenderer SpriteRenderer { get; private set; }

    public event Action<Potion> OnRequestComplete;
    public event Action OnWrongPotionSubmitted;
    public event Action OnPatienceDepleted;

    public event Action OnDestroyed;

    // Time (in seconds) a customer will wait for their potion before leaving
    private float patience = 20.0f;
    public float Patience
    {
        get => patience;
        set
        {
            patience = value;
            if (patience <= 0)
                OnPatienceDepleted?.Invoke();
        }
    }

    private void Awake()
    {
        scoreManager = FindObjectOfType<ScoreManager>();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animation>();
    }

    private void OnEnable()
    {
        OnRequestComplete += (potion) => { PlayOutAnimation(); };
        OnRequestComplete += IncreaseScore;
        OnPatienceDepleted += PlayOutAnimation;
    }

    private void OnDisable()
    {
        // Unsubscribe all event handlers
        OnRequestComplete = null;
        OnWrongPotionSubmitted = null;
        OnPatienceDepleted = null;
    }

    private void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }

    // Submit a potion to a customer, checks if it is the right one
    // Invoke the OnRequestComplete event if it is the right one
    public void SubmitPotion(Potion potion)
    {
        if (potion.PotionType == potionRequested)
            OnRequestComplete?.Invoke(potion);
        else
            OnWrongPotionSubmitted?.Invoke();
    }

    private void PlayOutAnimation()
    {
        animator.Play("CustomerOut");
    }

    private void IncreaseScore(Potion potion)
    {
        scoreManager.IncreaseScore(potion);
    }
}
