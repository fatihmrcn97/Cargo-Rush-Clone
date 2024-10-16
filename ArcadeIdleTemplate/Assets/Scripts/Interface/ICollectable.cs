
using UnityEngine;

public interface ICollectable
{
    GameObject GameObject { get; }

    void DeactivateObjAndPhysics();
}
