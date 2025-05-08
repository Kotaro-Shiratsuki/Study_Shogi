using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A helper class that allows DualKeyDictionary to be treated like a Dictionary.
/// </summary>
public class GridDictionaryController
{
    private DualKeyDictionary<string, Vector2Int, ShogiGrid> gridDictionary;
    private Index _index;

    public GridDictionaryController(Index index)
    {
        gridDictionary = new DualKeyDictionary<string, Vector2Int, ShogiGrid>();
        _index = index;
    }

    #region Basement Methods
    public void Add(Vector2Int firstKey, Vector2Int secondKey, ShogiGrid grid)
    {
        Add(_index.ParseShogiPosition(firstKey), secondKey, grid);
    }

    public void Add(string firstKey, Vector2Int secondKey, ShogiGrid grid)
    {
        gridDictionary.Add(firstKey, secondKey, grid);
    }

    public GridState GetGridState(string key)
    {
        return GetValue(key).GetGridState();
    }

    public GridState GetGridState(Vector2Int key)
    {
        return GetValue(key).GetGridState();
    }

    public void ChangeGridState(string key, GridState state)
    {
        GetValue(key).ChangeState(state);
    }

    public void ChangeGridState(Vector2Int key, GridState state)
    {
        GetValue(key).ChangeState(state);
    }

    public void ChangeRegion(string key, GridRegion region)
    {
        GetValue(key).SetRegion(region);
    }

    public void ChangeRegion(Vector2Int key, GridRegion region)
    {
        GetValue(key).SetRegion(region);
    }

    public bool TryGetValue(string key, out ShogiGrid result)
    {
        return gridDictionary.TryGetValueFirstKey(key, out result);
    }

    public bool TryGetValue(Vector2Int key, out ShogiGrid result)
    {
       return gridDictionary.TryGetValueSecondKey(key, out result);
    }

    public ShogiGrid GetValue(string key)
    {
        return TryGetValue(key, out ShogiGrid result) ? result : null;
    }

    public ShogiGrid GetValue(Vector2Int key)
    {
        return TryGetValue(key, out ShogiGrid result) ? result : null;
    }

    public bool ContainsKey(string key)
    {
        return gridDictionary.ContainsFirstKey(key);
    }

    public bool ContainsKey(Vector2Int key)
    {
        return gridDictionary.ContainsSecondKey(key);
    }

    public List<ShogiGrid> GetAllValues()
    {
        return gridDictionary.GetAllValues();
    }

    public Vector2Int GetPairKey(string key)
    {
        if (gridDictionary.ContainsFirstKey(key))
        {
            return gridDictionary.GetPairKeyWithFirst(key);
        }

        return new Vector2Int(-1, -1);
    }

    public string GetPairKey(Vector2Int key)
    {
        if (gridDictionary.ContainsSecondKey(key))
        {
            return gridDictionary.GetPairKeyWithSecond(key);
        }

        return string.Empty;
    }

    public GridState GetState(string key)
    {
        return GetValue(key).GetGridState();
    }

    public GridState GetState(Vector2Int key)
    {
        return GetValue(key).GetGridState();
    }
    #endregion

    public void ResetGridState(Owner owner)
    {
        var grids = gridDictionary.GetAllValues();

        foreach (var grid in grids)
        {
            switch (grid.GetGridState())
            {
                case GridState.Movable:
                case GridState.MovableWithEv:
                    grid.ChangeState(GridState.Empty);
                    break;

                case GridState.Blocked:
                    grid.ChangeState(
                        owner == Owner.Player ? GridState.Friend : GridState.Enemy
                        );
                    break;

                case GridState.Attackable:
                case GridState.AttackableWithEv:
                    grid.ChangeState(
                        owner == Owner.Player ? GridState.Enemy : GridState.Friend
                        );
                    break;

                default:
                    break;
            }
        }
    }
}
