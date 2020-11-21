using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionManager : MonoBehaviour
{
    [SerializeField] private PotionType[] possiblePotionTypes = null;
    private static PotionType[] possiblePTypes = null;

    private void Awake() { possiblePTypes = possiblePotionTypes; }

    public static bool TryGetPotionType(Dictionary<IngredientAttribute, int> attribAmounts,
        out PotionType potionType)
    {
        potionType = null;
        if (possiblePTypes == null) { return false; }

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

        //Return false if potion type is null and true if it's not.
        return potionType ? true : false;
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
