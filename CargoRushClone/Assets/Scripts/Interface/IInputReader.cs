
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
    public NewInputReader(Joystick joystick,Camera camera)
    {
        this.joystick = joystick;
        _camera = camera;
    }

    public Vector3 MoveDirection
    { 
        get
        {
            Vector3 direction = _camera.transform.forward * joystick.Vertical + _camera.transform.right * joystick.Horizontal;
            if (direction != Vector3.zero)
                direction.y -= gravity * Time.deltaTime;
            return direction;
        }
    }

}