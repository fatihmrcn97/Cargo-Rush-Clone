using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICargoBaseState 
{
    public abstract void EnterState(AICargoStateManager ai);

    public abstract void UpdateState(AICargoStateManager ai);

    public abstract void OnTriggerEnter(AICargoStateManager ai,Collider other);

}
