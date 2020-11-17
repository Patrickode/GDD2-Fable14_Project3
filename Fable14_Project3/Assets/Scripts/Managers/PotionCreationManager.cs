using System;
using System.Collections.Generic;
using UnityEngine;

public enum CreationState { MixingIngredients, Cooking }

public class PotionCreationManager : MonoBehaviour
{
    // Stage the player is in when creating a potion
    public static CreationState creationState = CreationState.MixingIngredients;

    private event Action OnPotionDiscarded;

    private void OnEnable()
    {
        OnPotionDiscarded += SetMixingState;
        MixingBowl.ContentsDiscarded += TriggerPotionDiscardedEvent;
        MixingBowl.ContentsSubmitted += SetCookingState;
    }

    private void OnDisable()
    {
        OnPotionDiscarded -= SetMixingState;
        MixingBowl.ContentsDiscarded -= TriggerPotionDiscardedEvent;
        MixingBowl.ContentsSubmitted -= SetCookingState;
    }

    private void SetMixingState()
    {
        PotionCreationManager.creationState = CreationState.MixingIngredients;
    }

    private void SetCookingState(Dictionary<IngredientAttribute, int> dictionary)
    {
        PotionCreationManager.creationState = CreationState.Cooking;
    }

    private void TriggerPotionDiscardedEvent()
    {
        OnPotionDiscarded?.Invoke();
    }
}
