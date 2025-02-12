using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WeightedRandomizer
{
    public static T GetRandomItem<T>(List<WeightedValue<T>> weightedValues)
    {
        float totalWeight = weightedValues.Sum(v => v.Weight);
        float randomValue = Random.Range(0, totalWeight);

        foreach (var entry in weightedValues)
        {
            if (randomValue < entry.Weight)
                return entry.Value;

            randomValue -= entry.Weight;
        }

        return weightedValues.Last().Value;
    }
}

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
