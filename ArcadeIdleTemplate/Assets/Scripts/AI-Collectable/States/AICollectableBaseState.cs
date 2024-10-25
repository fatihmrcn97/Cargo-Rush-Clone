using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AICollectableBaseState
{
    public abstract void EnterState(AICollectableStateMananger ai);

    public abstract void UpdateState(AICollectableStateMananger ai);

    public abstract void OnTriggerEnter(AICollectableStateMananger ai,Collider other);
}
