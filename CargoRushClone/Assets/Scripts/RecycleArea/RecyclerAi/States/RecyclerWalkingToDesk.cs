using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RecyclerWalkingToDesk : StateMachineBehaviour
{
    private RecyclerController _recyclerController;
    private Transform destination;
    private NavMeshAgent ai;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _recyclerController = animator.GetComponent<RecyclerController>();
        destination = _recyclerController.destination;
        ai = _recyclerController.Agent;
    }
 
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.SetDestination(destination.position); 
        
        if (Vector3.Distance(destination.position, ai.transform.position) < .35f)
        { 
            ai.SetDestination(ai.transform.position);  
            animator.SetTrigger("burning");
        }
    }
 
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }
}
