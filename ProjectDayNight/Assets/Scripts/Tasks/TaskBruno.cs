using System;
using System.Collections;
using UnityEngine;

public class TaskBruno : MonoBehaviour, IInteractable
{
    [SerializeField] private string eventName;
    public string Name => eventName;

    float timeLeft;
    bool countdown = false;

    private Color normalColor;
    private bool taskActive;
    [SerializeField] int moneyReward = 100;
    InformationPopupUI informationPopupUI;

    private void Awake()
    {
        normalColor = GetComponent<SpriteRenderer>().color;
        gameObject.layer = 9; // move to not interactable layer
    }

    void Start()
    {
        informationPopupUI = InformationPopupUI.Instance;
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

        informationPopupUI.DisplayText(eventName, true);
        MinimapManager.Instance.RegisterTask(this);
    }

    protected virtual void FailTask()
    {
        countdown = false;
        GetComponent<SpriteRenderer>().color = Color.red;
        StartCoroutine(RemoveTask(1));
        informationPopupUI.DisplayText($"{eventName} Failed!", false, 2f);
        MinimapManager.Instance.UnregisterTask(this);

        // TODO Increment mistakes
    }

    protected virtual void CompleteTask()
    {
        if (!taskActive) return;
       
        GetComponent<SpriteRenderer>().color = Color.green;
        StartCoroutine(RemoveTask(1));
        CompanyManager.Instance.AddMoney(moneyReward);
        informationPopupUI.DisplayText($"{eventName} Completed! Earned +${moneyReward}", true, 2f);
        MinimapManager.Instance.UnregisterTask(this);
    }

    public virtual void OnInteract(PlayerInteractor interactor)
    {
        MinigameManager.Instance.StartAimTrainer(CompleteTask, FailTask);
    }
    protected IEnumerator RemoveTask(float duration)
    {
        taskActive = false;
        countdown = false;
        gameObject.layer = 9; // move to not interactable layer
        PlayerInteractor.Instance.RemoveInteractable(transform); // manually remove from interactables list (b/c OnTriggerExit doesnt see this task anymore)
        yield return new WaitForSeconds(duration);
        GetComponent<SpriteRenderer>().color = normalColor;
        BossTaskManagerBruno.Instance.RemoveTask(this);
    }

    private void Deactivate()
    {
        StartCoroutine(RemoveTask(0));
    }

    private void OnEnable()
    {
        GameStateManager.Instance.NightStart += Deactivate;
    }

    private void OnDisable()
    {
        GameStateManager.Instance.NightStart -= Deactivate;
    }
}
