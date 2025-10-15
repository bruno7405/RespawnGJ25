using System.Collections.Generic;
using UnityEngine;

public class BossTaskManagerBruno : MonoBehaviour
{
    private static BossTaskManagerBruno instance;
    public static BossTaskManagerBruno Instance => instance;

    [SerializeField] int timeForNormalTasks;
    [SerializeField] int timeForDialogTasks;

    [SerializeField] List<TaskBruno> tasks = new List<TaskBruno>();
    [SerializeField] DialogEventTaskBruno phoneTask;

    List<TaskBruno> currentTasks = new List<TaskBruno>(); // used to store all active tasks
    int maxTasks = 3;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        instance = this;
    }

    public void ActivateRandomTask()
    {
        if (currentTasks.Count >= 3) return;

        int rand = Random.Range(0, tasks.Count);
        TaskBruno task = tasks[rand];

        if (currentTasks.Contains(task)) ActivateRandomTask(); // the num of active tasks MUST NOT EQUAL tasks.Count, on function call

        task.Activate(timeForNormalTasks);
        currentTasks.Add(task);
    }

    public void ActivateDialogTask(DialogEvent dialog)
    {
        if (currentTasks.Count >= 3) return;


        phoneTask.Activate(timeForDialogTasks, dialog);
        currentTasks.Add((TaskBruno)phoneTask);
    }

    public void RemoveTask(TaskBruno task)
    {
        currentTasks.Remove(task);
    }
}
