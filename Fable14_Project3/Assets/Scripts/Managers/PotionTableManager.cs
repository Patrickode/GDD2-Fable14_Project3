using System;
using UnityEngine;

public class PotionTableManager : MonoBehaviour
{
    public PotionTable potionTable;

    public PotionType FetchRandomPotionType()
    {
        return potionTable.FetchRandomPotionType();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log(FetchRandomPotionType());
        }
    }
}
