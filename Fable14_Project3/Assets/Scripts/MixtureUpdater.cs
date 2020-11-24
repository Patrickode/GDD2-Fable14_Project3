using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MixtureUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mixtureRend = null;

    private void Start()
    {
        MixingBowl.MixingComplete += SetAndShowMixture;
        MixingBowl.ContentsSubmitted += EmptyMixture;
        MixingBowl.ContentsDiscarded += EmptyMixture;
    }
    private void OnDestroy()
    {
        MixingBowl.MixingComplete -= SetAndShowMixture;
        MixingBowl.ContentsSubmitted -= EmptyMixture;
        MixingBowl.ContentsDiscarded -= EmptyMixture;
    }

    private void EmptyMixture(Dictionary<IngredientAttribute, int> _) { EmptyMixture(); }
    private void EmptyMixture() { mixtureRend.enabled = false; }

    private void SetAndShowMixture(Dictionary<IngredientAttribute, int> attributeAmounts)
    {
        mixtureRend.enabled = true;

        ConversionManager.TryGetPotionType(attributeAmounts, out PotionType pType);
        if (pType != null)
        {
            mixtureRend.color = pType.color;
        }
        else
        {
            mixtureRend.color = Color.white;
        }
    }
}
