using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CookingIndicator : MonoBehaviour
{
    private CookingManager cookingManager;

    [SerializeField] private float barWidth = 4.0f;
    [SerializeField] private float pulseScaleAmount = 1.5f;
    [SerializeField] private float maxMoveDelta = 0.25f;
    private Vector3 normalScale;
    private Vector3 pulseScale;

    private float Left => barWidth / -2.0f;
    private float Right => barWidth / 2.0f;

    private void Start()
    {
        cookingManager = GetComponentInParent<CookingManager>();

        normalScale = transform.localScale;
        pulseScale = transform.localScale * pulseScaleAmount;
    }

    private void Update()
    {
        SetPosition();
        SetScale();
    }

    private void SetPosition()
    {
        Vector3 temp = transform.localPosition;
        temp.x = Mathf.Clamp(
            Mathf.Lerp(Left, Right, cookingManager.CookPercent),
            temp.x - maxMoveDelta, temp.x + maxMoveDelta
        );
        transform.localPosition = temp;
    }
    private void SetScale()
    {
        float dPercent = cookingManager.DelayPercent;

        if (dPercent <= 0.5f)
        {
            transform.localScale = SmoothStepVector3(normalScale, pulseScale, dPercent * 2);
        }
        else
        {
            transform.localScale = SmoothStepVector3(pulseScale, normalScale, (dPercent - 0.5f) * 2);
        }
    }

    private Vector3 SmoothStepVector3(Vector3 from, Vector3 to, float percent)
    {
        return new Vector3(
            Mathf.SmoothStep(from.x, to.x, percent),
            Mathf.SmoothStep(from.y, to.y, percent),
            Mathf.SmoothStep(from.z, to.z, percent)
        );
    }
}
