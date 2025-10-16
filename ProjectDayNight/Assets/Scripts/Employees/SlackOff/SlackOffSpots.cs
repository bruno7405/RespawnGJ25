using Random = UnityEngine.Random;
using System;
using System.Linq;
using System.Collections.Generic;
using static SlackOffSpotType;

public enum SlackOffSpotType
{
    UsingBathroom,
    ConferenceTable,
    ShelvesCabinets,
    DeskWork,
    Custodial,
    Print,
}

public static class SlackOffSpots
{
    private static readonly Dictionary<SlackOffSpotType, float> slackOffWeights = new() {
        { UsingBathroom, 1f },
        { ConferenceTable, 1f },
        { ShelvesCabinets, 1f },
        { DeskWork, 1f },
        { Custodial, 1f },
        { Print, 1f },
    };
    public static List<SlackOffSpot> Spots { get; } = new();
    public static List<SlackOffSpot> GetAvailableSpots() => Spots.Where(job => !job.IsAssigned).ToList();

    public static SlackOffSpot TakeRandomSpot()
    {
        SlackOffSpotType SlackOffSpotType;
        List<SlackOffSpot> availableOfType;

        var shallowJobWeights = new Dictionary<SlackOffSpotType, float>(slackOffWeights);
        do
        {
            SlackOffSpotType = PickWeighted(shallowJobWeights);
            shallowJobWeights.Remove(SlackOffSpotType);
            availableOfType = GetAvailableSpots().Where(job => job.Type == SlackOffSpotType).ToList();
        } while (availableOfType.Count == 0 && shallowJobWeights.Count > 0);

        if (availableOfType.Count == 0) return null;

        var randomJob = availableOfType[Random.Range(0, availableOfType.Count)];
        randomJob?.Assign();
        return randomJob;
    }

    public static SlackOffSpotType PickWeighted(Dictionary<SlackOffSpotType, float> jobWeights)
    {
        float totalWeight = jobWeights.Values.Sum();
        float randomValue = Random.value * totalWeight;
        float cumulative = 0f;

        foreach (var kvp in jobWeights)
        {
            cumulative += kvp.Value;
            if (randomValue <= cumulative) return kvp.Key;
        }

        throw new Exception("Should never reach here in Weighted Job Select");
    }

    public static void Register(SlackOffSpot job)
    {
        if (!Spots.Contains(job))
            Spots.Add(job);
    }
}