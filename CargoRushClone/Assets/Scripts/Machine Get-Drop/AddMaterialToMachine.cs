using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToMachine : MonoBehaviour
{ 

    public IStackSystem stackSystem;

    private MachineController _machineController;
  
    public int _maxConvertedMaterial;

    private Coroutine DropMaterialCorotine;

    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;

    [SerializeField] private bool isFirstMachine;
    
    private void Start()
    {
        _machineController = GetComponentInParent<MachineController>();
        stackSystem = GetComponent<IStackSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG) || other.CompareTag(TagManager.AI_TAG))
        {
            other.GetComponent<IItemList>().IsInTrigger = true;
            DropMaterialCorotine= StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<IItemList>()));
        }
        
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            if (DropMaterialCorotine != null) StopCoroutine(DropMaterialCorotine);
                other.GetComponent<IItemList>().IsInTrigger = false;
        }

        if (other.CompareTag(TagManager.AI_TAG))
        {
            other.GetComponent<IItemList>().IsInTrigger = false;
        }
    }

    private bool CheckIsMax()
    {
        return _machineController.convertedMaterials.Count >= _maxConvertedMaterial;
    }

    private IEnumerator PlayerDroppingMaterialsToTheMachine(IItemList stackController)
    { 
        float progressionTime = stackController.StackMovementSpeed();
        List<GameObject> tempList = new(stackController.StackedMaterialList());
        tempList.Reverse();
        if (stackController.StackedMaterialList().Count <= 0) yield break;
        if(CheckIsMax()) yield break;
        // yield return new WaitForSeconds(stackController.StackGetGiveDelaySpeed());
            
        foreach (var currentSingleMaterial in tempList)
        {
            var iItem = currentSingleMaterial.GetComponent<IItem>();
            if (!stackController.IsInTrigger || _machineController.convertedMaterials.Count >= _maxConvertedMaterial) yield break; 
            if(itemStatus != iItem.ItemStatus()) continue;
            if(tapedItemStatus != iItem.TapedItemStatus()) continue;
            if(CheckIsMax()) yield break;
            currentSingleMaterial.transform.SetParent(null);
            // Active collision
            
            currentSingleMaterial.tag = TagManager.PACKABLE_ITEM;
            currentSingleMaterial.GetComponent<BoxCollider>().enabled = true;
            currentSingleMaterial.GetComponent<BoxCollider>().isTrigger = true; 
            
            stackController.StackedMaterialList().Remove(currentSingleMaterial);
            stackController.StackPositionHandler();
            _machineController.convertedMaterials.Add(currentSingleMaterial);
            currentSingleMaterial.transform.DOLocalRotate(Vector3.zero, progressionTime); 
            currentSingleMaterial.transform.DOLocalJump(stackSystem.MaterialDropPositon().position, .5f, 1, progressionTime);
            if(isFirstMachine)
                currentSingleMaterial.transform.DOLocalRotate(new Vector3(Random.Range(0,90), Random.Range(0,190), Random.Range(0,290)), progressionTime);
            stackSystem.DropPointHandle();
            Events.MaterialStackedEvent?.Invoke();
            yield return new WaitForSeconds(progressionTime + .02f);
        }
    }


   
}
