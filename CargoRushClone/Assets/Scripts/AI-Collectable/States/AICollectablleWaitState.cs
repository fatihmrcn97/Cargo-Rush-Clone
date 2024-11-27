using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollectablleWaitState : AICollectableBaseState
{
    private float _waitTime;
    private float timer = 0;
    public override void EnterState(AICollectableStateMananger ai)
    { 
        ai.animator.SetBool(TagManager.WALKING_BOOL_ANIM,false);
        _waitTime = UIManager.instance.globalVars.AICollectableAfterWorkWaitTime;
        timer = 0;
    }

    public override void UpdateState(AICollectableStateMananger ai)
    {
        if(timer > _waitTime)
            ai.SwitchState(ai.IdleState);
        timer += Time.deltaTime;
            
    }

    public override void OnTriggerEnter(AICollectableStateMananger ai, Collider other)
    {
        
    }
}
