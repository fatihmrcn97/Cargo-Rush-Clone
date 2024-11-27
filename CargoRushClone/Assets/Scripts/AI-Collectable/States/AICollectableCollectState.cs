using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollectableCollectState : AICollectableBaseState
{
    public override void EnterState(AICollectableStateMananger ai)
    {
         
    }

    public override void UpdateState(AICollectableStateMananger ai)
    {
        if(PackableItemSpawner.Instance.allCollectables.Count<=0 && ai.ItemList.StackedMaterialList().Count<=0) 
            ai.SwitchState(ai.IdleState);
 
        if (PackableItemSpawner.Instance.allCollectables.Count <= 0 && ai.ItemList.StackedMaterialList().Count >= 1)
        {
            //elindekileri makineye bırakmaya git
            ai.destination = ai.MachineController._addMaterialToMachine.transform;
            ai.SwitchState(ai.WalkingState);
        }

        if (PackableItemSpawner.Instance.allCollectables.Count > 0 && !ai.ItemList.CheckPlayerHandMax())
        {
            ai.animator.SetBool(TagManager.WALKING_BOOL_ANIM,true);
            var destination = PackableItemSpawner.Instance.allCollectables[0].transform;
            ai.Agent.SetDestination(destination.position);
        }

        if (ai.ItemList.CheckPlayerHandMax())
        {
            // Walking state destination hallet
            ai.shouldWait = true;
            ai.destination = ai.MachineController._addMaterialToMachine.transform;
            ai.SwitchState(ai.WalkingState);
            
        }
    }

    public override void OnTriggerEnter(AICollectableStateMananger ai, Collider other)
    {
        
    }
}
