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
        if (!taskActive) return;
        DialogEventManager.Instance.SetDialogEvent(dialogEvent, CompleteTask, FailTask);
    }
    protected override void FailTask()
    {
        base.FailTask();
        GameStateManager.Instance.IncrementMistakes();
    }


}
