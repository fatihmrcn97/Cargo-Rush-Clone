using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddMaterialToMachine : MonoBehaviour
{
    public IStackSystem stackSystem;

    private MachineController _machineController;

    public int _maxConvertedMaterial;

    private Coroutine DropMaterialCorotine, DropMaterialCoroutineAI;

    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;

    [SerializeField] private bool isFirstMachine;

    private ISave _saveSystem;

    [SerializeField] private AddMachineTypes machineType;

    [SerializeField] private string saveName;
    

    private IEnumerator Start()
    {
        _machineController = GetComponentInParent<MachineController>();
        stackSystem = GetComponent<IStackSystem>();
        _saveSystem = machineType switch
        {
            AddMachineTypes.CollectibleMachine => new AddMaterialSaveCollectable(stackSystem, _machineController),
            AddMachineTypes.BantTapingMachine => new AddMateriaSaveBantMachine(stackSystem, _machineController,saveName),
            _ => _saveSystem
        };

        _machineController.AddMaterialSaveCollectable = _saveSystem;

        yield return new WaitForSeconds(1.1f);
        _saveSystem.LoadData();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagManager.PLAYER_TAG))
        {
            other.GetComponent<IItemList>().IsInTrigger = true;
            DropMaterialCorotine = StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<IItemList>()));
        }

        if (other.CompareTag(TagManager.AI_TAG))
        {
            other.GetComponent<IItemList>().IsInTrigger = true;
            DropMaterialCoroutineAI =
                StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<IItemList>()));
            if (other.GetComponent<AICargoStateManager>() != null)
                other.GetComponent<AICargoStateManager>().RandomGetMaterialFromMachine();
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
            //if (DropMaterialCoroutineAI != null) StopCoroutine(DropMaterialCoroutineAI);
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
        if(!isFirstMachine)
            tempList.Reverse();
        if (stackController.StackedMaterialList().Count <= 0) yield break;
        if (CheckIsMax()) yield break;
        // yield return new WaitForSeconds(stackController.StackGetGiveDelaySpeed());

        foreach (var currentSingleMaterial in tempList)
        {
            var iItem = currentSingleMaterial.GetComponent<IItem>();
            if (!stackController.IsInTrigger ||
                _machineController.convertedMaterials.Count >= _maxConvertedMaterial) yield break;
            if (itemStatus != iItem.ItemStatus()) continue;
            if (tapedItemStatus != iItem.TapedItemStatus()) continue;
            if (CheckIsMax()) yield break;
            // Active collision

            currentSingleMaterial.tag = TagManager.PACKABLE_ITEM;
            currentSingleMaterial.GetComponent<BoxCollider>().enabled = true;
            currentSingleMaterial.GetComponent<BoxCollider>().isTrigger = true;

            stackController.StackedMaterialList().Remove(currentSingleMaterial);
            stackController.StackPositionHandler();
            _machineController.convertedMaterials.Add(currentSingleMaterial);
            
            currentSingleMaterial.transform.SetParent(null);
            
            if(machineType == AddMachineTypes.CollectibleMachine)
                _saveSystem.SaveData((int)iItem.CollectableType());
            else 
                _saveSystem.SaveData((int)tapedItemStatus-1);
         
            currentSingleMaterial.transform.DOLocalJump(stackSystem.MaterialDropPositon().position, .5f, 1,
                progressionTime);
            currentSingleMaterial.transform.DOLocalRotate(
                isFirstMachine
                    ? new Vector3(Random.Range(0, 90), Random.Range(0, 190), Random.Range(0, 290))
                    : Vector3.zero, progressionTime);
            
            stackSystem.DropPointHandle();
            Events.MaterialStackedEvent?.Invoke();

            yield return isFirstMachine
                ? new WaitForSeconds(0.1f)
                : new WaitForSeconds(stackController.StackGetGiveDelaySpeed());
        }
    }
 
}