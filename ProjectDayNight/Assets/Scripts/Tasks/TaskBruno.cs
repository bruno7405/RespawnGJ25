using System;
using System.Collections;
using UnityEngine;

public class TaskBruno : MonoBehaviour, IInteractable
{
    [SerializeField] private string eventName;

    float timeLeft;
    bool countdown = false;

    private Color normalColor;
    private bool taskActive;

    private void Awake()
    {
        normalColor = GetComponent<SpriteRenderer>().color;
        gameObject.layer = 9; // move to not interactable layer
    }

    void Update()
    {
        if (!countdown) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            FailTask();
        }
    }

    /// <summary>
    /// Activate task and start counting down
    /// </summary>
    /// <param name="duration"> seconds player has to complete task</param>
    public void Activate(int duration)
    {
        timeLeft = duration;
        countdown = true;
        taskActive = true;
        gameObject.layer = 6; // move to interactable layer

        // Visuals
        GetComponent<SpriteRenderer>().color = Color.blue;
        // SFX
    }

    protected virtual void FailTask()
    {
        countdown = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(RemoveTask(3));

        // TODO Increment mistakes
    }

    protected virtual void CompleteTask()
    {
        if (!taskActive) return;

        countdown = false;
       
        GetComponent<SpriteRenderer>().color = Color.green;
        StartCoroutine(RemoveTask(3));
    }

    public void OnInteract(PlayerInteractor interactor)
    {
        CompleteTask();
    }
    protected IEnumerator RemoveTask(float duration)
    {
        taskActive = false;
        gameObject.layer = 9; // move to not interactable layer
        PlayerInteractor.Instance.RemoveInteractable(transform); // manually remove from interactables list (b/c OnTriggerExit doesnt see this task anymore)
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = normalColor;
        BossTaskManagerBruno.Instance.RemoveTask(this);
    }
}
