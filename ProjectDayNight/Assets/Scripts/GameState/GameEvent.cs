using TMPro;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    [SerializeField] float eventDuration;
    float timeLeft;
    bool countdown = true;

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
            GameStateManager.instance.GameOver();
            countdown = false;
        }
    }

    void CompleteTask()
    {
        countdown = false;
        // Visuals
        // Delete object after a few seconds
    }
}
