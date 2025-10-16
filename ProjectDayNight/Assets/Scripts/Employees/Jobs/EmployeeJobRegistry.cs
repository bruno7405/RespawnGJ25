using Random = UnityEngine.Random;
using System;
using System.Linq;
using System.Collections.Generic;
using static JobType;

public enum JobType
{
    WaterPlant,
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
        { WaterPlant, 1f },
        { UsingBathroom, 0.8f },
        { ConferenceTable, 0.3f },
        { ShelvesCabinets, 1f },
        { DeskWork, 1f },
        { Custodial, 1f },
        { Print, 0.3f },
    };
    public static List<EmployeeJob> Jobs { get; } = new();
    // public static List<EmployeeJob> GetAvailableJobs() => Jobs.Where(job => !job.IsAssigned).ToList();
    // public static List<EmployeeJob> GetAvailableJobs(Role role) => Jobs.Where(job => !job.IsAssigned && job.AllowedRoles.Contains(role)).ToList();
    // public static List<EmployeeJob> GetAvailableJobs(JobType type) => Jobs.Where(job => !job.IsAssigned && job.Type == type).ToList();
    public static List<EmployeeJob> GetAvailableJobs(Role role, JobType type) => Jobs.Where(job => !job.IsAssigned && job.AllowedRoles.Contains(role) && job.Type == type).ToList();

    public static EmployeeJob TakeRandomJob(Role role)
    {
        JobType jobType;
        List<EmployeeJob> availableOfType;

        var shallowJobWeights = new Dictionary<JobType, float>(jobWeights);
        do
        {
            jobType = PickWeighted(shallowJobWeights);
            shallowJobWeights.Remove(jobType);
            availableOfType = GetAvailableJobs(role, jobType).ToList();
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