using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICargoIdleState : AICargoBaseState
{
    public override void EnterState(AICargoStateManager ai)
    {
        ai.anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
    }

    public override void UpdateState(AICargoStateManager ai)
    {
        if (ai.MachineController.singleMaterial.Count == 0 && ai.ItemList.StackedMaterialList().Count == 0)
        {
            // Alicak vericek yok bekleme noktasında git ve bekle
            ai.shoudWait = true;
            ai.destination = ai.startPoint;
            ai.SwitchState(ai.WalkingState);
        }
        
        if (ai.MachineController.singleMaterial.Count > 0)
        {
            //Eli fulse kargo bırakmaya git
            if (ai.ItemList.CheckPlayerHandMax())
            {
                ai.destination =
                    ai.FindDestinationByItem(ai.ItemList.StackedMaterialList()[0].GetComponent<IItem>()
                        .TapedItemStatus());
                ai.SwitchState(ai.WalkingState);
                ai.shoudWait = true;
                return;
            }

            ai.shoudWait = false;
            // Walking to get place 
            ai.destination = ai.MachineController.transform;
            ai.SwitchState(ai.WalkingState);
        }

        if (ai.ItemList.CheckPlayerHandMax())
        {
            ai.shoudWait = true;
            // kARGoyu bırakmaya giden destination
            ai.destination =
                ai.FindDestinationByItem(ai.ItemList.StackedMaterialList()[0].GetComponent<IItem>().TapedItemStatus());
            ai.SwitchState(ai.WalkingState);
        }
        else
        {
            if (ai.MachineController.singleMaterial.Count > 0)
            {
                ai.shoudWait = false;
                ai.destination = ai.MachineController.transform;
                ai.SwitchState(ai.WalkingState);
            }
        }

        //ai.SwitchState();
    }

    public override void OnTriggerEnter(AICargoStateManager ai, Collider other)
    {
    }

}