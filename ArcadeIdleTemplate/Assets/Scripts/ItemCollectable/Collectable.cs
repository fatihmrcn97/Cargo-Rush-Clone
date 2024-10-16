using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable
{
    private Rigidbody _rb;
    private Collider _collider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<BoxCollider>();
    }

    public GameObject GameObject => gameObject;

    public void DeactivateObjAndPhysics()
    {
       _collider.enabled = false;
       _rb.useGravity = false;
       _rb.isKinematic = true;  
    }
}
