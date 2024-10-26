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
    #endregion

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;

    [SerializeField] private GetMaterialFromMachine _machineController;
    public GetMaterialFromMachine MachineController => _machineController;

    public Transform cargoPlace;
    
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

    public void SwitchState(AICargoBaseState state)
    {
        _currentState = state;
        state.EnterState(this);
    }
}
