
using UnityEngine;

public interface ICollectable
{
    GameObject GameObject { get; }

    void DeactivateObjAndPhysics();

    void PushInCircle(Vector3 direction);
}
