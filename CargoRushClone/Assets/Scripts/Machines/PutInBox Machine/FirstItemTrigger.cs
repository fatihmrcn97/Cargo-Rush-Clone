using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstItemTrigger : MonoBehaviour
{
    private ITriggerInteraction _putInBoxMachine;

    [SerializeField] private TriggerActivationOptions triggerActivationOptions;
    
    private Collider _boxCollider;
    private void Awake()
    {
        _boxCollider = GetComponent<Collider>();
        _boxCollider.enabled = false;
        _putInBoxMachine = GetComponentInParent<ITriggerInteraction>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f); // SAFE OPERATIONS
        _boxCollider.enabled = true;
    }

    private void OnTriggerEnter(Collider other) 
    {
        if (!other.CompareTag(TagManager.PACKABLE_ITEM)) return;
        
        switch (triggerActivationOptions)
        {
            case TriggerActivationOptions.OnPathEnded:
                _putInBoxMachine.PathEnded(other.gameObject);
                break;
            case TriggerActivationOptions.OnTappingSectionTriggered:
                _putInBoxMachine.OnTriggerBanting(other.gameObject);
                break;
            default:
                return;
        }
    }
}
