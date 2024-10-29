using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICargoWalkingState : AICargoBaseState
{
    public override void EnterState(AICargoStateManager ai)
    {
        ai.anim.SetBool(TagManager.WALKING_BOOL_ANIM,true);
    }

    public override void UpdateState(AICargoStateManager ai)
    {
        ai.Agent.SetDestination(ai.destination.position);
        if(Vector3.Distance(ai.destination.position,ai.transform.position) >=.55) return;
        ai.SwitchState(ai.IdleState);
    }

    public override void OnTriggerEnter(AICargoStateManager ai, Collider other)
    {
       
    }
}
