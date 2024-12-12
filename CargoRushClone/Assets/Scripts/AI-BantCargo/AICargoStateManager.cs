using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI; 
using Random = UnityEngine.Random;

public class AICargoStateManager : MonoBehaviour , IAIWorker
{
    #region STATES

    private AICargoBaseState _currentState;
    public AICargoIdleState IdleState = new();
    public AICargoWalkingState WalkingState = new();
    public AICargoWaitState WaitingState = new();

    #endregion

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

    private float agentBaseSpeed;
    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        ItemList = GetComponent<IItemList>();
        anim = GetComponentInChildren<Animator>();
        _currentMachine = machineControllers[0];

        var startPosObj = new GameObject();
        startPosObj.transform.position = transform.position;
        startPoint = startPosObj.transform;
        
        if(!PlayerPrefs.HasKey("AgentSpeed"))
            PlayerPrefs.SetFloat("AgentSpeed",0);
        agentBaseSpeed = _agent.speed;
        _agent.speed = agentBaseSpeed + PlayerPrefs.GetFloat("AgentSpeed");
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
        //Debug.Log(_currentState);
        _currentState.UpdateState(this);
    }

    public void RandomGetMaterialFromMachine()
    {
        var fakeMachines = machineControllers
            .Where(item => item.gameObject.activeInHierarchy && item.singleMaterial.Count > 0).ToList();
        _currentMachine = fakeMachines.Count == 0 ? machineControllers[0] : fakeMachines[Random.Range(0, fakeMachines.Count)];
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

    public GetMaterialFromMachine GetMachineController()
    {
        return _currentMachine;
    }

    private void OnEnable()
    {
        Events.OnSpeedUpgradeForAI += SpeedUpgrade;
        Events.OnWorkerBoosterClaimed += WorkerBoosterClaimed;
    }

    private void OnDisable()
    {
        Events.OnSpeedUpgradeForAI -= SpeedUpgrade;
        Events.OnWorkerBoosterClaimed -= WorkerBoosterClaimed;
    }

    private void WorkerBoosterClaimed(float time)
    {
        StartCoroutine(WorkerBooster(time));
    }

    private IEnumerator WorkerBooster(float time)
    {
        _agent.speed = agentBaseSpeed + PlayerPrefs.GetFloat("AgentSpeed") + .5f;
        yield return new WaitForSeconds(150f);
        _agent.speed = agentBaseSpeed + PlayerPrefs.GetFloat("AgentSpeed");
    }

    public void SpeedUpgrade()
    {
        _agent.speed = agentBaseSpeed + PlayerPrefs.GetFloat("AgentSpeed");
    }
}