using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Provides functionality to randomly select an item from a list based on assigned weights.
/// Higher weight values increase the probability of selection.
/// </summary>

public static class WeightedRandomizer
{
    // Selects a random item from a list of weighted values based on their probability weight.
    // Uses a weighted probability approach where items with higher weight have a greater chance of being selected.
    public static T GetRandomItem<T>(List<WeightedValue<T>> weightedValues)
    {
        float totalWeight = weightedValues.Sum(v => v.Weight); // Calculate the total sum of all weights in the list
        float randomValue = Random.Range(0, totalWeight); // Generate a random number between 0 and the total weight

        // Iterate through the list and subtract each item's weight from the random value
        foreach (var entry in weightedValues)
        {
            // If the random value falls within the current weight, select this item
            if (randomValue < entry.Weight)
            {
                return entry.Value;
            }
            // Reduce the random value by the current item's weight and continue checking
            randomValue -= entry.Weight;
        }
        // If no item is selected due to precision errors, return the last item as a fallback
        return weightedValues.Last().Value;
    }
}

// Represents an item with an associated weight for probability-based selection.
// Commonly used in loot tables, random item drops, and procedural generation.
public struct WeightedValue<T>
{
    public T Value { get; } // The item associated with this weighted value.
    public float Weight { get; } // The weight representing the likelihood of this item being selected.
                                 // A higher weight increases selection probability.

    // Constructor to initialize a weighted value.
    public WeightedValue(T value, float weight)
    {
        Value = value;
        Weight = weight;
    }
}
