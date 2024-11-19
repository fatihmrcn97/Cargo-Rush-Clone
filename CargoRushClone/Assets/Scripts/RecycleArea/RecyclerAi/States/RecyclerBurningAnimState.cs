using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RecyclerBurningAnimState : StateMachineBehaviour
{
    private RecyclerController _recyclerController;
    private Transform destination; 
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _recyclerController = animator.GetComponent<RecyclerController>();
        destination = _recyclerController.destination;
        
        _recyclerController.DropTheBoxToBurning();
    }
 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
         
    }
 
   
}
