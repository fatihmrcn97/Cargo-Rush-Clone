using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour, ICollectable , IItem
{

    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;
    [SerializeField] private CollectableTypes collectableTypes;

    public CollectableTypes CollectableTypes => collectableTypes;
    
    
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

        _rb.AddForce(direction * 155);
        StartCoroutine(PushInCircleCorotine());
    }

    private IEnumerator PushInCircleCorotine()
    {
        yield return new WaitForSeconds(.5f);
        tag = TagManager.COLLECTABLE_ITEM;
    }

    public ItemStatus ItemStatus()
    {
        return itemStatus;
    }

    public TapedItemStatus TapedItemStatus()
    {
        return tapedItemStatus;
    }

    public void SetStatus(ItemStatus iStatus, TapedItemStatus tapedStatus)
    {
        itemStatus = iStatus;
        tapedItemStatus = tapedStatus;
    }
}
