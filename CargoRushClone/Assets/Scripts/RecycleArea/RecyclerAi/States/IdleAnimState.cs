using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleAnimState : StateMachineBehaviour
{
    private RecyclerController _recyclerController;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _recyclerController = animator.GetComponent<RecyclerController>();
    }
 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_recyclerController.shouldMove)
        {
            _recyclerController.shouldMove = false;
            animator.SetTrigger("walking");
        }
    }
 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
 

   
}
