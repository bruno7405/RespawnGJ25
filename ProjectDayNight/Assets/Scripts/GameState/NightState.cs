using System.Collections;
using UnityEngine;

public class NightState : State
{
    float duration;
    float timeElapsed;

    [SerializeField] State endState;
    [SerializeField] TimeUI timeUI;
    [SerializeField] BlackScreenUI blackScreenUI;

    public override void OnStart()
    {
        Debug.Log("night state");
        ((GameStateManager)stateMachine).InvokeNightStart();
        
        StartCoroutine(DayToNightTransition());

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        timeUI.SetTimeElasped(timeElapsed, false);

        if (timeElapsed >= duration)
        {
            stateMachine.SetNewState(endState);
        }
    }

    public void SetDuration(float d)
    {
        duration = d;
    }

    IEnumerator DayToNightTransition()
    {
        // Lights off
        blackScreenUI.Black();
        VisualsManager.Instance?.LightsOff();
        // Lights off sound effect

        yield return new WaitForSeconds(2f);

        AudioManager.Instance.PlayBackgroundMusic("NightSong");
        blackScreenUI.FadeOut();

        // UI Popup
        InformationPopupUI.Instance.DisplayText("EMPLOYEES ARE TRYING TO ESCAPE", false);
    }

    public override void OnExit()
    {
        return;
    }
}
