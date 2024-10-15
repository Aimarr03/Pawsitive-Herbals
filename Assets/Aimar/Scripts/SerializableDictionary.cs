using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializableDictionary<TKey, TValue> : ISerializationCallbackReceiver
{
    [SerializeField]
    private List<SerializableKeyValuePair<TKey, TValue>> keyValuePairList = new List<SerializableKeyValuePair<TKey, TValue>>();

    private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

    public void OnBeforeSerialize()
    {
        keyValuePairList.Clear();
        foreach (var kvp in dictionary)
        {
            keyValuePairList.Add(new SerializableKeyValuePair<TKey, TValue>(kvp.Key, kvp.Value));
        }
    }

    public void OnAfterDeserialize()
    {
        dictionary = new Dictionary<TKey, TValue>();
        foreach (var kvp in keyValuePairList)
        {
            dictionary[kvp.Key] = kvp.Value;
        }
    }

    public void Add(TKey key, TValue value)
    {
        dictionary[key] = value;
    }

    public bool ContainsKey(TKey key)
    {
        return dictionary.ContainsKey(key);
    }

    public bool ContainsValue(TValue value)
    {
        return dictionary.ContainsValue(value);
    }
    public bool Remove(TKey key)
    {
        return dictionary.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get { return dictionary[key]; }
        set { dictionary[key] = value; }
    }
    public Dictionary<TKey, TValue>.KeyCollection Keys => dictionary.Keys;
    public Dictionary<TKey, TValue>.ValueCollection Values => dictionary.Values;
}
