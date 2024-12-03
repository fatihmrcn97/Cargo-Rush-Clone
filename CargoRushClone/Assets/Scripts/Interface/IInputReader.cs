using UnityEngine;

public interface IInputReader
{
    public Vector3 MoveDirection { get; }
}


public class NewInputReader : IInputReader
{
    private Joystick joystick;
    readonly float gravity = 35f;
    private readonly Camera _camera;

    public NewInputReader(Joystick joystick, Camera camera)
    {
        this.joystick = joystick;
        _camera = camera;
    }

    public Vector3 MoveDirection
    {
        get
        {
            Vector3 direction = _camera.transform.forward * joystick.Vertical +
                                _camera.transform.right * joystick.Horizontal;
            if (direction != Vector3.zero)
                direction.y -= gravity * Time.deltaTime;
            return direction;
        }
    }
}

public class MouseInputReader : IInputReader
{
    private readonly Camera _camera;
    readonly float gravity = 35f;

    public MouseInputReader(Camera camera)
    {
        _camera = camera;
    }
    Vector3 GetMouseDirection()
    {
        Vector3 mousePos = Input.mousePosition;
        Vector3 center = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        Vector3 offset = mousePos - center;
        offset.Normalize(); // Normalize to get a unit vector

        // Adjust for camera orientation (optional)
        offset = _camera.transform.rotation * offset;

        return offset;
    }
    public Vector3 MoveDirection
    {
        get
        {
            Vector3 direction = GetMouseDirection();
            if (Input.GetMouseButton(0))
            {
            
                return direction;
            }
            //
            // direction = _camera.transform.forward *  Input.GetAxis("Mouse X") +
            //             _camera.transform.right * Input.GetAxis("Mouse Y");
            // if (direction != Vector3.zero)
            //     direction.y -= gravity * Time.deltaTime;
            //return direction;
            return Vector3.zero;
        }
    }
}