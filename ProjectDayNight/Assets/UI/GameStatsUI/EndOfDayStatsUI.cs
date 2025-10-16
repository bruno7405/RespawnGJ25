using UnityEngine;
using TMPro;
using System;
using System.Collections;
using System.Collections.Generic;

public class EndOfDayStatsUI : MonoBehaviour
{
    [SerializeField] GameObject root;
    [SerializeField] RectTransform statsContainer; // where stats are spawned
    [SerializeField] GameStatFade statPrefab;      // prefab with StatUIEntry + TMP Text
    [SerializeField] float statInterval = 1f;
    [SerializeField] float statMoveDistance = 10f;
    [SerializeField] Color goodColor = new Color(0.3f, 1f, 0.3f, 1f);
    [SerializeField] Color badColor  = new Color(1f, 0.3f, 0.3f, 1f);
    [SerializeField] Animator animator;

    
    public event Action OnDisplayClosed;

    private void Awake()
    {
        root.SetActive(false);
    }

    public void DisplayStats()
    {
        root.SetActive(true);

        foreach (Transform child in statsContainer)
        {
            Destroy(child.gameObject);
        }

        StartCoroutine(RunTransition());
    }

    IEnumerator RunTransition()
    {
        float lifeTime = 2f * statInterval; // fade time for each stat

        int profit = CompanyManager.Instance.GetProfit();

        yield return new WaitForSeconds(0.35f); // wait for panel slide in

        if (profit >= 0) SpawnStat("Money: +$" + profit.ToString(), true, lifeTime);
        else SpawnStat("Money: -$" + Mathf.Abs(profit).ToString(), false, lifeTime);
        yield return new WaitForSeconds(statInterval);
        SpawnStat("Lost 3 Employees", false, lifeTime);
        yield return new WaitForSeconds(statInterval);
        SpawnStat("Gained 5 Morale", true, lifeTime);
        yield return new WaitForSeconds(statInterval);

        StartCoroutine(CloseStatsPanel());
    }

    void SpawnStat(String stat, bool isPositive, float lifeTime)
    {
        var statCard = Instantiate(statPrefab, statsContainer);
        Color col = isPositive ? goodColor : badColor;
        statCard.UpdateUI(stat, col, lifeTime);
    }

    private IEnumerator CloseStatsPanel()
    {
        animator.SetTrigger("slideOut");
        yield return new WaitForSeconds(0.5f);
        OnDisplayClosed?.Invoke();
        root.SetActive(false);
    }
}
