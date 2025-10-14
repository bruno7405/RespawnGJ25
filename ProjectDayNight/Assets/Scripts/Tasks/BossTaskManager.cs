using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class BossTaskManager : MonoBehaviour
{
    private static BossTaskManager instance;
    public static BossTaskManager Instance => instance;
    [SerializeField] private List<GameEvent> allGameEvents;
    [SerializeField] private int maxActiveTasks = 3;
    [SerializeField] private int minTaskQueueTime;
    [SerializeField] private int maxTaskQueueTime;
    private float timeSinceTask;

    private List<GameEvent> standardTasks;
    private IEnumerator<GameEvent> standardTaskEnumerator;
    private List<DialogEventInteractable> dialogEvents;
    private IEnumerator<DialogEventInteractable> dialogEventEnumerator;

    private Queue<GameEvent> queuedEvents;
    private float nextQueueTime;

    public List<GameEvent> ActiveTasks { get; private set; }
    public int StandardTasksToday => Mathf.Min(GameStateManager.Instance.CurrentDay + 1, 4);
    public int DialogTasksToday => Mathf.Min(GameStateManager.Instance.CurrentDay, 3);
    // 3 on Day 1, 5 on Day 2, 7 on Day 3
    public int TotalTasksToday => StandardTasksToday + DialogTasksToday;

    void QueueTasks()
    {
        List<GameEvent> selectedEvents = new(new GameEvent[TotalTasksToday]);
        for (int i = 0; i < StandardTasksToday; i++)
        {
            if (!standardTaskEnumerator.MoveNext()) throw new InvalidOperationException("Not enough standard tasks.");
            var s = standardTaskEnumerator.Current;
            selectedEvents.Add(s);
        }
        for (int i = 0; i < DialogTasksToday; i++)
        {
            if (!dialogEventEnumerator.MoveNext()) throw new InvalidOperationException("Not enough dialog tasks.");
            var d = dialogEventEnumerator.Current;
            selectedEvents.Add(d);
        }
        queuedEvents = new(selectedEvents.OrderBy(_ => Random.value));
        timeSinceTask = 0;
        nextQueueTime = 10;
    }

    void Start()
    {
        GameStateManager.Instance.DayStart += QueueTasks;
        dialogEvents = allGameEvents.OfType<DialogEventInteractable>().OrderBy(_ => Random.value).ToList();
        standardTasks = allGameEvents.Except(dialogEvents).OrderBy(_ => Random.value).ToList();
        dialogEventEnumerator = dialogEvents.GetEnumerator();
        standardTaskEnumerator = standardTasks.GetEnumerator();
        timeSinceTask = 0;
    }

    void Update()
    {
        if (GameStateManager.Instance.currentState != GameStateManager.Instance.DayState) return;

        ActiveTasks.RemoveAll(task => task == null || task.resolved);

        timeSinceTask += Time.deltaTime;
        if (ActiveTasks.Count < maxActiveTasks && queuedEvents.Count > 0 && timeSinceTask >= nextQueueTime)
        {
            ActiveTasks.Add(queuedEvents.Dequeue());
            timeSinceTask = 0;
            nextQueueTime = Random.Range((float)minTaskQueueTime, maxTaskQueueTime);
        }
    }
    private void Awake()
    {
        instance = this;
        ActiveTasks = new List<GameEvent>();
    }
}