using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    [SerializeField] private KeyCode submitCode = KeyCode.Return;
    [SerializeField] private KeyCode stirCode = KeyCode.Space;
    [SerializeField] private KeyCode discardCode = KeyCode.Backspace;

    [Space(10)]
    [SerializeField] private AudioClip poofSound = null;
    [SerializeField] private AudioClip stirringSound = null;
    [SerializeField] private AudioClip splashSound = null;
    [SerializeField] AudioClip ingredientUsedSound = null;
    private SoundEffectsManager soundEffectsManager;

    private HashSet<IngredientType> addedTypes;
    private Dictionary<IngredientAttribute, int> attributeAmounts;
    // For Stirring
    [Space(10)]
    [SerializeField] private TextMeshPro progressLabel = null;
    [SerializeField] private Transform progressBarPivot = null;
    [SerializeField] private float stirDuration = 5.0f;
    [Tooltip("The speed at which the progress bar follows stirAmount, in percentage per second. 1 = 100%. " +
        "Not to be confused with stir duration!")]
    [SerializeField] private float percentPerSecond = 5f;
    private float stirAmount = 0.0f;
    private float lastStirAmount = -1.0f;
    private Vector3 progressBarTarget;

    /// <summary>
    /// Invoked when an ingredient is added to the bowl.<br/>
    /// <i>Parameter:</i> The ingredient that was added.
    /// </summary>
    public static Action<Ingredient> IngredientAddedToBowl;
    /// <summary>
    /// Invoked when mixing is complete. <br/>
    /// <i>Parameter:</i> The attributes of this mixture, and the amount of each.
    /// </summary>
    public static Action<Dictionary<IngredientAttribute, int>> MixingComplete;
    /// <summary>
    /// Invoked when the contents of this potion are put aside to brew. <br/>
    /// <i>Parameter:</i> The attributes of this mixture, and the amount of each.
    /// </summary>
    public static Action<Dictionary<IngredientAttribute, int>> ContentsSubmitted;
    /// <summary>
    /// Invoked when the contents of this potion are discarded.
    /// </summary>
    public static Action ContentsDiscarded;

    private void Awake()
    {
        soundEffectsManager = FindObjectOfType<SoundEffectsManager>();
    }

    private void Start()
    {
        progressBarPivot.localScale = new Vector3(
            0,
            progressBarPivot.localScale.y,
            progressBarPivot.localScale.z
        );
        progressBarTarget = progressBarPivot.localScale;

        addedTypes = new HashSet<IngredientType>();
        attributeAmounts = new Dictionary<IngredientAttribute, int>();

        IngredientSource.IngredientUsed += AddIngredientAttributes;
    }
    private void OnDestroy()
    {
        IngredientSource.IngredientUsed -= AddIngredientAttributes;
    }

    private void OnEnable()
    {
        IngredientAddedToBowl += PlayPoofSound;
        IngredientSource.IngredientUsed += PlayIngredientUsedSound;
        ContentsDiscarded += PlayPoofSound;
    }

    private void OnDisable()
    {
        IngredientSource.IngredientUsed -= PlayIngredientUsedSound;
        IngredientAddedToBowl = null;
        ContentsDiscarded = null;
    }

    private void Update()
    {
        if (progressLabel) { progressLabel.text = $"{stirAmount * 100:F0}% Stirred"; }
        if (progressBarPivot)
        {
            progressBarTarget.x = stirAmount;
            progressBarPivot.localScale = Vector3.MoveTowards
                (progressBarPivot.localScale,
                progressBarTarget,
                percentPerSecond * Time.deltaTime
            );
        }

        if (PotionCreationManager.creationState == CreationState.MixingIngredients)
        {
            if (Time.timeScale > 0 && Input.GetKeyDown(submitCode))
            {
                if (addedTypes.Count < 8 || stirAmount < 1)
                {
                    //Debug.Log("Tried to submit mixture, not all types were added / mixture not stirred");
                }
                else
                {
                    ContentsSubmitted?.Invoke(attributeAmounts);
                    //Debug.Log($"Submitted mixture. (Attributes below)\n{GetMixtureAttributesAsString()}");
                    ClearMixtureInfo();
                }
            }
            else if (Input.GetKey(stirCode))
            {
                if (addedTypes.Count >= 8 && stirAmount < 1)
                {
                    // Make sounds
                    PlayStirringSound();
                }

                //Stir the mixture if all ingredients have been added and the mixture is not fully stirred already
                if (addedTypes.Count >= 8 && stirAmount < 1) { stirAmount += Time.deltaTime / stirDuration; }
            }
        }

        if (Input.GetKeyUp(stirCode) || PotionCreationManager.creationState != CreationState.MixingIngredients || stirAmount >= 1)
        {
            StopStirringSound();
        }
        

        if (Time.timeScale > 0 && addedTypes.Count > 0 && Input.GetKeyDown(discardCode))
        {
            ContentsDiscarded?.Invoke();
            ClearMixtureInfo();
            //Debug.Log("Discarded mixture.");
        }

        if (stirAmount >= 1 && lastStirAmount < 1)
        {
            PlaySplashSound();
            InvokePoofAction();
            MixingComplete?.Invoke(attributeAmounts);
        }
        lastStirAmount = stirAmount;
    }

    private void AddIngredientAttributes(Ingredient newIngredient)
    {
        //If an ingredient of this new ingredient's type has already been added, bail out. Only one type per 
        //ingredient allowed.
        if (addedTypes.Contains(newIngredient.SenseType)) { return; }
        //Add this ingredient's type to the addedTypes set so another of the same type can't be added.
        addedTypes.Add(newIngredient.SenseType);

        //Get the attributes of this ingredient, and for each attribute,
        IngredientAttribute[] newAttrs = newIngredient.GetAttributes();
        foreach (IngredientAttribute attr in newAttrs)
        {
            //If the attribute is not in the amounts dictionary, add it to the amounts dictionary.
            if (!attributeAmounts.ContainsKey(attr))
            {
                attributeAmounts.Add(attr, 1);
            }
            //If the attribute IS in the amounts dictionary, add one to its value.
            else
            {
                attributeAmounts[attr]++;
            }
        }

        IngredientAddedToBowl?.Invoke(newIngredient);

        //Debug.Log($"Added ingredient: {newIngredient} (Attributes below)\n" +
        //    $"{GetIngredientAttributesAsString(newIngredient)}");
    }

    private void ClearMixtureInfo()
    {
        addedTypes.Clear();
        attributeAmounts.Clear();
        stirAmount = 0;

        InvokePoofAction();
    }

    #region Sounds
    private void PlayPoofSound()
    {
        soundEffectsManager.PlaySound(poofSound);
    }

    private void PlayPoofSound(Ingredient ingredient)
    {
        PlayPoofSound();
    }

    private void PlayStirringSound()
    {
        soundEffectsManager.StartLoop(stirringSound);
    }

    private void StopStirringSound()
    {
        soundEffectsManager.StopLoop();
    }

    private void PlaySplashSound()
    {
        soundEffectsManager.PlaySound(splashSound);
    }

    private void PlaySplashSound(Dictionary<IngredientAttribute, int> mix)
    {
        PlaySplashSound();
    }

    private void PlayIngredientUsedSound()
    {
        soundEffectsManager.PlaySound(ingredientUsedSound);
    }

    private void PlayIngredientUsedSound(Ingredient ingredient)
    {
        PlayIngredientUsedSound();
    }
    #endregion

    private void InvokePoofAction() { ParticleManager.SummonPoof?.Invoke(transform.position, Vector3.one * 2.5f); }

#if UNITY_EDITOR
    private string GetMixtureAttributesAsString()
    {
        if (attributeAmounts.Count <= 0) { return "[attribute dict is empty]"; }

        string attrString = "";
        foreach (var attrAmountPair in attributeAmounts)
        {
            attrString += $"[{attrAmountPair.Key}, {attrAmountPair.Value}], ";
        }
        return attrString.Remove(attrString.Length - 2);
    }

    private string GetIngredientAttributesAsString(Ingredient ingredient)
    {
        IngredientAttribute[] attrs = ingredient.GetAttributes();
        if (attrs.Length <= 0) { return "(attribute array is empty)"; }

        string attrString = "";
        foreach (var attr in attrs)
        {
            attrString += $"{attr}, ";
        }
        return attrString.Remove(attrString.Length - 2);
    }
#endif
}
