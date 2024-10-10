using UnityEngine;

[CreateAssetMenu(menuName ="Stack SO/StackPosData")]
public class StackSO : ScriptableObject
{

    [SerializeField] private float x, y, z;

    public float X => x;
    public float Y => y;
    public float Z => z;
 

}
