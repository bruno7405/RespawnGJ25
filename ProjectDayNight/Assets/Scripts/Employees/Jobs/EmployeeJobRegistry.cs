using Random = UnityEngine.Random;
using System;
using System.Linq;
using System.Collections.Generic;

public enum JobType
{
    UsingBathroom,
    ConferenceTable,
    ShelvesCabinets,
    DeskWork,
    Custodial,
    Print,
}

public static class EmployeeJobRegistry
{
    private static readonly Dictionary<JobType, float> jobWeights = new() {
        { JobType.UsingBathroom, 1f },
        { JobType.ConferenceTable, 1f },
        { JobType.ShelvesCabinets, 1f },
        { JobType.DeskWork, 1f },
        { JobType.Custodial, 1f },
        { JobType.Print, 1f },
    };
    public static List<EmployeeJob> Jobs { get; } = new();
    public static List<EmployeeJob> GetAvailableJobs(Role role) => Jobs.Where(job => !job.IsAssigned && job.AllowedRoles.Contains(role)).ToList();
    public static List<EmployeeJob> GetAvailableJobs() => Jobs.Where(job => !job.IsAssigned).ToList();

    public static EmployeeJob TakeRandomJob(Role role)
    {
        JobType jobType;
        List<EmployeeJob> availableOfType;

        var shallowJobWeights = new Dictionary<JobType, float>(jobWeights);
        do
        {
            jobType = PickWeighted(shallowJobWeights);
            shallowJobWeights.Remove(jobType);
            availableOfType = GetAvailableJobs(role).Where(job => job.Type == jobType).ToList();
        } while (availableOfType.Count == 0 && shallowJobWeights.Count > 0);

        if (availableOfType.Count == 0) return null;

        var randomJob = availableOfType[Random.Range(0, availableOfType.Count)];
        randomJob?.Assign();
        return randomJob;
    }

    public static JobType PickWeighted(Dictionary<JobType, float> jobWeights)
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

    public static void RegisterJob(EmployeeJob job)
    {
        if (!Jobs.Contains(job))
            Jobs.Add(job);
    }
}