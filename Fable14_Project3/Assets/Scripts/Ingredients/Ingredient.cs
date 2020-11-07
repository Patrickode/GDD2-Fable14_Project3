using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewIngredient", menuName = "ScriptableObjects/Ingredient")]
public class Ingredient : ScriptableObject
{
    [Tooltip("The image that this ingredient uses.")]
    [SerializeField] private Sprite sprite;

    [Space(10)]

    [Tooltip("The type of this ingredient; the sense it appeals to.")]
    [SerializeField] private IngredientType senseType;

    [Tooltip("The attributes of this ingredient. Determines what potions it's good for.")]
    [SerializeField] private IngredientAttribute[] attributes;
}