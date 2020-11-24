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

    public string potionName;
    public Color color;
    [Tooltip("Score awarded when potion is submitted successfully to a customer that asked for it.")]
    public float score;

    // To be used in the inspector to set the values of the dictionary
    [SerializeField] private List<AttributeRequirement> attributeRequirements = null;

    // Exact attribute count for each attribute needed to make the potion
    public Dictionary<IngredientAttribute, int> requirements;

    private void OnEnable()
    {
        requirements = new Dictionary<IngredientAttribute, int>();
        // Populate dictionary with values from the inspector
        if (attributeRequirements != null)
        {
            foreach (AttributeRequirement requirement in attributeRequirements)
                requirements.Add(requirement.attribute, requirement.amount);
        }
    }
}