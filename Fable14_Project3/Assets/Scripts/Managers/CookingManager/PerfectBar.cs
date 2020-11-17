using UnityEngine;

public class PerfectBar : MonoBehaviour
{
    private CookingManager cookingManager;
    private Pivot pivot;

    private void Awake()
    {
        cookingManager = GetComponentInParent<CookingManager>();
        pivot = GetComponentInChildren<Pivot>();
    }

    void Start()
    {
        SetPivot(cookingManager.perfectEndPercent);
    }

    private void SetPivot(float value)
    {
        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;

        Vector3 temp = transform.localScale;
        temp.x = value;
        pivot.transform.localScale = temp;
    }
}
