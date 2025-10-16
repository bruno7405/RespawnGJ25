using System;
using System.Collections;
using UnityEngine;

public class NightState : State
{
    //float duration;
    float timeElapsed;

    [SerializeField] State endState;
    [SerializeField] TimeUI timeUI;
    [SerializeField] BlackScreenUI blackScreenUI;

    // Singleton References
    AudioManager audioManager;
    VisualsManager visualsManager;
    InformationPopupUI informationPopupUI;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        visualsManager = VisualsManager.Instance;
        informationPopupUI = InformationPopupUI.Instance;
    }

    public override void OnStart()
    {
        Debug.Log("night state");
        try
        {
            ((GameStateManager)stateMachine).InvokeNightStart();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
        
        
        StartCoroutine(DayToNightTransition());

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        timeElapsed += Time.deltaTime;
        timeUI.SetTimeElasped(timeElapsed, false);

        if (CompanyManager.Instance.NumEscapists <= 0)
        {
            stateMachine.SetNewState(endState);
        }
    }

    // public void SetDuration(float d)
    // {
    //     duration = d;
    // }

    IEnumerator DayToNightTransition()
    {
        // Lights off
        blackScreenUI.Black();
        visualsManager.LightsOff();
        audioManager.PlayOneShot("LightSwitch", 0.1f);

        yield return new WaitForSeconds(2f);

        audioManager.PlayBackgroundMusic("NightSong");
        blackScreenUI.FadeOut();

        yield return new WaitForSeconds(1f);

        // UI Popup
        informationPopupUI.DisplayText("EMPLOYEES ARE TRYING TO ESCAPE", false);
    }

    public override void OnExit()
    {
        return;
    }
}
