using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetIngredientLabels : MonoBehaviour
{
    [SerializeField] private IngredientSource ingSource = null;
    [SerializeField] private TextMeshPro keyLabel = null;
    [SerializeField] private TextMeshPro nameLabel = null;

    private void Start()
    {
        string keyText = ingSource.Code.ToString();
        keyText = keyText.Replace("Comma", "<");
        keyLabel.text = keyText;

        string nameText = ingSource.Ingredient.ToString();
        nameText = nameText.Replace(" (Ingredient)", "");
        nameLabel.text = nameText;
    }
}
