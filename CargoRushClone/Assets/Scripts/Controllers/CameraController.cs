using Cinemachine;
using System.Collections;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;

    private void Awake()
    {
        instance = this;
    }

    public Transform player;

    public float smoothing;

    [Header("Kameralarr")]
    [SerializeField] private CinemachineVirtualCamera traning1Show;

    private Vector3 velocity = Vector3.zero;

    private void Update()
    { 
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothing);
    }

    private IEnumerator ChangeCameraCorotine(CinemachineVirtualCamera cinemachineCamera)
    {
        cinemachineCamera.Priority = 15;
        yield return new WaitForSeconds(3f);
        cinemachineCamera.Priority = 1;
    } 
    public void ChangeCamera(CinemachineVirtualCamera cinemachineCamera)
    {
        StartCoroutine(ChangeCameraCorotine(cinemachineCamera));
    }
 
}