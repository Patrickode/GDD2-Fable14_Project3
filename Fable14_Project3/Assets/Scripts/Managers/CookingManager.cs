using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookState
{
    Undercooked, 
    Perfect,
    Overcooked
}

public class CookingManager : MonoBehaviour
{
    Potion currentPotion;
    
    void Start()
    {
        // === this is all for TESTING (will be removed) ===
        currentPotion = new Potion();
        currentPotion.isStirring = false;
        currentPotion.currentStirAmount = 0.0f;
        currentPotion.neededStirAmount = 50.0f;
        currentPotion.isCooking = false;
        currentPotion.currentCookState = CookState.Undercooked;
        currentPotion.cookTimer = 0.0f; 
        currentPotion.cookInterval = 3.0f;
    }

	void Update()
    {
        // Stirs the potion 
        // if SPACE is pressed
        // if it hasn't fully been stirred yet
        if(Input.GetKey(KeyCode.Space)
            && currentPotion.currentStirAmount < currentPotion.neededStirAmount)
            Stir(currentPotion);
        else
            currentPotion.isStirring = false;

        // Cooks the potion
        // if Return (Enter) is pressed 
        // if the potion is fully stirred
        if(Input.GetKeyDown(KeyCode.Return)
            && currentPotion.currentStirAmount >= currentPotion.neededStirAmount) {
            currentPotion.isCooking = true;
            Debug.Log("Potion is cooking! It is " + currentPotion.currentCookState);
        }

        // if the potion is cooking, it calls Cook()
        if(currentPotion.isCooking
            && currentPotion.currentCookState != CookState.Overcooked) {
            Cook(currentPotion);
        }
    }

    /// <summary>
    /// Stirs a potion, up-ing its stir amount value
    /// </summary>
    /// <param name="potion">The potion being stirred</param>
    void Stir(Potion potion)
	{
        potion.isStirring = true;

        // Increase "stirring" progress until its 100%
        if(potion.currentStirAmount < potion.neededStirAmount)
            potion.currentStirAmount += Time.deltaTime * 10;

        float stirPercent = (int)(currentPotion.currentStirAmount / currentPotion.neededStirAmount * 100);
        Debug.Log("Stir Amount: " + stirPercent + "%");
    }

    /// <summary>
    /// Cooks a potion, adding to a cook "timer"
    /// </summary>
    /// <param name="potion">The potion being cooked</param>
    void Cook(Potion potion)
	{
        potion.cookTimer += Time.deltaTime;

        // If an interval is passed, the CookState is "up'd"
        if(potion.cookTimer / potion.cookInterval >= (int)potion.currentCookState + 1) {
            // "Ups" the CookState of the potion depending on its current CookState
            // Undercooked  -> Ready
            // Ready        -> Overcooked
            // Overcooked   -> Do nothing
            if(potion.currentCookState != CookState.Overcooked)
                potion.currentCookState += 1;
                
            Debug.Log("The potion is " + potion.currentCookState);
        }
    }
}
