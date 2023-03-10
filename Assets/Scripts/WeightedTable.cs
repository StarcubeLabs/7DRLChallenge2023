using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

[Serializable]
public class WeightedEntry<T> where T : class
{
    public int Weight = 1;
    public T Entry;
}

public class WeightedTable<T> where T : class
{
    private SortedDictionary<int, KeyValuePair<int, T>> weightedTable;
    public int TotalWeight = 0;
    public void ConstructWeightedTable(List<WeightedEntry<T>> entries)
    {
        weightedTable = new SortedDictionary<int, KeyValuePair<int, T>>();
        
        foreach (var entry in entries)
        {
            weightedTable.Add(TotalWeight, new KeyValuePair<int, T>(TotalWeight + entry.Weight, entry.Entry));
            TotalWeight += entry.Weight;
        }
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
            if (randomValue >= entry.Key && randomValue < entry.Value.Key)
            {
                return entry.Value.Value;
            }
        }

        return null;
    }

    public override string ToString()
    {
        var toReturn = "Low: High: Value:" + "\n";
        foreach (var entry in weightedTable)
        {
            toReturn += entry.Key + " " + entry.Value.Key + " " + entry.Value.Value;
            toReturn += "\n";
        }

        return toReturn;
    }
    
    public T GetEntryAtIndex(int index)
    {
        return weightedTable.ElementAt(index).Value.Value;
    }
}
