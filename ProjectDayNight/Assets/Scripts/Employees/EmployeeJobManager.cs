using System;
using UnityEngine;
using Random = UnityEngine.Random;

public struct EmployeeJob
{
    public Vector2Int location;
    public float duration;
}

public class EmployeeJobManager
{
    private static readonly EmployeeJob[] employeeJobs =
    {
        new() { location = new Vector2Int(5, 5), duration = 5f },
        new() { location = new Vector2Int(10, 5), duration = 3f },
        new() { location = new Vector2Int(6, 2), duration = 4f },
        new() { location = new Vector2Int(5, 9), duration = 2f },
    };
    private static readonly EmployeeJob[] internJobs =
    {
        new() { location = new Vector2Int(4, 3), duration = 2f },
        new() { location = new Vector2Int(1, 8), duration = 1.5f },
        new() { location = new Vector2Int(9, 1), duration = 2.5f },
    };
    private static readonly EmployeeJob[] managerJobs =
    {
    };

    private readonly EmployeeJob[] jobs;
    private int previousJob;

    public EmployeeJobManager(Role role)
    {
        jobs = role == Role.Intern ? internJobs
            : role == Role.Employee ? employeeJobs
            : managerJobs;
        previousJob = -1;
    }

    public EmployeeJob NewJob(bool allowRepeats = false)
    {
        if (jobs.Length == 0) throw new Exception("No jobs available for this role");
        if (jobs.Length == 1 && !allowRepeats) throw new ArgumentException("There is only one job, must allow repeats");

        int newJob = Random.Range(0, jobs.Length);
        if (!allowRepeats)
            while (newJob == previousJob) newJob = Random.Range(0, jobs.Length);

        previousJob = newJob;
        return jobs[newJob];
    }

    public static Vector2Int RandomHidePoint()
    {
        int x, y;
        do
        {
            x = Random.Range(0, GridData.Instance.Width);
            y = Random.Range(0, GridData.Instance.Height);
        } while (!GridData.Instance.IsWalkable(x, y));
        return new(x, y);
    }
}