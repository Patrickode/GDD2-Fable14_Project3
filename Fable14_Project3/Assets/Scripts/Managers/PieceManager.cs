using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceManager : MonoBehaviour
{
    [Tooltip("This array is used for determining which images correspond to which images.")]
    [SerializeField] private IngredientSource[] ingredientSources = null;
    [Space(15)]
    [SerializeField] private SpriteRenderer piecePrefab = null;
    [SerializeField] private float pieceSpawnHeight = 2.5f;
    [SerializeField] private float pieceSpawnXRange = 1.25f;
    [Space(10)]
    [SerializeField] private KeyCode mixCode = KeyCode.Space;
    [SerializeField] private float jostleIntensity = 1;
    [SerializeField] private float jostleInterval = 0.25f;

    private Dictionary<Ingredient, Sprite> ingredientDict = null;
    private List<SpriteRenderer> spawnedPieces = null;
    private List<Rigidbody2D> pieceRBs = null;
    private Coroutine jostleCoroutine = null;

    private void Start()
    {
        ingredientDict = new Dictionary<Ingredient, Sprite>();
        foreach (IngredientSource source in ingredientSources)
        {
            ingredientDict.Add(source.Ingredient, source.PieceImage);
        }

        spawnedPieces = new List<SpriteRenderer>();
        pieceRBs = new List<Rigidbody2D>();

        MixingBowl.IngredientAddedToBowl += SpawnIngredientPiece;
        MixingBowl.MixingComplete += ClearSpawnedPieces;
        MixingBowl.ContentsSubmitted += ClearSpawnedPieces;
        MixingBowl.ContentsDiscarded += ClearSpawnedPieces;
    }
    private void OnDestroy()
    {
        MixingBowl.IngredientAddedToBowl -= SpawnIngredientPiece;
        MixingBowl.MixingComplete -= ClearSpawnedPieces;
        MixingBowl.ContentsSubmitted -= ClearSpawnedPieces;
        MixingBowl.ContentsDiscarded -= ClearSpawnedPieces;
    }

    private void OnEnable() { jostleCoroutine = StartCoroutine(JostlePieces()); }
    private void OnDisable() { if (jostleCoroutine != null) { StopCoroutine(jostleCoroutine); } }

    private IEnumerator JostlePieces()
    {
        while (true)
        {
            //This all ensures that ingredients are jostled immediately on space press, unless space is 
            //being mashed faster than jostleInterval.

            //If space is down,
            if (Input.GetKey(mixCode))
            {
                //jostle the pieces, then wait for jostleInterval seconds.
                foreach (Rigidbody2D piece in pieceRBs)
                {
                    piece.AddForce(Vector2.up * jostleIntensity);
                }
                yield return new WaitForSeconds(jostleInterval);
            }
            //Otherwise, just move on.
            else { yield return null; }
        }
    }

    private void SpawnIngredientPiece(Ingredient ingToSpawn)
    {
        Vector3 spawnPos = transform.position;
        spawnPos.y += pieceSpawnHeight;
        spawnPos.x = UnityEngine.Random.Range(spawnPos.x - pieceSpawnXRange, spawnPos.x + pieceSpawnXRange);

        ParticleManager.SummonPoof(spawnPos, Vector3.one * 0.5f);
        var spawnedIng = Instantiate(
            piecePrefab,
            spawnPos,
            Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360), Vector3.forward)
        );

        if (ingredientDict.TryGetValue(ingToSpawn, out Sprite correspondingImage) && correspondingImage)
        {
            spawnedIng.sprite = correspondingImage;
        }
        else
        {
            switch (ingToSpawn.SenseType)
            {
                default:
                case IngredientType.LiquidBase:
                    break;
                case IngredientType.Sight:
                    spawnedIng.color = Color.red;
                    break;
                case IngredientType.Sound:
                    spawnedIng.color = new Color(1, 0.498f, 0, 1);
                    break;
                case IngredientType.Smell:
                    spawnedIng.color = Color.yellow;
                    break;
                case IngredientType.Taste:
                    spawnedIng.color = Color.green;
                    break;
                case IngredientType.Touch:
                    spawnedIng.color = Color.blue;
                    break;
                case IngredientType.Mind:
                    spawnedIng.color = new Color(0.624f, 0, 0.773f, 1);
                    break;
                case IngredientType.Soul:
                    spawnedIng.color = new Color(1, 0.412f, 0.706f, 1);
                    break;
            }
        }

        spawnedPieces.Add(spawnedIng);
        pieceRBs.Add(spawnedIng.GetComponent<Rigidbody2D>());
    }

    private void ClearSpawnedPieces(Dictionary<IngredientAttribute, int> _) { ClearSpawnedPieces(); }
    private void ClearSpawnedPieces()
    {
        foreach (SpriteRenderer piece in spawnedPieces)
        {
            Destroy(piece.gameObject);
        }
        spawnedPieces.Clear();
        pieceRBs.Clear();
    }
}
