using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem poofSystem = null;

    public static Action<Vector3, Vector3?> SummonPoof;

    private void Start()
    {
        SummonPoof += OnSummonPoof;
    }
    private void OnDestroy()
    {
        SummonPoof -= OnSummonPoof;
    }

    private void OnSummonPoof(Vector3 poofPosition, Vector3? poofLocalScale = null)
    {
        var summonedPoof = Instantiate(poofSystem, poofPosition, Quaternion.identity);
        //If poofLocalScale isn't null, set scale to that, otherwise, set it to (1, 1, 1)
        summonedPoof.transform.localScale = poofLocalScale ?? Vector3.one;
    }
}
