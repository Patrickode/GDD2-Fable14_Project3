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

    [Header("Cooking Fields")]
    [Tooltip("How long to wait before the cooking indicator starts moving, in seconds.")]
    [SerializeField] private float startDelay = 0.5f;
    [Tooltip("How long it will take for the cooking indicator to reach the end of the cooking bar, in seconds.")]
    [SerializeField] private float cookDuration = 5.0f;
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
    private float cookTime = 0.0f;
    private float delayTime = 0.0f;

    public float CookPercent => cookTime / cookDuration;
    public float DelayPercent => delayTime / startDelay;

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
        cookingPotion = true;
    }

    void Update()
    {
        HandlePlayerInput();

        if (cookingPotion) { Cook(); }

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
            // Submit potion to current customer
            if (Time.timeScale > 0 && cookingPotion && Input.GetKeyDown(submitCode))
            {
                ParticleManager.SummonPoof(transform.position, Vector3.one * 2.5f);
                customerManager.SubmitPotion(currentPotion);
                Reset();
            }
        }
    }

    private void Cook()
    {
        // Update cook values
        if (DelayPercent < 1) { delayTime += Time.deltaTime; }
        else { cookTime += Time.deltaTime; }

        if (CookPercent >= perfectStartPercent)
        {
            OnPerfect?.Invoke();
        }
        if (CookPercent >= perfectEndPercent)
        {
            OnOvercooked?.Invoke();
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
        cookTime = 0;
        delayTime = 0;

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