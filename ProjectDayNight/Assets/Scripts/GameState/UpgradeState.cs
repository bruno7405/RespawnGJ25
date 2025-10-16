using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : State
{
    [SerializeField] List<Upgrade> upgrades = new();

    [SerializeField] EndOfDayStatsUI gameStatsUI;
    [SerializeField] UpgradesUI upgradesUI;
    [SerializeField] BlackScreenUI blackScreenUI;

    [SerializeField] DayState dayState;

    public static UpgradeState instance;

    // Singleton References
    AudioManager audioManager;
    InformationPopupUI informationPopupUI;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;

        audioManager = AudioManager.Instance;
        informationPopupUI = InformationPopupUI.Instance;
    }

    /// <summary>
    /// Displays stats UI
    /// Wait for display to close, then show upgrades UI (event)
    /// </summary>
    public override void OnStart()
    {
        ((GameStateManager)stateMachine).InvokeNewDay();
        gameStatsUI.DisplayStats();
    }

    public override void OnUpdate() { }

    public void ShowUpgradesUI()
    {
        upgradesUI.gameObject.SetActive(true);

        // Sets upgrade options
        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, upgrades.Count);
            upgradesUI.CreateUpgradeButton(upgrades[index]);
            upgrades.RemoveAt(index);
        }
    }

    /// <summary>
    /// Changes stats based on upgrade chosen
    /// </summary>
    /// <param name="upgrade"></param>
    public void SetNewUpgrades(Upgrade upgrade)
    {
        switch (upgrade.category)
        {
            case Upgrade.Category.playerSpeed: // increase speed
                PlayerMovement.Instance.IncreaseSpeed(upgrade.increasePercentage);
                break;
            case Upgrade.Category.playerInteractRange: // increase range
                PlayerAttack.Instance.IncreaseRange(upgrade.increasePercentage);
                PlayerInteractor.Instance.IncreaseRange(upgrade.increasePercentage);
                break;
            case Upgrade.Category.playerLightRange: // increase view distance (night time)
                PlayerVisibility.Instance.IncreaseLightRange(upgrade.increasePercentage);
                break;
            case Upgrade.Category.morale: // increase morale
                CompanyManager.Instance.ChangeCompanyMorale(upgrade.increasePercentage);    
                break;
            case Upgrade.Category.productivity: // increase revenue
                CompanyManager.Instance.ChangeCompanyRevenue(upgrade.increasePercentage);
                break;
        }



        StartCoroutine(SetDayState());
    }

    private IEnumerator SetDayState()
    {
        blackScreenUI.FadeIn();
        yield return new WaitForSeconds(1);

        stateMachine.SetNewState(dayState);

    }
    public override void OnExit()
    {
        return;
    }

    private void OnEnable()
    {
        gameStatsUI.OnDisplayClosed += ShowUpgradesUI;
    }

    private void OnDisable()
    {
        gameStatsUI.OnDisplayClosed -= ShowUpgradesUI;
    }

}
