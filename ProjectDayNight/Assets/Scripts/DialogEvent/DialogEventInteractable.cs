using UnityEngine;

public class DialogEventInteractable : GameEvent
{
    [SerializeField] DialogEvent dialogEvent;

    /// <summary>
    /// Stops countdown (for game over), starts dialogue event
    /// </summary>
    protected override void CompleteTask()
    {
        countdown = false;
        // Visuals
        GetComponent<SpriteRenderer>().color = Color.green;
        DialogEventManager.Instance.SetDialogEvent(dialogEvent);
    }
}
