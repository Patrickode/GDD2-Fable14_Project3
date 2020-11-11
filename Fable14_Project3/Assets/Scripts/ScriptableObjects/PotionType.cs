using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PotionType", menuName = "ScriptableObjects/PotionType")]
public class PotionType : ScriptableObject
{
    // Exact attribute count for each attribute needed to make the potion
    public Dictionary<IngredientAttribute, int> attributesRequired;

    public Color color;
}