using System.Collections.Generic;
using UnityEngine;

public class UpgradeState : State
{
    [SerializeField] List<Upgrade> upgrades = new List<Upgrade>();
    [SerializeField] UpgradesUI upgradesUI;

    [SerializeField] DayState dayState;

    public static UpgradeState instance;



    private void Awake()
    {
        instance = this;
    }

    public override void OnStart()
    {
        // Adds all money for the day
        // Checks quota(game over?)



        // Enables UpgradeUI
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
        ((GameStateManager)stateMachine).InvokeDayStart();
        stateMachine.SetNewState(dayState);

    }
    public override void OnExit()
    {
        return;
    }



}
