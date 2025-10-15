using UnityEngine;

public class DialogEventInteractable : GameEvent
{
    [SerializeField] DialogEvent dialogEvent;

    /// <summary>
    /// Stops countdown (for game over), starts dialogue event
    /// </summary>
    protected override void CompleteTask()
    {
        base.CompleteTask();
        DialogEventManager.Instance.SetDialogEvent(dialogEvent);
    }
}
