using System;
using UnityEngine;

public class BossTask : GameEvent
{
    [SerializeField] private string description;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Vector2 locationOffset;
    [SerializeField] int dollarPenalty = 0;
    public string Description => description;
    private Vector2 location;
    public Vector2 Location => location;

    private void setColorTilNight(Color color)
    {
        sr.color = color;
        void HandleNightStart()
        {
            sr.color = Color.white;
            GameStateManager.Instance.NightStart -= HandleNightStart;
        }
        GameStateManager.Instance.NightStart += HandleNightStart;
    }

    protected override void FailTask()
    {
        base.FailTask();
        setColorTilNight(Color.red);
        Debug.Log("Player Failed Boss Task! -$" + dollarPenalty);
        if (!CompanyManager.Instance.SpendMoney(dollarPenalty))
            GameStateManager.Instance.GameOver();
    }
    
    protected override void CompleteTask()
    {
        base.CompleteTask();
        setColorTilNight(Color.green);
    }

    protected override void OnStart()
    {
        BossTaskManager.Instance.RegisterTask(this);
        Debug.Log("Registered Boss Task: " + Name);
        location = locationOffset + (sr == null ? (Vector2)transform.position : new(transform.position.x, sr.bounds.min.y));
    }
}