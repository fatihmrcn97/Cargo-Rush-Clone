using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToMachine : MonoBehaviour
{ 

    [HideInInspector] public IStackSystem stackSystem;

    private MachineController _machineController;

    private bool isInTrigger; 

    public int _maxConvertedMaterial = 50;

    private Coroutine DropMaterialCorotine;

    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;
    
    
    private void Start()
    {
        _machineController = GetComponentInParent<MachineController>();
        stackSystem = GetComponent<IStackSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            isInTrigger = true;
            DropMaterialCorotine= StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<IItemList>()));
        }
        
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            if (DropMaterialCorotine != null) StopCoroutine(DropMaterialCorotine);
            isInTrigger = false;
        }  
    }

    public bool CheckIsMax()
    {
        if (_machineController.convertedMaterials.Count >= _maxConvertedMaterial) return true;
        else return false;
    }

    private IEnumerator PlayerDroppingMaterialsToTheMachine(IItemList stackController)
    { 
        float progressionTime = .1f;
        List<GameObject> tempList = new(stackController.StackedMaterialList());
        tempList.Reverse();
        if (stackController.StackedMaterialList().Count <= 0) yield break;

            
        foreach (var currentSingleMaterial in tempList)
        {
            var iItem = currentSingleMaterial.GetComponent<IItem>();
            if (!isInTrigger || _machineController.convertedMaterials.Count >= _maxConvertedMaterial) yield break; 
            if(itemStatus != iItem.ItemStatus()) continue;
            if(tapedItemStatus != iItem.TapedItemStatus()) continue;
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
            stackSystem.DropPointHandle();
            Events.MaterialStackedEvent?.Invoke();
            yield return new WaitForSeconds(progressionTime + .02f);
        }
    }


   
}
