using UnityEngine;

public class LookatCamera : MonoBehaviour
{
    private new Camera camera;

    private void Start()
    {
        camera = Camera.main;
    }

    private void Update()
    {
        transform.LookAt(transform.position + camera.transform.rotation * Vector3.back, camera.transform.rotation * Vector3.up);
    }
}