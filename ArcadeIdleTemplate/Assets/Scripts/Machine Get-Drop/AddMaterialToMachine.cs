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
            DropMaterialCorotine= StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<PlayerStackController>()));
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

    private IEnumerator PlayerDroppingMaterialsToTheMachine(PlayerStackController stackController)
    {
        float progressionTime = .1f;
        List<GameObject> tempList = new(stackController.stackedMaterials);
        tempList.Reverse();
        if (stackController.stackedMaterials.Count <= 0) yield break;
        foreach (var currentSingleMaterial in tempList)
        {
            if (!isInTrigger || _machineController.convertedMaterials.Count >= _maxConvertedMaterial) yield break; 
            currentSingleMaterial.transform.SetParent(null);
            // Active collision
            
            currentSingleMaterial.tag = TagManager.PACKABLE_ITEM;
            currentSingleMaterial.GetComponent<BoxCollider>().enabled = true;
            currentSingleMaterial.GetComponent<BoxCollider>().isTrigger = true; 
            
            stackController.stackedMaterials.Remove(currentSingleMaterial);
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
