using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct IngredientImagePair
{
    public Ingredient ingredient;
    public Sprite image;
}

public class PieceManager : MonoBehaviour
{
    [Tooltip("The list that holds what images correspond to what ingredients.")]
    [SerializeField] private IngredientImagePair[] ingredientImageList = null;
    [Space(15)]
    [SerializeField] private SpriteRenderer piecePrefab = null;
    [SerializeField] private float pieceSpawnHeight = 2.5f;
    [SerializeField] private float pieceSpawnXRange = 1.25f;

    private Dictionary<Ingredient, Sprite> ingredientDict = null;
    private List<SpriteRenderer> spawnedPieces = null;

    private void Start()
    {
        ingredientDict = new Dictionary<Ingredient, Sprite>();
        foreach (var pair in ingredientImageList)
        {
            ingredientDict.Add(pair.ingredient, pair.image);
        }

        spawnedPieces = new List<SpriteRenderer>();

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

        spawnedPieces.Add(spawnedIng);
    }

    private void ClearSpawnedPieces(Dictionary<IngredientAttribute, int> _) { ClearSpawnedPieces(); }
    private void ClearSpawnedPieces()
    {
        foreach (var piece in spawnedPieces)
        {
            Destroy(piece.gameObject);
        }
        spawnedPieces.Clear();
    }
}
