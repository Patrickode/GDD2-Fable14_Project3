#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetArraySpacing : MonoBehaviour
{
    [SerializeField] private float rowSpacing = 1.75f;
    [SerializeField] private float columnSpacing = 1.15f;
    [SerializeField] private bool apply = false;
    [Space(10)]
    [SerializeField] private GameObject[] rows = null;
    [SerializeField] private GameObject[] row1Children = null;
    [SerializeField] private GameObject[] row2Children = null;
    [SerializeField] private GameObject[] row3Children = null;

    private void OnValidate()
    {
        if (apply)
        {
            apply = false;

            for (int i = 0; i < rows.Length; i++)
            {
                Vector3 newPos = rows[i].transform.localPosition;
                newPos.y = -rowSpacing * i;
                rows[i].transform.localPosition = newPos;
            }
            for (int i = 0; i < row1Children.Length; i++)
            {
                SetColumnSpacingIfNotNull(row1Children[i], i);
                SetColumnSpacingIfNotNull(row2Children[i], i);
                SetColumnSpacingIfNotNull(row3Children[i], i);
            }
        }
    }

    private void SetColumnSpacingIfNotNull(GameObject spacedObj, int index)
    {
        if (spacedObj)
        {
            Vector3 newPos = spacedObj.transform.localPosition;
            newPos.x = columnSpacing * index;
            spacedObj.transform.localPosition = newPos;
        }
    }
}
#endif