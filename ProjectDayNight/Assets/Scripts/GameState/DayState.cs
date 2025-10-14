using UnityEngine;

public class DayState : State
{
    float duration;
    float timeElapsed;

    [SerializeField] State nightState;
    [SerializeField] TimeUI timeUI;
    public override void OnStart()
    {
        // Visuals
        // Show day number
        // Invoke events in random time?
        Debug.Log("day state");
        ((GameStateManager)stateMachine).InvokeNewDay();

        // UI Popup
        InformationPopupUI.Instance.DisplayText("Keep employees on task", true);

        // Play Music
        AudioManager.Instance.PlayBackgroundMusic("DaySong");

        // Lights
        VisualsManager.Instance.LightsOn();

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        timeUI.SetTimeElasped(timeElapsed, true);
        
        if (timeElapsed >= duration)
        {
            stateMachine.SetNewState(nightState);
        }
    }
    public override void OnExit()
    {
        return;
    }

    public void SetDuration(float d)
    {
        duration = d;
    }
}
