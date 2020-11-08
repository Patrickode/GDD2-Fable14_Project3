using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PotionType", menuName = "ScriptableObjects/PotionType")]
public class PotionType : ScriptableObject
{
    public List<Ingredient> ingredients;
    public Color color;
}