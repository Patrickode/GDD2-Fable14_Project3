﻿using UnityEngine;

public class Potion : MonoBehaviour
{
    private PotionLiquid potionLiquid;

    [SerializeField] private PotionType potionType;
    public PotionType PotionType
    {
        get => potionType;
        set
        {
            potionType = value;
            if (potionType)
            {
                potionLiquid.spriteRenderer.color = potionType.color;
            }
            else
            {
                potionLiquid.spriteRenderer.color = Color.white;
            }
        }
    }

    // For Cooking
    public CookState cookState = CookState.Undercooked;
    // public bool isCooking;
    // public CookState currentCookState;
    // public float cookTimer;
    // public float cookInterval;

    private void Awake()
    {
        potionLiquid = GetComponentInChildren<PotionLiquid>();
    }
}