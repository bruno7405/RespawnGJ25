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
    [SerializeField] DayNightTransitionUI dayNightTransitionUI;

    // Singleton References
    AudioManager audioManager;
    VisualsManager visualsManager;
    InformationPopupUI informationPopupUI;
    bool inTransition = false;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        visualsManager = VisualsManager.Instance;
        informationPopupUI = InformationPopupUI.Instance;
    }

    public override void OnStart()
    {
        Debug.Log("night state");

        StartCoroutine(DayToNightTransition());

        timeElapsed = 0;
    }

    public override void OnUpdate()
    {
        if (inTransition) return;
        timeElapsed += Time.deltaTime;
        // timeUI.SetTimeElasped(timeElapsed, false);

        if (CompanyManager.Instance.NumRunners <= 0)
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
        inTransition = true;
        PlayerInput.active = false;

        #region Splash Art Transition
        blackScreenUI.FadeIn();
        yield return new WaitForSeconds(0.5f);
        blackScreenUI.FadeOut();

        dayNightTransitionUI.TransitionToNight(3f);
        yield return new WaitForSeconds(3);

        blackScreenUI.FadeIn();
        AudioManager.Instance.PlayOneShot("LightSwitch");
        yield return new WaitForSeconds(0.5f);
        dayNightTransitionUI.HideUI();
        visualsManager.LightsOff();
        blackScreenUI.FadeOut();

        #endregion

        audioManager.PlayBackgroundMusic("NightSong");
        PlayerInput.active = true;

        try
        {
            ((GameStateManager)stateMachine).InvokeNightStart();
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }

        yield return new WaitForSeconds(1f);

        // UI Popup
        inTransition = false;
        informationPopupUI.DisplayText("EMPLOYEES ARE TRYING TO ESCAPE", false);
    }

    public override void OnExit()
    {
        return;
    }
}
