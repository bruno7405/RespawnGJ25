using Random = UnityEngine.Random;
using System;
using System.Linq;
using System.Collections.Generic;
using static SlackOffRoom;

public enum SlackOffRoom
{
    Lounge,
    TopKitchen,
    BottomKitchen,
    Couch,
    SmallTable,
    WaterCooler,
    ChalkBoard,
}

public static class SlackOffSpots
{
    private static readonly Dictionary<SlackOffRoom, float> slackOffWeights = new() {
        { Lounge, 1f },
        { TopKitchen, 1f },
        { BottomKitchen, 1f },
        { Couch, 1f },
        { SmallTable, 1f },
        { WaterCooler, 1f },
        { ChalkBoard, 1f },
    };
    public static List<SlackOffSpot> Spots { get; } = new();
    public static List<SlackOffSpot> GetAvailableSpots(SlackOffRoom room) => Spots.Where(job => !job.IsAssigned && job.Room == room).ToList();
    public static List<SlackOffSpot> GetAvailableSpots() => Spots.Where(job => !job.IsAssigned).ToList();

    public static SlackOffSpot TakeRandomSpot(bool takeAssigned = true)
    {
        SlackOffRoom slackOffRoom;
        List<SlackOffSpot> availableOfType;

        var shallowJobWeights = new Dictionary<SlackOffRoom, float>(slackOffWeights);
        do
        {
            slackOffRoom = PickWeighted(shallowJobWeights);
            shallowJobWeights.Remove(slackOffRoom);
            availableOfType = takeAssigned ?
                Spots.Where(job => job.Room == slackOffRoom).ToList() :
                GetAvailableSpots(slackOffRoom).Where(job => job.Room == slackOffRoom).ToList();
        } while (availableOfType.Count == 0 && shallowJobWeights.Count > 0);

        if (availableOfType.Count == 0) return null;

        var randomJob = availableOfType[Random.Range(0, availableOfType.Count)];
        randomJob?.Assign();
        return randomJob;
    }

    public static SlackOffRoom PickWeighted(Dictionary<SlackOffRoom, float> jobWeights)
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