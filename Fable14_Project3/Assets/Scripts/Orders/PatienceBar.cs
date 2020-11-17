using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatienceBar : MonoBehaviour
{
    public void SetPatience(float value)
    {
        if (value < 0)
            value = 0;
        else if (value > 1)
            value = 1;

        Vector3 temp = transform.localScale;
        temp.x = value;
        transform.localScale = temp;
    }
}
