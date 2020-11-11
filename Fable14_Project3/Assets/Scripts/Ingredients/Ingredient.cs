using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "ScriptableObjects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [Tooltip("The image that this ingredient uses.")]
    [SerializeField] private Sprite sprite = null;

    [Space(10)]

    [Tooltip("The type of this ingredient; the sense it appeals to.")]
    [SerializeField] private IngredientType senseType = IngredientType.LiquidBase;

    [Tooltip("The attributes of this ingredient. Determines what potions it's good for.")]
    [SerializeField] private IngredientAttribute[] attributes = null;

    public Sprite Sprite { get { return sprite; } }
    public IngredientType SenseType { get { return senseType; } }
    /// <summary>
    /// Returns a clone of this ingredient's attributes, so the local array can't be modified. <br/>
    /// <see href="https://docs.microsoft.com/en-us/dotnet/fundamentals/code-analysis/quality-rules/ca1819"/>
    /// </summary>
    /// <returns></returns>
    public IngredientAttribute[] GetAttributes() { return (IngredientAttribute[])attributes.Clone(); }
}