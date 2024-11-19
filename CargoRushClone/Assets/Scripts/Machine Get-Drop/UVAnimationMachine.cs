using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVAnimationMachine : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer belt;
    [SerializeField] float scrollSpeed = 1;
    [SerializeField] int index_material = 0;
    float offSet;
    private void Update()
    {
        offSet += (Time.deltaTime * scrollSpeed / 5);
        belt.materials[index_material].mainTextureOffset = new Vector2(offSet, 0);
    }
}
