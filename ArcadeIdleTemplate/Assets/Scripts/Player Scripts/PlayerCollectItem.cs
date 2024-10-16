using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectItem : MonoBehaviour
{

    private const float ProgressTime = .25f;
    private IItemList ItemList;

    private void Awake()
    {
        ItemList = GetComponentInParent<IItemList>();
    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (other.CompareTag(TagManager.COLLECTABLE_ITEM))
        {
            if (ItemList.CheckPlayerHandMax()) return;
            ICollectable collectable =  other.GetComponent<ICollectable>();
            var collectableObj = collectable.GameObject;
            collectable.DeactivateObjAndPhysics();

            collectableObj.transform.SetParent(ItemList.StackTransforms()[ItemList.StackedMaterialList().Count]);
            collectableObj.transform.DOLocalJump(Vector3.zero, .5f, 1, ProgressTime);
            collectableObj.transform.DOLocalRotate(Vector3.zero, ProgressTime);
           
            ItemList.StackedMaterialList().Add(collectableObj);
            Events.MaterialStackedEvent?.Invoke(); 
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PACKABLE_ITEM_SPAWNER))
        {
            PushBackItems();
        }
    }

    private void PushBackItems()
    {
        foreach (var item in ItemList.StackedMaterialList())
        {
            item.GetComponent<ICollectable>().PushInCircle(-transform.forward);
        }
        ItemList.StackedMaterialList().Clear();
    }
}
