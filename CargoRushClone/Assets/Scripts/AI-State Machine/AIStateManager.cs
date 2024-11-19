using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIStateManager : MonoBehaviour
{

    AIBaseState currentState;
   public AIIdleState IdleState = new();
   public AIMovingState MovingState = new();

    private void Start()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(AIBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
