 using System;
 using System.Collections.Generic;
 using System.Linq;
 using DG.Tweening;
using Dreamteck.Splines; 
 using UnityEngine;

public class PutInBoxMachine : MachineController , ITriggerInteraction
{
    private GameObject _convertingItem;

    [SerializeField] private List<Transform> lastBoxPosition;
    
    [SerializeField] private float jumpPower = .25f;
    // [SerializeField] private ParticleSystem ps; 
    [SerializeField] private List<SplineComputer> splineComputers;

    [SerializeField] private ItemStatus outItemStatus;
    [SerializeField] private TapedItemStatus outTapedItemStatus;

    [SerializeField] private List<GameObject> newProducts;

    [SerializeField] private List<GetMaterialFromMachine> _getMaterialFromMachines;
    


    private void Awake()
    { 
       var stackSystemsTem = GetComponents<IStackSystem>();
       _stackSystems = stackSystemsTem.ToList();
       
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
      //  _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        InvokeRepeating(nameof(MachineStartedWorking), 1f, 1f);
        // animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT); 

       // Debug.Log(  stackSystems.GetValue(1));
    }

    private void MachineStartedWorking()
    {
        if (convertedMaterials.Count <= 0) return; 
        
        //      ps.Play();
        // anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        _convertingItem = convertedMaterials[^1];
        convertedMaterials.Remove(_convertingItem);
        _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
        _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f).OnComplete(ItemBoxingProcess);
        Destroy(_convertingItem, 6.25f);
    }

    private void ItemBoxingProcess()
    {
        var ctype = _convertingItem.GetComponent<Collectable>().CollectableTypes;
        _convertingItem.GetComponent<SplineFollower>().spline = splineComputers[GetCollectableItemIndex(ctype)];
        _convertingItem.GetComponent<SplineFollower>().enabled = true; 
        
    }

    public void PathEnded(GameObject item)
    {
        var type =item.GetComponent<Collectable>().CollectableTypes;
        var index = GetCollectableItemIndex(type);
        item.GetComponent<SplineFollower>().enabled = false;
        var packBox = Instantiate(newProducts[index],lastBoxPosition[index].position, Quaternion.Euler(0,90,0), null);
        item.transform.DOLocalJump(lastBoxPosition[index].position, .5f, 1, .25f)
            .OnComplete(() =>
            {
                packBox.GetComponentInChildren<Animator>().SetTrigger("close");
                packBox.GetComponent<IItem>().SetStatus(outItemStatus,CollectableItemTapedType(type));
                item.SetActive(false);
                packBox.transform.DOLocalRotate(_stackSystems[index].MaterialDropPositon().rotation.eulerAngles, .15f);
                packBox.transform.transform.DOLocalJump(_stackSystems[index].MaterialDropPositon().position, .5f, 1, .15f).OnComplete(() =>
                    {
                        _getMaterialFromMachines[index].singleMaterial.Add(packBox);
                        _stackSystems[index].DropPointHandle();
                        isMachineWorking = false;
                        anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
                    });
            }); 
    }

    public void OnTriggerBanting(GameObject item)
    {
        // Do Nothing for this class
    }

    private int GetCollectableItemIndex(CollectableTypes cType)// input olarak type girilecek
    {
        return cType switch
        {
            CollectableTypes.duck => 0,
            CollectableTypes.pinkduck => 1,
            _ => 0
        };
    }

    private TapedItemStatus CollectableItemTapedType(CollectableTypes cType)// input olarak type girilecek
    {
        return cType switch
        {
            CollectableTypes.duck => TapedItemStatus.yellowBox,
            CollectableTypes.pinkduck => TapedItemStatus.pinkBox,
            _ => 0
        };
    }

}