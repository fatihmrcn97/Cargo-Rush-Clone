using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomCustomStackSystem : MonoBehaviour,IStackSystem
{ 
    [SerializeField] private Transform materialDropPosition , constantPositon;
 
    public void DropPointHandle()
    {
        materialDropPosition.position = constantPositon.position + new Vector3(Random.Range(-.25f, .25f),
            Random.Range(-.05f, .25f), Random.Range(-.25f, .25f));
    }

    public void SetTheStackPositonBack(int stackMaterialCount)
    {
        materialDropPosition.position = constantPositon.position + new Vector3(Random.Range(-.25f, .25f),
            Random.Range(-.05f, .25f), Random.Range(-.25f, .25f));
    }

    public Transform MaterialDropPositon()
    {
        return materialDropPosition;
    }

}
