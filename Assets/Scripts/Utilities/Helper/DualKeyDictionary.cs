using System.Collections.Generic;

/// <summary>
/// A custom Dictionary class that allows two Keys to be set for one Value.
/// </summary>
/// <typeparam name="TKey1"></typeparam>
/// <typeparam name="TKey2"></typeparam>
/// <typeparam name="TValue"></typeparam>
[System.Serializable]
public class DualKeyDictionary<TKey1, TKey2, TValue>
{
    private SerializedDictionary<TKey1 , TValue> _dictionary = new SerializedDictionary<TKey1 , TValue>();
    private SerializedDictionary<TKey2, TKey1> _key1Map = new SerializedDictionary<TKey2, TKey1>();
    private SerializedDictionary<TKey1, TKey2> _key2Map = new SerializedDictionary<TKey1, TKey2>();


    public void Add(TKey1 key1, TKey2 key2, TValue value)
    {
        _dictionary.Add(key1, value);
        _key1Map.Add(key2, key1);
        _key2Map.Add(key1, key2);
    }

    public void Remove(TKey1 ke1)
    {
        _key1Map.Remove(_key2Map[ke1]);
        _key2Map.Remove(ke1);
        _dictionary.Remove(ke1);
    }

    public void Remove(TKey2 key2)
    {
        _key2Map.Remove(_key1Map[key2]);
        _dictionary.Remove(_key1Map[key2]);
        _key1Map.Remove(key2);
    }

    public bool TryAdd(TKey1 key1, TKey2 key2, TValue value)
    {
        if(!_dictionary.ContainsKey(key1) && !_key1Map.ContainsKey(key2) && !_key2Map.ContainsKey(key1))
        {
            _dictionary.Add(key1, value);
            _key1Map.Add(key2, key1);
            _key2Map.Add(key1, key2);

            return true;
        }

        return false;
    }

    public List<TValue> GetAllValues()
    {
        List<TValue> list = new List<TValue>();

        foreach(var value in _dictionary.Values)
        {
            list.Add(value);
        }

        return list;
    }

    public TValue GetValueByFirstKey(TKey1 key1)
    {
        return _dictionary[key1];
    }

    public TValue GetValueBySecondKey(TKey2 key2)
    {
        return _dictionary[_key1Map[key2]];
    }

    public bool ContainsFirstKey(TKey1 key1)
    {
        return _dictionary.ContainsKey(key1);
    }

    public bool ContainsSecondKey(TKey2 key2)
    {
        if(_key1Map.TryGetValue(key2, out TKey1 key))
        {
            return _dictionary.ContainsKey(key);
        }

        return false;
    }

    public bool TryGetValueFirstKey(TKey1 key1, out TValue value)
    {
        return _dictionary.TryGetValue(key1, out value);
    }

    public bool TryGetValueSecondKey(TKey2 key2, out TValue value)
    {
        TKey1 key = _key1Map.ContainsKey(key2)? _key1Map[key2] : default;

        return _dictionary.TryGetValue(key, out value);
    }

    public TKey2 GetPairKeyWithFirst(TKey1 key1)
    {
        return _key2Map[key1];
    }

    public TKey1 GetPairKeyWithSecond(TKey2 key2)
    {
        return _key1Map[key2];
    }
}