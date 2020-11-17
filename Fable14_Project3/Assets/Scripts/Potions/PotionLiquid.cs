using UnityEngine;

public class PotionLiquid : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
}
