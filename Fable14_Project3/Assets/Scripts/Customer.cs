using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public PotionType potionRequested;

    public event Action OnRequestComplete;
    public event Action OnWrongPotionSubmitted;
    public event Action OnPatienceDepleted;

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

    private void OnEnable()
    {
        OnRequestComplete += PlayOutAnimation;
        OnPatienceDepleted += PlayOutAnimation;
    }

    private void OnDisable()
    {
        // Unsubscribe all event handlers
        OnRequestComplete = null;
        OnWrongPotionSubmitted = null;
        OnPatienceDepleted = null;
}

    // Submit a potion to a customer, checks if it is the right one
    // Invoke the OnRequestComplete event if it is the right one
    public void SubmitPotion(Potion potion)
    {
        if (potion.potionType == potionRequested)
            OnRequestComplete?.Invoke();
        else
            OnWrongPotionSubmitted?.Invoke();
    }

    private void PlayOutAnimation()
    {
        Animation animator = GetComponentInChildren<Animation>();
        animator.Play("CustomerOut");
    }
}
