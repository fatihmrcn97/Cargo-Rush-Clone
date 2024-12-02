using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovingState : AIBaseState
{
    private Vector3 _randomizer;
    public override void EnterState(AIStateManager ai)
    { 
        _randomizer = ai.diffentiateLocation;
    }

    public override void OnTriggerEnter(AIStateManager ai, Collider other)
    { 
    }

    public override void UpdateState(AIStateManager ai)
    { 
        ai.Agent.SetDestination(ai.destination.position+_randomizer); 
        ai.anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        if (Vector3.Distance(ai.destination.position+_randomizer, ai.transform.position) >= .55) return;

        if (ai.shoudWait)
            ai.SwitchState(ai.WaitingState);
        else
            ai.SwitchState(ai.IdleState);
    }
}
