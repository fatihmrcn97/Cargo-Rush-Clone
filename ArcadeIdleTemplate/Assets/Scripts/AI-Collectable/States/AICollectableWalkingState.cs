using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollectableWalkingState : AICollectableBaseState
{
  
    public override void EnterState(AICollectableStateMananger ai)
    {
  
    }

    public override void UpdateState(AICollectableStateMananger ai)
    {
        ai.Agent.SetDestination(ai.destination.position);
        if(Vector3.Distance(ai.destination.position,ai.transform.position) >=.35) return;
        ai.SwitchState(ai.IdleState);
            
    }

    public override void OnTriggerEnter(AICollectableStateMananger ai, Collider other)
    {
        
    }
}
