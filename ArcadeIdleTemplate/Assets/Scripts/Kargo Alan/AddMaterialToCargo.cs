using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class AddMaterialToCargo : MonoBehaviour
{
    private IStackSystem stackSystem;
    private bool isInTrigger;
    private int _maxConvertedMaterial;
    private Coroutine DropMaterialCorotine;

    [SerializeField] private ItemStatus itemStatus;
    [SerializeField] private TapedItemStatus tapedItemStatus;

    public TapedItemStatus TapedItemStatus
    {
        get => tapedItemStatus;
        set => tapedItemStatus = value;
    }

    private CargoPlace _cargoPlace;


    private void Awake()
    {
        _cargoPlace = GetComponentInParent<CargoPlace>();
        _maxConvertedMaterial = _cargoPlace.maxConvertedMaterial;
        _cargoPlace.remaningText.text = "0/" + _maxConvertedMaterial;
    }

    private void Start()
    {
        stackSystem = GetComponent<IStackSystem>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag(TagManager.PLAYER_TAG))
        {
            _maxConvertedMaterial = _cargoPlace.maxConvertedMaterial;
            isInTrigger = true;
            DropMaterialCorotine =
                StartCoroutine(PlayerDroppingMaterialsToTheMachine(other.GetComponent<IItemList>()));
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


    private IEnumerator PlayerDroppingMaterialsToTheMachine(IItemList stackController)
    {
        float progressionTime = .1f;
        List<GameObject> tempList = new(stackController.StackedMaterialList());
        tempList.Reverse();
        if (stackController.StackedMaterialList().Count <= 0) yield break;

        foreach (var currentSingleMaterial in tempList)
        {
            if (!isInTrigger || _cargoPlace.IsCurrierGoing) yield break;
            var iItem = currentSingleMaterial.GetComponent<IItem>();
            if (itemStatus != iItem.ItemStatus()) continue;
            if (tapedItemStatus != iItem.TapedItemStatus()) continue;
       
            currentSingleMaterial.transform.SetParent(stackSystem.MaterialDropPositon().parent);
            // Active collision 
            stackController.StackedMaterialList().Remove(currentSingleMaterial);
            stackController.StackPositionHandler();
            _cargoPlace.cargoItems.Add(currentSingleMaterial);
            currentSingleMaterial.transform.DOLocalRotate(Vector3.zero, progressionTime);
            currentSingleMaterial.transform.DOLocalJump(stackSystem.MaterialDropPositon().localPosition, .5f, 1,
                progressionTime);
            stackSystem.DropPointHandle();
            Events.MaterialStackedEvent?.Invoke();
            _cargoPlace.remaningText.text = _cargoPlace.cargoItems.Count + "/" + _maxConvertedMaterial;

            UIManager.instance.InGameCollectablesCount[Helper.GetPoolName(tapedItemStatus)] -= 1;

            if (_cargoPlace.cargoItems.Count >= _maxConvertedMaterial)
            {
                _cargoPlace.CargoFullCurrierGo();
                stackSystem.SetTheStackPositonBack(0);
                yield break;
            }

            yield return new WaitForSeconds(progressionTime + .02f);
        }
    }
}