using System;
using UnityEngine;

public class PotionTableManager : MonoBehaviour
{
    public PotionTable potionTable;

    public PotionType FetchRandomPotionType()
    {
        return potionTable.FetchRandomPotionType();
    }
}
