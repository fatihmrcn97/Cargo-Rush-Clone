using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICollectableStateMananger : MonoBehaviour
{
    #region STATES
    AICollectableBaseState _currentState;
    public AICollectableIdleState IdleState = new();
    public AICollectableWalkingState WalkingState = new();
    public AICollectableCollectState CollectState = new();
    public AICollectablleWaitState WaitState = new();
    #endregion

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [SerializeField] private Transform waitTransform;
    
    [SerializeField] private MachineController _machineController;
    public MachineController MachineController => _machineController;

    public IItemList ItemList;

    [HideInInspector] public Transform destination;
    [HideInInspector] public bool shouldWait;
    [HideInInspector] public Animator animator;

    
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ItemList = GetComponent<IItemList>();
        animator = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        _currentState = WaitState;
        _currentState.EnterState(this);
        Events.OnPackableItemSpawnerStarted += PackbleItemSpwanerWorking;
    } 

    private void OnTriggerEnter(Collider other)
    {
        _currentState.OnTriggerEnter(this, other);
    }

    private void Update()
    {  
        _currentState.UpdateState(this);
    }

    public void SwitchState(AICollectableBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    public void PackbleItemSpwanerWorking()
    {
        shouldWait = true;
        destination = waitTransform;
        SwitchState(WalkingState);
    }
 
}
