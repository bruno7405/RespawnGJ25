using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : State
{
    [SerializeField] List<Upgrade> upgrades = new();

    [SerializeField] GameStatsUI gameStatsUI;
    [SerializeField] UpgradesUI upgradesUI;

    [SerializeField] DayState dayState;

    public static UpgradeState instance;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
        }
        else instance = this;
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

    public void ShowUpgradesUI()
    {
        upgradesUI.gameObject.SetActive(true);

        // Sets upgrade options
        for (int i = 0; i < 2; i++)
        {
            int index = Random.Range(0, upgrades.Count);
            upgradesUI.CreateUpgradeButton(upgrades[index]);
            //upgrades.RemoveAt(index);
        }
    }

    public override void OnUpdate() {}

    public void SetNewUpgrades(Upgrade upgrade)
    {
        Debug.Log("Upgrading: " + upgrade.stat + " by " + upgrade.increasePercentage.ToString() + "%");

        Invoke(nameof(SetDayState), 1);
    }

    private void SetDayState()
    {
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
