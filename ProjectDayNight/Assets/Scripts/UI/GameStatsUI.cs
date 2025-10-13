using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class GameStatsUI : MonoBehaviour
{
    [SerializeField] RectTransform statsContainer;
    [SerializeField] GameStatFade statPrefab;      // prefab with StatUIEntry + TMP Text
    [SerializeField] float statInterval = 1f;
    [SerializeField] float statMoveDistance = 10f;
    [SerializeField] UpgradeState upgradeState;
    [SerializeField] Color goodColor = new Color(0.3f, 1f, 0.3f, 1f); // green
    [SerializeField] Color badColor  = new Color(1f, 0.3f, 0.3f, 1f); // red

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GameStateManager.Instance.NewDay += DisplayStats;
    }
    private void DisplayStats()
    {
        // Start the display sequence.
        StartCoroutine(RunTransition());
    }
    IEnumerator RunTransition()
    {

        List<int[]> report = new();
        float lifeTime = 2f * statInterval; // fade time for each stat
        // 3) Show stats at fixed interval; each new one pushes older ones DOWN
        int profit = CompanyManager.Instance.GetProfit();

        if (profit >= 0) SpawnStat("Money: +$" + profit.ToString(), true, lifeTime);
        else SpawnStat("Money: -$" + Mathf.Abs(profit).ToString(), false, lifeTime);
        yield return new WaitForSeconds(statInterval);
        SpawnStat("Lost 3 Employees", false, lifeTime);
        yield return new WaitForSeconds(statInterval);
        SpawnStat("Gained 5 Morale", true, lifeTime);
        yield return new WaitForSeconds(statInterval);

        // 6) Now allow upgrades to be shown
        upgradeState.ShowUpgradesUI();
    }
    void SpawnStat(String stat, bool isPositive, float lifeTime)
    {
        var entry = Instantiate(statPrefab, statsContainer);

        // newest at top: set as first sibling so older ones push DOWN
        entry.transform.SetAsFirstSibling();
        // set text & color
        Color col = isPositive ? goodColor : badColor;
        entry.Init(stat, col, lifeTime);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
