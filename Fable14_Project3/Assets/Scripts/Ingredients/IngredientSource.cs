using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngredientSource : MonoBehaviour
{
    [SerializeField] private KeyCode code = KeyCode.None;
    [SerializeField] private Ingredient ingredient = null;
    [Tooltip("The image for this ingredient when it's put in the mixing bowl.")]
    [SerializeField] private Sprite pieceImage = null;
    [Space(15)]
    [SerializeField] private SpriteRenderer spRend = null;
    [SerializeField] private float flickerTime = 0;
    [SerializeField] private Color flickerColor = Color.black;
    private Color initialColor;
    private Coroutine flickerCoroutine;

    public KeyCode Code { get { return code; } }
    public Ingredient Ingredient { get { return ingredient; } }
    public Sprite PieceImage { get { return pieceImage; } }

    /// <summary>
    /// Invoked when an ingredient source's keycode is pressed, i.e., that source's ingredient is used. <br/>
    /// <i>Parameter:</i> The ingredient that was used.
    /// </summary>
    public static Action<Ingredient> IngredientUsed;

    private void Start()
    {
        initialColor = spRend.color;

        if (code == KeyCode.None)
        {
            Debug.LogWarning($"{gameObject.name}: This IngredientSource's KeyCode isn't assigned, so it will " +
                $"never be used. Assign a KeyCode to {gameObject.name}'s IngredientSource.");
        }
    }

    private void Update()
    {
        if (Time.timeScale > 0 && Input.GetKeyDown(code))
        {
            if (flickerCoroutine != null) { StopCoroutine(flickerCoroutine); }
            flickerCoroutine = StartCoroutine(Flicker());

            if (ingredient) { IngredientUsed?.Invoke(ingredient); }
            else
            {
                Debug.LogWarning($"{gameObject.name}: This IngredientSource's ingredient isn't assigned! " +
                    $"Assign it in the inspector.");
            }
        }
    }

    private IEnumerator Flicker()
    {
        spRend.color = flickerColor;
        yield return new WaitForSeconds(flickerTime);
        spRend.color = initialColor;
    }
}
