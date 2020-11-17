using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingIndicator : MonoBehaviour
{
    private CookingManager cookingManager;

    [SerializeField] private float barWidth = 4.0f;

    private float Left => barWidth / -2.0f;
    private float Right => barWidth / 2.0f;

    private void Awake()
    {
        cookingManager = GetComponentInParent<CookingManager>();
    }

    private void Update()
    {
        SetPosition();
    }

    private void SetPosition()
    {
        Vector3 temp = transform.localPosition;
        temp.x = Mathf.Lerp(Left, Right, cookingManager.CookPercent);
        transform.localPosition = temp;
    }
}
