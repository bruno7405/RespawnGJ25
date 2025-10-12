using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public State currentState;

    protected void Start()
    {
        if (currentState != null)
        {
            currentState.OnStart();
        }
    }

    protected void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public void SetNewState(State state)
    {
        if (state != null)
        {
            currentState?.OnExit();
            currentState = state;
            currentState.OnStart();
        }
    }
}