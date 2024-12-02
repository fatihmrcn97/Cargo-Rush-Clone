using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIStateManager : MonoBehaviour, IAIWorker
{
    AIBaseState currentState;
    public AIIdleState IdleState = new();
    public AIMovingState WalkingState = new();
    public AIWaitingState WaitingState = new();

    public List<Transform> destinations;
    [SerializeField] private List<GetMaterialFromMachine> machineControllers;

    public Vector3 diffentiateLocation;

    private NavMeshAgent _agent;
    public NavMeshAgent Agent => _agent;
    private GetMaterialFromMachine _currentMachine;
    public GetMaterialFromMachine MachineController => _currentMachine;


    public IItemList ItemList;
    [HideInInspector] public Transform destination;
    [HideInInspector] public Animator anim;
    [HideInInspector] public bool shoudWait = false;
    [HideInInspector] public Transform startPoint;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ItemList = GetComponent<IItemList>();
        anim = GetComponentInChildren<Animator>();
        _currentMachine = machineControllers[0];

        var startPosObj = new GameObject();
        startPosObj.transform.position = transform.position;
        startPoint = startPosObj.transform;
    }

    private void Start()
    {
        currentState = IdleState;

        currentState.EnterState(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(this, other);
    }

    private void Update()
    {
        Debug.Log(currentState);
        currentState.UpdateState(this);
    }

    public void SwitchState(AIBaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }

    public Transform FindDestinationByItem(TapedItemStatus tapedItemStatus)
    {
        return tapedItemStatus switch
        {
            TapedItemStatus.yellowBox => GetDestinationByItem(TapedItemStatus.yellowBox),
            TapedItemStatus.pinkBox => GetDestinationByItem(TapedItemStatus.pinkBox),
            TapedItemStatus.blueBox => GetDestinationByItem(TapedItemStatus.blueBox),
            _ => destinations[0]
        };
    }

    private Transform GetDestinationByItem(TapedItemStatus tapedItemStatus)
    {
        foreach (var destination in destinations)
        {
            if (destination.GetComponent<AddMaterialToCargo>().TapedItemStatus == tapedItemStatus &&
                !destination.GetComponent<AddMaterialToCargo>().CargoPlace.IsCurrierGoing
               )
                return destination;
        }

        return null;
    }

    public void CheckIfAIStillHasItem()
    {
        if (ItemList.StackedMaterialList().Count > 0)
        {
            var item =
                GetDestinationByItem(ItemList.StackedMaterialList()[0].GetComponent<IItem>().TapedItemStatus());
            if (item != null)
            {
                shoudWait = true;
                destination = item.transform;
                SwitchState(WalkingState);
            }

            if (item == null)
            {
                shoudWait = true;
                destination = startPoint;
                SwitchState(WalkingState);
            }
        }
    }

    public GetMaterialFromMachine GetMachineController()
    {
        return _currentMachine;
    }
}