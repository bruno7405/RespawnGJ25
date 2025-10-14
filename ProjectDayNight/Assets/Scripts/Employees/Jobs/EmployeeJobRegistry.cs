using System.Collections.Generic;
using System.Linq;

public static class EmployeeJobRegistry
{
    public static List<EmployeeJob> Jobs { get; } = new();
    public static List<EmployeeJob> GetAvailableJobs(Role role) => Jobs.Where(job => !job.IsAssigned && job.AllowedRoles.Contains(role)).ToList();
    public static List<EmployeeJob> GetAvailableJobs() => Jobs.Where(job => !job.IsAssigned).ToList();
    
    public static void RegisterJob(EmployeeJob job)
    {
        if (!Jobs.Contains(job))
            Jobs.Add(job);
    }
}