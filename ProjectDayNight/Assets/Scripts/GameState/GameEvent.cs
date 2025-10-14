using UnityEngine;

public class GameEvent : MonoBehaviour, IInteractable
{
    [SerializeField] float eventDuration;
    [SerializeField] int dollarPenalty;

    float timeLeft;
    protected bool countdown = true;

    // Not active; completed or failed
    public bool resolved { get; private set; } = false;

    void Start()
    {
        timeLeft = eventDuration;
    }   

    // Countdown event duration, when time runs out before task complete, game over
    void Update()
    {
        if (!countdown) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            resolved = true;
            countdown = false;
            if (!CompanyManager.Instance.SpendMoney(dollarPenalty))
                GameStateManager.Instance.GameOver();
        }
    }

    protected virtual void CompleteTask()
    {
        countdown = false;
        resolved = true;
        // Visuals
        GetComponent<SpriteRenderer>().color = Color.green;
        // Delete object after a few seconds
        Debug.Log("Task Complete!");
    }

    public void OnInteract(PlayerInteractor interactor)
    {
        CompleteTask();
    }
}
