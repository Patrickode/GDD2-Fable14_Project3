﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixingBowl : MonoBehaviour
{
    [SerializeField] private KeyCode submitCode = KeyCode.Return;
    [SerializeField] private KeyCode discardCode = KeyCode.Backspace;

    private HashSet<IngredientType> addedTypes;
    private Dictionary<IngredientAttribute, int> attributeAmounts;

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

    private void Start()
    {
        addedTypes = new HashSet<IngredientType>();
        attributeAmounts = new Dictionary<IngredientAttribute, int>();

        IngredientSource.IngredientUsed += AddIngredientAttributes;
    }
    private void OnDestroy()
    {
        IngredientSource.IngredientUsed -= AddIngredientAttributes;
    }

    private void Update()
    {
        if (Input.GetKeyDown(submitCode))
        {
            if (addedTypes.Count < 8) { Debug.Log("Tried to submit mixture, not all types were added"); }
            else
            {
                ContentsSubmitted?.Invoke(attributeAmounts);
                Debug.Log($"Submitted mixture. (Attributes below)\n{GetMixtureAttributesAsString()}");
                ClearMixtureInfo();
            }
        }
        else if (Input.GetKeyDown(discardCode))
        {
            ContentsDiscarded?.Invoke();
            ClearMixtureInfo();
            Debug.Log("Discarded mixture.");
        }
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

        Debug.Log($"Added ingredient: {newIngredient} (Attributes below)\n" +
            $"{GetIngredientAttributesAsString(newIngredient)}");
    }

    private void ClearMixtureInfo()
    {
        addedTypes.Clear();
        attributeAmounts.Clear();
    }

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
