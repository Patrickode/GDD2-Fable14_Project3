using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixtureUpdater : MonoBehaviour
{
    [SerializeField] private SpriteRenderer mixtureRend = null;
    [SerializeField] private PotionType[] potionTypes = null;

    private void Start()
    {
        MixingBowl.MixingComplete += ParseAttributesToType;
        MixingBowl.ContentsSubmitted += EmptyMixture;
        MixingBowl.ContentsDiscarded += EmptyMixture;
    }
    private void OnDestroy()
    {
        MixingBowl.MixingComplete -= ParseAttributesToType;
        MixingBowl.ContentsSubmitted -= EmptyMixture;
        MixingBowl.ContentsDiscarded -= EmptyMixture;
    }

    private void EmptyMixture(Dictionary<IngredientAttribute, int> _) { EmptyMixture(); }
    private void EmptyMixture() { mixtureRend.enabled = false; }

    private void ParseAttributesToType(Dictionary<IngredientAttribute, int> attributeAmounts)
    {
        //Check each potion type we have,
        foreach (PotionType pType in potionTypes)
        {
            //and if attributeAmounts has everything required for this potion type,
            if (MeetsAttributeRequirements(pType, attributeAmounts))
            {
                //enable the mixture renderer and make it the color of this potion type.
                //We then leave this method because we assume this is the best match we will find.
                mixtureRend.enabled = true;
                mixtureRend.color = pType.color;
                return;
            }
        }
    }

    private bool MeetsAttributeRequirements(PotionType potionType,
        Dictionary<IngredientAttribute, int> attributeAmounts)
    {
        //Get all the attributes the potion type needs, and check if the attribute amounts has enough of 
        //each attribute the potion needs
        var attribs = potionType.requirements.Keys;
        foreach (IngredientAttribute attrib in attribs)
        {
            //If attributeAmounts doesn't have this required attribute, OR it doesn't have enough of this
            //required attribute, return false.
            if (!attributeAmounts.TryGetValue(attrib, out var amount)
                || amount < potionType.requirements[attrib])
            {
                return false;
            }
        }

        //If we got this far, attributeAmounts meets all of potionType's requirements.
        return true;
    }
}
