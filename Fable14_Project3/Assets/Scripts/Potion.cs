using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public PotionType potionType;

    // For Cooking
    public bool isCooking;
    public CookState currentCookState;
    public float cookTimer;
    public float cookInterval;
}
