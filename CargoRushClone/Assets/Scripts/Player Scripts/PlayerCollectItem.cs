using DG.Tweening;
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
            if (_itemList.StackedMaterialList().Count > 0 &&
                !_itemList.StackedMaterialList()[0].CompareTag(TagManager.COLLECTABLE_ITEM)) return;
            ICollectable collectable =  other.GetComponent<ICollectable>();
            var collectableObj = collectable.GameObject;
            collectable.DeactivateObjAndPhysics();

            PackableItemSpawner.Instance.allCollectables.Remove(collectableObj);
            collectableObj.transform.SetParent(_itemList.StackTransforms()[_itemList.StackedMaterialList().Count]);
            collectableObj.transform.DOLocalJump(Vector3.zero, .5f, 1, ProgressTime);
            collectableObj.transform.DOLocalRotate(Vector3.zero, ProgressTime);
            
            UIManager.instance.InGameCollectablesCount[Helper.GetPoolName(collectableObj.GetComponent<IItem>().CollectableType())] -= 1;
            
            _itemList.StackedMaterialList().Add(collectableObj);
            Events.MaterialStackedEvent?.Invoke(); 
        }


    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PACKABLE_ITEM_SPAWNER) && !transform.parent.CompareTag(TagManager.AI_TAG))
        {
            if(_itemList.StackedMaterialList().Count > 0 && _itemList.StackedMaterialList()[0].CompareTag(TagManager.COLLECTABLE_ITEM))
                PushBackItems();
        }
    }

    private void PushBackItems()
    {
        foreach (var item in _itemList.StackedMaterialList())
        {
            UIManager.instance.InGameCollectablesCount[Helper.GetPoolName(item.GetComponent<IItem>().CollectableType())] += 1;
            item.GetComponent<ICollectable>().PushInCircle(-transform.forward);
        }
        _itemList.StackedMaterialList().Clear();
    }
}
