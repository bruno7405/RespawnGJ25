using UnityEngine;

public class Idle : State
{
    public override void OnStart()
    {
        Debug.Log("Started Idle");
    }

    public override void OnUpdate()
    {
        
    }

    public override void OnExit()
    {
        Debug.Log("Stopped Idle");
    }
}
