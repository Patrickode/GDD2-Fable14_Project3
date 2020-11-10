using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookState
{
    Raw, 
    Undercooked, 
    Cooked,
    Overcooked, 
    Burnt
}

public class CookingManager : MonoBehaviour
{
    Potion currentPotion;
    
    // Start is called before the first frame update
    void Start()
    {
        // === this is all for TESTING (will be removed) ===
        currentPotion = new Potion();
        currentPotion.isStirring = false;
        currentPotion.currentStirAmount = 0.0f;
        currentPotion.neededStirAmount = 50.0f;
        currentPotion.isCooking = false;
        currentPotion.currentCookState = CookState.Raw;
        currentPotion.cookTimer = 0.0f; 
        currentPotion.cookInterval = 2.0f;
    }

    void FixedUpdate()
    {
        // Inputs to stir OR cook the potion
        // SPACE    - Stir the potion
        // B        - Toggle cooking the potion
        if(Input.GetKey(KeyCode.Space)
            && !currentPotion.isCooking) 
            Stir(currentPotion);
        else
            currentPotion.isStirring = false;

        if(Input.GetKeyDown(KeyCode.B)
            && !currentPotion.isStirring) {
            // Toggles whether the potion is cooking
            currentPotion.isCooking = !currentPotion.isCooking;

            if(currentPotion.isCooking)
                Debug.Log("Potion is cooking!");
            else
                Debug.Log("Potion is no longer cooking!");

        }

        if(currentPotion.isCooking) {
            Cook(currentPotion);
		}
    }

    void Stir(Potion potion)
	{
        potion.isStirring = true;

        // Increase "stirring" progress until its 100%
        if(potion.currentStirAmount < potion.neededStirAmount)
            potion.currentStirAmount += Time.deltaTime;

        float stirPercent = currentPotion.currentStirAmount / currentPotion.neededStirAmount * 100;
        Debug.Log("Test Stir Amount: " + stirPercent + "%");
    }

    void Cook(Potion potion)
	{
        potion.cookTimer += Time.deltaTime * 100;

        // If an interval is passed, UpCookState() is called
        if(potion.cookTimer / potion.cookInterval >= (int)potion.currentCookState)
            UpCookState(potion);

        // If the potion's CookState == CookState.Burnt,
        // the potion's isCooking becomes false
        if(potion.currentCookState == CookState.Burnt) {
            Debug.Log("The potion is burned!");
            potion.isCooking = false;
		}
    }

    void UpCookState(Potion potion)
	{
        // "Ups" the CookState of the potion depending on its current CookState
        // Uncooked     -> Undercooked
        // Undercooked  -> Cooked
        // Cooked       -> Overcooked
        // Overcooked   -> Burnt
        // Burnt        -> Do nothing
        if(potion.currentCookState == CookState.Burnt)
            return;

        potion.currentCookState += 1;
        Debug.Log("The potion is " + potion.currentCookState);
	}
}
