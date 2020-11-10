using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public PotionType potionType;

    // For Stirring
    public bool isStirring;
    public float currentStirAmount;
    public float neededStirAmount;

    // For Cooking
    public bool isCooking;
    public CookState currentCookState;
    public float cookTimer;
    public float cookInterval;
}
