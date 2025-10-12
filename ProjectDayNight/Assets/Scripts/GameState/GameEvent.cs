using TMPro;
using UnityEngine;

public class GameEvent : MonoBehaviour, IInteractable
{
    [SerializeField] float eventDuration;
    float timeLeft;
    protected bool countdown = true;

    void Start()
    {
        timeLeft = eventDuration;
    }

    // Countdown event duration, when time runs out before task complete, game over
    void Update()
    {
        if (!countdown) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            GameStateManager.Instance.GameOver();
            countdown = false;
        }
    }

    protected virtual void CompleteTask()
    {
        countdown = false;
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
