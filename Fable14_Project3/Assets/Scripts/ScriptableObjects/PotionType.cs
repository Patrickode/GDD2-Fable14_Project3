using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PotionType", menuName = "ScriptableObjects/PotionType")]
public class PotionType : ScriptableObject
{
    [System.Serializable]
    internal struct AttributeRequirement
    {
        public IngredientAttribute attribute;
        public int amount;

        public AttributeRequirement(IngredientAttribute attribute, int amount = 0)
        {
            this.attribute = attribute;
            this.amount = amount;
        }
    }

    // To be used in the inspector to set the values of the dictionary
    [SerializeField] private List<AttributeRequirement> attributeRequirements;

    // Exact attribute count for each attribute needed to make the potion
    public Dictionary<IngredientAttribute, int> requirements;

    public Color color;

    private void OnEnable()
    {
        requirements = new Dictionary<IngredientAttribute, int>();
        // Populate dictionary with values from the inspector
        foreach (AttributeRequirement requirement in attributeRequirements)
            requirements.Add(requirement.attribute, requirement.amount);
    }
}