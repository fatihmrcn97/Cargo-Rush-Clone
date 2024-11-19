using UnityEngine;

public class PlayerMovement : MonoBehaviour
{ 
    private Joystick _joystick;
    private CharacterController _characterController;
    private Animator anim;
    // private Vector3 _direction;


    [SerializeField] private PlayerSO playerSO;
    [SerializeField] private float _rotationFactorPerFrame = 1f;

    private Vector3 _rotationVector = Vector3.zero;
      
    IInputReader _input;
    IMover _mover;

    private void Awake()
    {
        _joystick = FindObjectOfType<Joystick>();
        _characterController = GetComponent<CharacterController>();
        _input = new NewInputReader(_joystick,Camera.main);
        _mover = new PlayerMoveController(_characterController, playerSO);

        anim = GetComponentInChildren<Animator>(); 
    }
    private void Start()
    {
        CameraController.instance.player = gameObject.transform;
    }

    public void SpeedUpdated()
    {
        _mover.SpeedUpgraded();
    }

    private void Update()
    {
        PlayerMovementHandler();
    }

    private void PlayerMovementHandler()
    { 
        if (_input.MoveDirection != Vector3.zero)
        { 
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
            HandleRotation(_input.MoveDirection);
        }
        else
        {
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, false); 
        }

        _mover.FixedTick(_input.MoveDirection);
    }

    void HandleRotation(Vector3 currentMovement)
    {
        //Vector3 positionToLookAt;
        //positionToLookAt.x = currentMovement.x;
        //positionToLookAt.y = 0f;
        //positionToLookAt.z = currentMovement.z;
        //Quaternion currentRotation = transform.rotation;
        //Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
        //transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
        currentMovement.y = 0;
        _rotationVector = Vector3.Slerp(_rotationVector, currentMovement, Time.deltaTime * _rotationFactorPerFrame);
        transform.rotation = Quaternion.LookRotation(_rotationVector);

    }


    public void ShootingRotationHandler(Vector3 target)
    {
        var lookPos = target - transform.position;
        if (lookPos == Vector3.zero)
            return;
        lookPos.y = 0;
        var rotation = Quaternion.LookRotation(lookPos);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * 105);
    }

}