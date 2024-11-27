using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICargoWaitState : AICargoBaseState
{
    private float _waitTime;
    private float timer = 0;
    public override void EnterState(AICargoStateManager ai)
    {
        ai.anim.SetBool(TagManager.WALKING_BOOL_ANIM,false);
        _waitTime = UIManager.instance.globalVars.AICargoAfterWorkWaitTime;
        timer = 0;
    }

    public override void UpdateState(AICargoStateManager ai)
    {
       if(timer > _waitTime)
           ai.SwitchState(ai.IdleState);
       timer += Time.deltaTime;
    }

    public override void OnTriggerEnter(AICargoStateManager ai, Collider other)
    {
       
    }
}
