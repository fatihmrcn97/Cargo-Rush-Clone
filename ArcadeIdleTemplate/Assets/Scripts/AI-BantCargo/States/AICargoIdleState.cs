using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AICargoIdleState : AICargoBaseState
{
    public override void EnterState(AICargoStateManager ai)
    {
    }

    public override void UpdateState(AICargoStateManager ai)
    {
        if (ai.MachineController.singleMaterial.Count > 0)
        {
            //Eli fulse kargo bırakmaya git
            if (ai.ItemList.CheckPlayerHandMax())
            {
                ai.destination = ai.cargoPlace;
                ai.SwitchState(ai.WalkingState);
            }

            // Walking to get place 
            ai.destination = ai.MachineController.transform;
            ai.SwitchState(ai.WalkingState);
        }

        if (ai.ItemList.CheckPlayerHandMax())
        {
            // kARGoyu bırakmaya giden destination
            ai.destination = ai.cargoPlace;
            ai.SwitchState(ai.WalkingState);
        }
        else
        {
            if (ai.MachineController.singleMaterial.Count > 0)
            {
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