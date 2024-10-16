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

    public void PushInCircle(Vector3 direction)
    {
        transform.parent = null;
        tag = TagManager.Default;
        _collider.enabled = true;
        _rb.useGravity = true;
        _rb.isKinematic = false;

        _rb.AddForce(direction * 25);
        StartCoroutine(PushInCircleCorotine());
    }

    private IEnumerator PushInCircleCorotine()
    {
        yield return new WaitForSeconds(.5f);
        tag = TagManager.COLLECTABLE_ITEM;
    }
}
