using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICollectableIdleState : AICollectableBaseState
{
    public override void EnterState(AICollectableStateMananger ai)
    {
  
        // item topla ve makine fullmu bak sürekli degilse itemi makineye götür
        ai.animator.SetBool(TagManager.WALKING_BOOL_ANIM,false);
        
    }

    public override void UpdateState(AICollectableStateMananger ai)
    {
        ai.animator.SetBool(TagManager.WALKING_BOOL_ANIM,false);
        if(PackableItemSpawner.Instance.allCollectables.Count<=0) return;
        if(ai.MachineController.convertedMaterials.Count >= ai.MachineController._addMaterialToMachine._maxConvertedMaterial) return; // Makine alim kapasitesine ulaşmıs
        if (ai.ItemList.CheckPlayerHandMax())
        {
            // Makineye bırakmaya giden state git yada Walking state git destination makine olsun
            ai.destination = ai.MachineController._addMaterialToMachine.transform;
            ai.SwitchState(ai.WalkingState);
        }
        else
        {
            // Collect State git
            ai.SwitchState(ai.CollectState);
        }
        //ai.SwitchState();
    }

    public override void OnTriggerEnter(AICollectableStateMananger ai, Collider other)
    {
        
    }
}
