using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WeightedRandomizer
{
    /// <summary>
    /// Selects a random item from a list of weighted values based on their probability weight.
    /// </summary>
    /// <typeparam name="T">The type of item being randomized.</typeparam>
    /// <param name="weightedValues">A list of items with associated weight values.</param>
    /// <returns>A randomly selected item based on weight distribution.</returns>
    
    public static T GetRandomItem<T>(List<WeightedValue<T>> weightedValues)
    {
        float totalWeight = weightedValues.Sum(v => v.Weight);
        float randomValue = Random.Range(0, totalWeight);

        Debug.Log($"Total Weight: {totalWeight}, Random Value: {randomValue}");

        foreach (var entry in weightedValues)
        {
            Debug.Log($"Checking item: {entry.Value} with weight {entry.Weight}");

            if (randomValue < entry.Weight)
            {
                Debug.Log($"Selected item: {entry.Value}");
                return entry.Value;
            }

            randomValue -= entry.Weight;
        }

        Debug.LogWarning("Fallback to last item in the list. Possible issue in weight calculation.");
        return weightedValues.Last().Value; // Fallback to the last item if none were selected
    }
}

/// <summary>
/// Represents an item with an associated weight for probability-based selection.
/// </summary>
/// <typeparam name="T">The type of item.</typeparam>

public struct WeightedValue<T>
{
    public T Value { get; }
    public float Weight { get; }

    public WeightedValue(T value, float weight)
    {
        Value = value;
        Weight = weight;
    }
}
