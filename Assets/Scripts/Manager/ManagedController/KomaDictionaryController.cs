using UnityEngine;

/// <summary>
/// A helper class that allows DualKeyDictionary to be treated like a Dictionary.
/// </summary>
public class KomaDictionaryController
{
    private DualKeyDictionary<string, Vector2Int, Koma> komaDictionary;
    private Index _index;

    public KomaDictionaryController(Index index)
    {
        komaDictionary = new DualKeyDictionary<string, Vector2Int, Koma>();
        _index = index;
    }

    #region Basement Methods
    public void Add(Vector2Int firstKey, Vector2Int secondKey, Koma koma)
    {
        Add(_index.ParseShogiPosition(firstKey), secondKey, koma);
    }

    public void Add(string firstKey, Vector2Int secondKey, Koma koma)
    {
        komaDictionary.Add(firstKey, secondKey, koma);
    }

    public bool TryGetValue(string key, out Koma result)
    {
        return komaDictionary.TryGetValueFirstKey(key, out result);
    }

    public bool TryGetValue(Vector2Int key, out Koma result)
    {
        return komaDictionary.TryGetValueSecondKey(key, out result);
    }

    public Koma GetValue(string key)
    {
        return TryGetValue(key, out Koma result) ? result : null;
    }

    public Koma GetValue(Vector2Int key)
    {
        return TryGetValue(key, out Koma result) ? result : null;
    }

    public bool ContainsKey(string key)
    {
        return komaDictionary.ContainsFirstKey(key);
    }

    public bool ContainsKey(Vector2Int key)
    {
        return komaDictionary.ContainsSecondKey(key);
    }

    public Vector2Int GetPairKey(string key)
    {
        if (komaDictionary.ContainsFirstKey(key))
        {
            return komaDictionary.GetPairKeyWithFirst(key);
        }

        return new Vector2Int(-1, -1);
    }

    public string GetPairKey(Vector2Int key)
    {
        if(komaDictionary.ContainsSecondKey(key))
        {
            return komaDictionary.GetPairKeyWithSecond(key);
        }

        return string.Empty;
    }

    public void Remove(string key)
    {
        komaDictionary.Remove(key);
    }

    public void Remove(Vector2Int key)
    {
        komaDictionary.Remove(key);
    }
    #endregion

    public void UpdateKomaDictionary(string pre, string post, Vector2Int index)
    {
        if(TryGetValue(pre, out var koma))
        {
            komaDictionary.Remove(pre);
            komaDictionary.Add(post, index, koma);
        }
    }
}