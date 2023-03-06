using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]
public class WeightedEntry<T> where T : class
{
    public int Weight;
    public T Entry;
}

public class WeightedTable<T> where T : class
{
    private SortedDictionary<KeyValuePair<int, int>, T> weightedTable;
    public int TotalWeight = 0;
    public void ConstructWeightedTable(List<WeightedEntry<T>> entries)
    {
        weightedTable = new SortedDictionary<KeyValuePair<int, int>, T>();
        
        foreach (var entry in entries)
        {
            weightedTable.Add(new KeyValuePair<int, int>(TotalWeight,TotalWeight+entry.Weight),entry.Entry);
            TotalWeight += entry.Weight;
        }
        Debug.Log(this.ToString());
    }

    public T GetRandomEntry()
    {
        int randomValue = Random.Range(0, TotalWeight);
        return GetWeightedEntry(randomValue);
    }
    
    public T GetWeightedEntry(int randomValue)
    {
        foreach (var entry in weightedTable)
        {
            if (randomValue >= entry.Key.Key && randomValue <= entry.Key.Value)
            {
                return entry.Value;
            }
        }

        return null;
    }

    public string ToString()
    {
        var toReturn = "Low: High: Value:" + "\n";
        foreach (var entry in weightedTable)
        {
            toReturn += entry.Key.Key + " " + entry.Key.Value + " " + entry.Value.ToString();
            toReturn += "\n";
        }

        return toReturn;
    }
    
    public T GetEntryAtIndex(int index)
    {
        return weightedTable.ElementAt(index).Value;
    }
}
