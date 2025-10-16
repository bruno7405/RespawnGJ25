using UnityEngine;

public class DialogEventTaskBruno : TaskBruno
{
    [SerializeField] DialogEvent dialogEvent;

    public void Activate(int duration, DialogEvent dialog)
    {
        base.Activate(duration);
        dialogEvent = dialog;
    }

    public override void OnInteract(PlayerInteractor interactor)
    {
        CompleteTask();
    }


    /// <summary>
    /// Stops countdown (for game over), starts dialogue event
    /// </summary>
    protected override void CompleteTask()
    {
        base.CompleteTask();
        DialogEventManager.Instance.SetDialogEvent(dialogEvent);

        StartCoroutine(RemoveTask(5));
    }
}
