using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class KomaAnimationData
{
    [Header("State Group Parameter Name")]
    [SerializeField] private string fieldedParameterName = "Fielded";

    public int FieldedParameterHash {  get; private set; }
    public void Initialize()
    {
        fieldedParameterName += " initialized";
        FieldedParameterHash = 1;
    }
}