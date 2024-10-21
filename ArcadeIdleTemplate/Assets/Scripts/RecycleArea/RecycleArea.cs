using System;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

public class RecycleArea : MonoBehaviour
{
    [SerializeField] private GameObject recycle_karakter;

    [SerializeField] private Transform boxGetTransform;

    [SerializeField] private RecyclerController recyclerControllerKarakter;
     
    [SerializeField] private Transform _startTransform;

    private bool oneTime = true;

    private AddMaterialToRecycle _addMaterialToRecycle;
 
    private void Awake()
    { 
        _addMaterialToRecycle = GetComponentInChildren<AddMaterialToRecycle>();  
    }

    private void Start()
    {
        InvokeRepeating(nameof(CheckIsThereItem),2,2);
    }

    private void CheckIsThereItem()
    {
        if(recyclerControllerKarakter.shouldMove) return;
        if(_addMaterialToRecycle.recycleMaterials.Count<=0) return;

        recyclerControllerKarakter.SetDestination(boxGetTransform);
        recyclerControllerKarakter.shouldMove = true;

    }


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(TagManager.AI_RECYCLER_TAG)) return; 
        if (oneTime)
        {
            oneTime = false; 
            var itemToDispose = _addMaterialToRecycle.recycleMaterials[^1];
            _addMaterialToRecycle.recycleMaterials.Remove(itemToDispose);
            recyclerControllerKarakter.SetBox(itemToDispose,_startTransform);
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.AI_RECYCLER_TAG))
        {
            oneTime = true;
        }
    }
}
