using Unity.VisualScripting;
using UnityEngine;

public class GameEvent : MonoBehaviour, IInteractable
{
    [Range(10, 120)]
    [SerializeField] private int eventDuration;
    [SerializeField] private string eventName;
    public string Name => eventName;

    float timeLeft;
    protected bool countdown = true;

    // Not active; completed or failed
    public bool resolved { get; protected set; } = false;

    void Awake()
    {
        timeLeft = eventDuration;
        countdown = false;
    }

    void Start() { OnStart(); }

    protected virtual void OnStart()
    {
        
    }

    // Countdown event duration, when time runs out before task complete, game over
    void Update()
    {
        if (!countdown) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            FailTask();
        }
    }

    public void StartCountdown()
    {
        countdown = true;
    }
    
    protected virtual void FailTask()
    {
        countdown = false;
        resolved = true;
        // TODO Increment mistake bar
    }

    protected virtual void CompleteTask()
    {
        countdown = false;
        resolved = true;
        Debug.Log("Task Complete!");
    }

    public void OnInteract(PlayerInteractor interactor)
    {
        CompleteTask();
    }
}
