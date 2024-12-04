using System;
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

    IInputReader _inputJoystick, _inputMouse;
    IMover _mover;

    private bool _isInputTypeJoystick = true;

    private void Awake()
    {
        _joystick = FindObjectOfType<Joystick>();
        _characterController = GetComponent<CharacterController>();
        _inputJoystick = new NewInputReader(_joystick, Camera.main);
        _inputMouse = new MouseInputReader(Camera.main);
        _mover = new PlayerMoveController(_characterController, playerSO);

        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        CameraController.instance.player = gameObject.transform;
    }

    private void OnEnable()
    {
        Events.OnWorldCanvasOpened += SwitchInputType;
    }

    private void OnDisable()
    {
        Events.OnWorldCanvasOpened -= SwitchInputType;
    }

    private void SwitchInputType()
    {
        if (_isInputTypeJoystick)
        {
            _isInputTypeJoystick = false;
            UIManager.instance.joystick.SetActive(false);
            // _inputMouse = new MouseInputReader(Camera.main);
        }
        else
        {
            UIManager.instance.joystick.SetActive(true);
            _isInputTypeJoystick = true;
            _joystick.MakeZero();

            // _inputJoystick = new NewInputReader(_joystick,Camera.main);
        }
    }

    public void SpeedUpdated()
    {
        _mover.SpeedUpgraded();
    }

    private void Update()
    {
        if (_isInputTypeJoystick)
            PlayerMovementHandler();
        else
            PlayerMovementHandlerOnWorldUI();
    }

    private void PlayerMovementHandler()
    {
        if (_inputJoystick.MoveDirection != Vector3.zero)
        {
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
            float joystickMagnitude = new Vector2(_joystick.Horizontal, _joystick.Vertical).magnitude;
            anim.SetFloat(TagManager.CHAR_SPEED_FLOAT, joystickMagnitude);
            HandleRotation(_inputJoystick.MoveDirection);
        }
        else
        {
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
        }

        _mover.FixedTick(_inputJoystick.MoveDirection);
    }

    private void PlayerMovementHandlerOnWorldUI()
    {
        if (_inputMouse.MoveDirection != Vector3.zero)
        {
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
            anim.SetFloat(TagManager.CHAR_SPEED_FLOAT, _inputMouse.MoveDirection.magnitude);
            HandleRotation(_inputMouse.MoveDirection);
        }
        else
        {
            anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
        }

        _mover.FixedTick(_inputMouse.MoveDirection);
    }

    void HandleRotation(Vector3 currentMovement)
    {
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