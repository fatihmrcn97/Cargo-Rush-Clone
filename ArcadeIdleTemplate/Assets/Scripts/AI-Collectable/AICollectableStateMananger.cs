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
    #endregion

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [SerializeField] private MachineController _machineController;
    public MachineController MachineController => _machineController;

    public IItemList ItemList;

    public Transform destination;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ItemList = GetComponent<IItemList>();
    }

    private void Start()
    {
        _currentState = IdleState;
        _currentState.EnterState(this);
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
}
