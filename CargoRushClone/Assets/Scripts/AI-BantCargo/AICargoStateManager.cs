using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AICargoStateManager : MonoBehaviour
{
    #region STATES

    private AICargoBaseState _currentState;
    public AICargoIdleState IdleState = new();
    public AICargoWalkingState WalkingState = new();
    public AICargoWaitState WaitingState = new();
    #endregion

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [SerializeField] private GetMaterialFromMachine _machineController;
    public GetMaterialFromMachine MachineController => _machineController;

    public List<Transform> destinations;

    public IItemList ItemList;

    [HideInInspector] public Transform destination;

    [HideInInspector] public Animator anim;
    [HideInInspector] public bool shoudWait=false;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ItemList = GetComponent<IItemList>();
        anim = GetComponentInChildren<Animator>();
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

    public void SwitchState(AICargoBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }

    public Transform FindDestinationByItem(TapedItemStatus tapedItemStatus)
    {
        return tapedItemStatus switch
        {
            TapedItemStatus.nonTapped => destinations[0],
            TapedItemStatus.yellowBox => destinations[0],
            TapedItemStatus.pinkBox => destinations[1],
            TapedItemStatus.blueBox => destinations[2],
            _ => destinations[0]
        };
    }
}