using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICargoWalkingState : AICargoBaseState
{
    private Vector3 _randomizer;
    public override void EnterState(AICargoStateManager ai)
    {
        _randomizer = ai.diffentiateLocation;
    }

    public override void UpdateState(AICargoStateManager ai)
    {
        ai.Agent.SetDestination(ai.destination.position+_randomizer); 
        ai.anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        if (Vector3.Distance(ai.destination.position+_randomizer, ai.transform.position) >= .55) return;

        if (ai.shoudWait)
            ai.SwitchState(ai.WaitingState);
        else
            ai.SwitchState(ai.IdleState);
    }

    public override void OnTriggerEnter(AICargoStateManager ai, Collider other)
    {
    }
   
}