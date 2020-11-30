using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionManager : MonoBehaviour
{
    [SerializeField] private PotionTable possiblePotionTypes = null;
    [Tooltip("The type to use if no other suitable type is found.")]
    [SerializeField] private PotionType fallbackPotionType = null;
    private static PotionType[] possiblePTypes = null;
    private static PotionType fallbackType = null;

    private void Awake()
    {
        possiblePTypes = possiblePotionTypes.Types;
        fallbackType = fallbackPotionType;
    }

    public static bool TryGetPotionType(Dictionary<IngredientAttribute, int> attribAmounts,
        out PotionType potionType)
    {
        if (possiblePTypes == null)
        {
            potionType = fallbackType;
            return false;
        }

        potionType = null;

        //For each possible potion type,
        foreach (PotionType pType in possiblePTypes)
        {
            //see if that potion type's requirements are ment by the attrib amount dictionary we were given.
            if (MeetsAttributeRequirements(pType, attribAmounts))
            {
                //If it does, set potion type and break out of the loop; we assume that there will be no better
                //match later on.
                potionType = pType;
                break;
            }
        }

        //Return true if potion type is not null and false otherwise.
        if (potionType) { return true; }
        else
        {
            potionType = fallbackType;
            return false;
        }
    }

    public static bool MeetsAttributeRequirements(PotionType potionType,
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
