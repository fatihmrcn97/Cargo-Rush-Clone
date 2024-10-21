using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class RecyclerController : MonoBehaviour
{

    [SerializeField] private Transform handPosition;

    [SerializeField] private Transform flameGarbagePos;
    
    public NavMeshAgent Agent;

     [HideInInspector] public Transform destination;

    public bool shouldMove = false;

    private GameObject boxToBurn;

    public void SetDestination(Transform dst)
    {
        destination = dst;
    }

    public void SetBox(GameObject box,Transform startPos)
    {
        boxToBurn = box;
        boxToBurn.transform.SetParent(transform);
        boxToBurn.transform.DOLocalJump(handPosition.localPosition, 1, 1, .25f);
        destination = startPos;
    }
     

    public void DropTheBoxToBurning()
    {
        boxToBurn.transform.DOLocalJump(flameGarbagePos.localPosition, 1, 1, .35f).OnComplete(() =>
        {
            Destroy(boxToBurn,.2f);
            shouldMove = false;
        });
        
    }
}
