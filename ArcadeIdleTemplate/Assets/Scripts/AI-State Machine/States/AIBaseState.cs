using UnityEngine;
public abstract class AIBaseState
{
   public abstract void EnterState(AIStateManager ai);

   public abstract void UpdateState(AIStateManager ai);

   public abstract void OnTriggerEnter(AIStateManager ai,Collider other);
}
