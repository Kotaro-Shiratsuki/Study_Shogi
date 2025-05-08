using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using R3;

public class ShogiGrid : MonoBehaviour
{
    [field: Header("Reference")]
    [field: SerializeField]
    public GridData GridData { get; private set; }

    #region Getter Methods
    public Vector3 GetGridPosition()
    {
        return GridData.WorldPosition;
    }

    public Vector2Int GetShogiPosition()
    {
        return GridData.ShogiPosition;
    }

    public Vector2Int GetElementNumber()
    {
        return GridData.IndexNumber;
    }

    public GridState GetGridState()
    {
        return GridData.State;
    }

    public GridRegion GetGridRegion()
    {
        return GridData.Region;
    }
    #endregion
}
