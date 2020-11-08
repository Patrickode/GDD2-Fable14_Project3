using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.PlayerLoop;

[CreateAssetMenu(fileName = "New PotionTable", menuName = "ScriptableObjects/PotionTable")]
public class PotionTable : ScriptableObject
{
    [System.Serializable]
    internal struct PotionProbability
    {
        public PotionType potionType;
        // Probability that this potion will be fetched from the pool
        public int weight;

        public PotionProbability(PotionType type = null, int weight = 1)
        {
            this.potionType = type;
            this.weight = weight;
        }
    }

    [SerializeField] private List<PotionProbability> possiblePotionTypes = null;
    private int maxWeight;

    private void OnEnable()
    {
        maxWeight = possiblePotionTypes.Sum(p => p.weight);
    }

    // Returns a random potion with probabilities calculated by their weight
    public PotionType FetchRandomPotionType()
    {
        int random = Random.Range(0, maxWeight);
        for (int i = 0; i < possiblePotionTypes.Count; i++)
        {
            // If random is within the index of the potion and its weight, return it
            if (random >= i && random < i + possiblePotionTypes[i].weight)
            {
                return possiblePotionTypes[i].potionType;
            }
        }

        // This line of code should be unreachable
        return null;
    }
}
