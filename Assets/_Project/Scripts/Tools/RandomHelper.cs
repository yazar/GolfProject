using UnityEngine;
using System.Collections.Generic;

public static class RandomHelper
{
    public static bool CoinFlip()
    {
        return Random.value > 0.5f;
    }

    public static T RandomElement<T>(this T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    public static T RandomElement<T>(this List<T> list)
    {
        return list[Random.Range(0, list.Count)];
    }

    public static T[] RandomElements<T>(this T[] array, int elementCount)
    {
        List<T> pickedElements = new List<T>();
        List<T> currentElements = new List<T>(array);

        for (int i = 0; i < elementCount; i++)
        {
            var randomIndex = Random.Range(0, currentElements.Count);
            pickedElements.Add(currentElements[randomIndex]);
            currentElements.RemoveAt(randomIndex);
        }

        return pickedElements.ToArray();
    }

    public static T[] RandomElements<T>(this List<T> list, int elementCount)
    {
        List<T> pickedElements = new List<T>();
        List<T> currentElements = new List<T>(list.ToArray());

        for (int i = 0; i < elementCount; i++)
        {
            var randomIndex = Random.Range(0, currentElements.Count);
            pickedElements.Add(currentElements[randomIndex]);
            currentElements.RemoveAt(randomIndex);
        }

        return pickedElements.ToArray();
    }
    
    public static T[] ShuffledElements<T>(this T[] array)
    {
        return array.RandomElements(array.Length);
    }

    public static T[] ShuffledElements<T>(this List<T> list)
    {
        return list.RandomElements(list.Count);
    }

    public static void Shuffle<T>(this List<T> list)
    {
        var shuffledElements = list.ShuffledElements(); 
        list.Clear();
        list.AddRange(shuffledElements);
    }
    
    public static string ContentsToString<T>(this T[] array)
    {
        if (array.Length == 0)
        {
            return "Empty!";
        }

        var totalString = array[0].ToString();
        
        if (array.Length > 1)
        {
            for (int i = 1; i < array.Length; i++)
            {
                totalString += ", " + array[i];
            }
        }

        return totalString;
    }
    
    
    public static string ContentsToString<T>(this List<T> list)
    {
        if (list.Count == 0)
        {
            return "Empty!";
        }

        var totalString = list[0].ToString();
        
        if (list.Count > 1)
        {
            for (int i = 1; i < list.Count; i++)
            {
                totalString += ", " + list[i];
            }
        }

        return totalString;
    }
}

public class RandomWeightedElementSelector<T>
{
    readonly Dictionary<T, int> _weights;

    public RandomWeightedElementSelector()
    {
        _weights = new Dictionary<T, int>();
    }
    
    public RandomWeightedElementSelector(ElementWeightPair<T>[] typeWeightPairs)
    {
        _weights = new Dictionary<T, int>();

        foreach (var typeWeightPair in typeWeightPairs)
        {
            Add(typeWeightPair.element, typeWeightPair.weight);
        }
    }

    public void Add(T element, int weight)
    {
        if (weight < 1)
        {
            return;
        }
        
        _weights[element] = weight;
    }

    public void Remove(T element)
    {
        _weights.Remove(element);
    }
    
    public bool isEmpty => _weights.Count == 0;

    public T SelectOne()
    {
        var sortedSpawnRate = Sort(_weights);

        int sum = 0;
        foreach (var spawn in _weights)
        {
            sum += spawn.Value;
        }

        int roll = Random.Range(0, sum);

        T selected = sortedSpawnRate[sortedSpawnRate.Count - 1].Key;
        foreach (var spawn in sortedSpawnRate)
        {
            if (roll < spawn.Value)
            {
                selected = spawn.Key;
                break;
            }
            roll -= spawn.Value;
        }

        return selected;
    }

    List<KeyValuePair<T, int>> Sort(Dictionary<T, int> weights)
    {
        var list = new List<KeyValuePair<T, int>>(weights);

        list.Sort(
            delegate (KeyValuePair<T, int> firstPair,
                     KeyValuePair<T, int> nextPair)
            {
                return firstPair.Value.CompareTo(nextPair.Value);
            }
         );

        return list;
    }
}

[System.Serializable]
public class ElementWeightPair<T>
{
    public T element;
    public int weight;

    public ElementWeightPair(T element, int weight)
    {
        this.element = element;
        this.weight = weight;
    }
}

