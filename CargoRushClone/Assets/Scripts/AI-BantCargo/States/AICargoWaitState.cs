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
        if(ai.ItemList.StackedMaterialList().Count>0 && Vector3.Distance(ai.transform.position,ai.startPoint.position)>.85f) return;
        
        if (ai.MachineController.singleMaterial.Count == 0 && ai.ItemList.StackedMaterialList().Count == 0)
        {
            if (IsApproximatelyEqual(ai.transform.position, ai.startPoint.position, .55f))
            {
                ai.transform.LookAt(Vector3.forward);
                return;
            }
            // Alicak vericek yok bekleme noktasında bekle
            ai.destination = ai.startPoint;
            ai.shoudWait = true;
            ai.SwitchState(ai.WalkingState);
            return;
        }
        
        if(timer > _waitTime)
            ai.SwitchState(ai.IdleState);
        timer += Time.deltaTime;
    }

    public override void OnTriggerEnter(AICargoStateManager ai, Collider other)
    {
       
    }

    bool IsApproximatelyEqual(Vector3 a, Vector3 b, float tolerance)
    {
        return Vector3.Distance(a, b) <= tolerance;
    }
}
