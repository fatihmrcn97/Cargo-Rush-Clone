using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollectItem : MonoBehaviour
{

    private const float ProgressTime = .25f;
    private IItemList _itemList;

    private void Awake()
    {
        _itemList = GetComponentInParent<IItemList>();
    }

    private void OnTriggerEnter(Collider other)
    {
         
        if (other.CompareTag(TagManager.COLLECTABLE_ITEM))
        {
            if (_itemList.CheckPlayerHandMax()) return;
            ICollectable collectable =  other.GetComponent<ICollectable>();
            var collectableObj = collectable.GameObject;
            collectable.DeactivateObjAndPhysics();

            collectableObj.transform.SetParent(_itemList.StackTransforms()[_itemList.StackedMaterialList().Count]);
            collectableObj.transform.DOLocalJump(Vector3.zero, .5f, 1, ProgressTime);
            collectableObj.transform.DOLocalRotate(Vector3.zero, ProgressTime);
           
            _itemList.StackedMaterialList().Add(collectableObj);
            Events.MaterialStackedEvent?.Invoke(); 
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PACKABLE_ITEM_SPAWNER))
        {
            if(_itemList.StackedMaterialList().Count > 0 && _itemList.StackedMaterialList()[0].CompareTag(TagManager.COLLECTABLE_ITEM))
                PushBackItems();
        }
    }

    private void PushBackItems()
    {
        foreach (var item in _itemList.StackedMaterialList())
        {
            item.GetComponent<ICollectable>().PushInCircle(-transform.forward);
        }
        _itemList.StackedMaterialList().Clear();
    }
}
