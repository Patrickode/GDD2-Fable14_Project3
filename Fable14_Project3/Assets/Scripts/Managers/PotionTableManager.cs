using System;
using UnityEngine;

public class PotionTableManager : MonoBehaviour
{
    public PotionTable potionTable;

    public PotionData FetchRandomPotionType()
    {
        return potionTable.FetchRandomPotionType();
    }
}
