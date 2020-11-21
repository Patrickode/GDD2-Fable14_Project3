using System;
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
    [Header("Key Codes")]
    [SerializeField] private KeyCode submitCode = KeyCode.Return;
    [SerializeField] private KeyCode cookCode = KeyCode.Space;

    [Header("Cooking Fields")]
    [SerializeField] private float cookDuration = 5.0f;                         // in seconds
    [Tooltip("Percent of cooking duration where potion becomes perfect.")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float perfectStartPercent = 0.4f;
    [Tooltip("Percent of cooking duration where potion stops being perfect.")]
    [SerializeField]
    [Range(0.0f, 1.0f)]
    public float perfectEndPercent = 0.8f;

    private CustomerManager customerManager;
    private Potion currentPotion;

    private bool cookingPotion = false;
    private bool potionReady = false;                                           // Potion is ready to be submitted
    private float cookTime = 0.0f;

    public float CookPercent => cookTime / cookDuration;

    private event Action OnPerfect;
    private event Action OnOvercooked;

    private void Awake()
    {
        customerManager = FindObjectOfType<CustomerManager>();
        currentPotion = GetComponentInChildren<Potion>();
    }

    //    void Start()
    //    {
    //        // === this is all for TESTING (will be removed) ===
    //        currentPotion = new Potion();
    //        currentPotion.isCooking = false;
    //        currentPotion.currentCookState = CookState.Undercooked;
    //        currentPotion.cookTimer = 0.0f;
    //        currentPotion.cookInterval = 3.0f;
    //    }

    private void OnEnable()
    {
        OnPerfect += SetPerfect;
        OnOvercooked += SetOvercooked;
        MixingBowl.ContentsSubmitted += AssignPotionType;
    }

    private void OnDisable()
    {
        OnPerfect -= SetPerfect;
        OnOvercooked -= SetOvercooked;
        MixingBowl.ContentsSubmitted -= AssignPotionType;
    }

    private void AssignPotionType(Dictionary<IngredientAttribute, int> attributeAmounts)
    {
        ParticleManager.SummonPoof(transform.position, Vector3.one * 2.5f);
        ConversionManager.TryGetPotionType(attributeAmounts, out PotionType pType);
        currentPotion.PotionType = pType;
    }

    void Update()
    {
        HandlePlayerInput();

        if (cookingPotion && !potionReady)
        {
            Cook();
        }

        // For test on Tuesday only
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentPotion.PotionType = Resources.Load<PotionType>("RedTestPotion");
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentPotion.PotionType = Resources.Load<PotionType>("BlueTestPotion");
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentPotion.PotionType = null;
        }

        // Cooks the potion
        //        // if Return (Enter) is pressed 
        //        // if the potion is fully stirred
        //        if (Input.GetKeyDown(KeyCode.Return))
        //        {
        //            currentPotion.isCooking = true;
        //            Debug.Log("Potion is cooking! It is " + currentPotion.currentCookState);
        //        }

        //        // if the potion is cooking, it calls Cook()
        //        if (currentPotion.isCooking
        //            && currentPotion.currentCookState != CookState.Overcooked)
        //        {
        //            Cook(currentPotion);
        //        }
    }

    private void HandlePlayerInput()
    {
        if (PotionCreationManager.creationState == CreationState.Cooking)
        {
            if (cookingPotion)
            {
                if (potionReady)
                {
                    // Submit potion to current customer
                    if (Input.GetKeyDown(submitCode))
                    {
                        ParticleManager.SummonPoof(transform.position, Vector3.one * 2.5f);
                        customerManager.SubmitPotion(currentPotion);
                        Reset();
                    }
                }

                // Readies potion at current cook state
                if (Input.GetKeyDown(cookCode))
                {
                    potionReady = true;
                }
            }
            else
            {
                // Start cooking potion
                if (Input.GetKeyDown(submitCode))
                {
                    cookingPotion = true;
                }
            }
        }
    }

    private void Cook()
    {
        // Update cook values
        cookTime += Time.deltaTime;

        if (CookPercent >= perfectStartPercent)
        {
            OnPerfect?.Invoke();
        }
        if (CookPercent >= perfectEndPercent)
        {
            OnOvercooked?.Invoke();
        }
        if (CookPercent >= 1)
        {
            potionReady = true;
        }
    }

    //    /// <summary>
    //    /// Cooks a potion, adding to a cook "timer"
    //    /// </summary>
    //    /// <param name="potion">The potion being cooked</param>
    //    void Cook(Potion potion)
    //    {
    //        potion.cookTimer += Time.deltaTime;

    //        // If an interval is passed, the CookState is "up'd"
    //        if (potion.cookTimer / potion.cookInterval >= (int)potion.currentCookState + 1)
    //        {
    //            // "Ups" the CookState of the potion depending on its current CookState
    //            // Undercooked  -> Ready
    //            // Ready        -> Overcooked
    //            // Overcooked   -> Do nothing
    //            if (potion.currentCookState != CookState.Overcooked)
    //                potion.currentCookState += 1;

    //            Debug.Log("The potion is " + potion.currentCookState);
    //        }
    //    }
    //}

    private void Reset()
    {
        currentPotion.PotionType = null;
        currentPotion.cookState = CookState.Undercooked;
        cookingPotion = false;
        potionReady = false;
        cookTime = 0;

        PotionCreationManager.creationState = CreationState.MixingIngredients;
    }

    private void SetPerfect()
    {
        currentPotion.cookState = CookState.Perfect;
    }

    private void SetOvercooked()
    {
        currentPotion.cookState = CookState.Overcooked;
    }
}