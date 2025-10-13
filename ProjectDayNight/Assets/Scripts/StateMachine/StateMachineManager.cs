using UnityEngine;

public class StateMachineManager : MonoBehaviour
{
    public State currentState;

    protected virtual void Start()
    {
        if (currentState != null)
        {
            currentState.OnStart();
        }
    }

    protected virtual void Update()
    {
        if (currentState != null)
        {
            currentState.OnUpdate();
        }
    }

    public virtual void SetNewState(State state)
    {
        if (state != null)
        {
            currentState?.OnExit();
            currentState = state;
            currentState.OnStart();
        }
    }
}