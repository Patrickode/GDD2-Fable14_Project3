using System;
using UnityEngine;

public class Customer : MonoBehaviour
{
    public PotionData potionTypeRequested;

    public event Action OnRequestComplete;
    public event Action OnWrongPotionSubmitted;
    public event Action OnPatienceDepleted;

    // Time (in seconds) a customer will wait for their potion before leaving
    public float patience = 20.0f;

    private void Start()
    {
        // Get a random potion type when the customer is created
        potionTypeRequested = FindObjectOfType<PotionTableManager>().FetchRandomPotionType();
    }

    private void Update()
    {
        patience -= Time.deltaTime;
        if (patience <= 0)
            OnPatienceDepleted?.Invoke();
    }

    // Submit a potion to a customer, checks if it is the right one
    // Invoke the OnRequestComplete event if it is the right one
    public void SubmitPotion(Potion potion)
    {
        if (potion.potionData == potionTypeRequested)
            OnRequestComplete?.Invoke();
        else
            OnWrongPotionSubmitted?.Invoke();
    }
}
