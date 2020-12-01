using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    private SoundEffectsManager soundEffectsManager;

    [SerializeField] private AudioClip cashRegisterSound = null;

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
    private float startPatience = 20.0f;
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
    public float PatiencePercentLeft => patience / startPatience;

    public void InitPatience(float patience)
    {
        Patience = patience;
        startPatience = patience;
    }

    private void Awake()
    {
        soundEffectsManager = FindObjectOfType<SoundEffectsManager>();

        scoreManager = FindObjectOfType<ScoreManager>();

        SpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animation>();
    }

    private void OnEnable()
    {
        OnRequestComplete += (potion) => { PlayOutAnimation(); };
        OnRequestComplete += IncreaseScore;
        OnRequestComplete += PlayCashRegisterSound;
        OnWrongPotionSubmitted += () => { PlayOutAnimation(); };
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
        {
            DecreaseScore(potionRequested);
            OnWrongPotionSubmitted?.Invoke();
        }
    }

    private void PlayOutAnimation()
    {
        animator.Play("CustomerOut");
    }

    private void IncreaseScore(Potion potion)
    {
        float score = potion.PotionType.score;

        //Add bonuses / penalties for cook state.
        score = ApplyCookStateModifiers(score, potion.cookState);

        float roundedScore = Mathf.Round(score);
        float bonus = 0.30f;
        float maxBonusPercent = 0.75f;
        float minBonusSeconds = 5;

        //If more than more than the max percentage of time is left, don't add extra, just add the full bonus.
        if (PatiencePercentLeft > maxBonusPercent)
        {
            score += roundedScore * bonus;
        }
        //Otherwise, add to the score based on how much patience is left. No bonus for less that 5 seconds left.
        else if (patience > minBonusSeconds)
        {
            score += roundedScore * Mathf.Lerp(
                0,
                bonus,
                (patience - minBonusSeconds) / (startPatience - minBonusSeconds)
            );
        }

        scoreManager.IncreaseScore(score);
    }
    private void DecreaseScore(PotionType potionType)
    {
        scoreManager.DecreaseScore(potionType.score - 100.01f);
    }

    private float ApplyCookStateModifiers(float scoreToModify, CookState potionState)
    {
        switch (potionState)
        {
            default:
            case CookState.Undercooked:
                return scoreToModify;
            case CookState.Perfect:
                return scoreToModify + 20f;
            case CookState.Overcooked:
                return scoreToModify - 15f;
        }
    }

    #region Sounds
    private void PlayCashRegisterSound()
    {
        soundEffectsManager.PlaySound(cashRegisterSound);
    }

    private void PlayCashRegisterSound(Potion potion)
    {
        PlayCashRegisterSound();
    }
    #endregion
}
