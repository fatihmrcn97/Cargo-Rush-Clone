using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstItemTrigger : MonoBehaviour
{
    private ITriggerInteraction _putInBoxMachine;

    [SerializeField] private TriggerActivationOptions triggerActivationOptions;
    
    private void Awake()
    {
        _putInBoxMachine = GetComponentInParent<ITriggerInteraction>();
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
