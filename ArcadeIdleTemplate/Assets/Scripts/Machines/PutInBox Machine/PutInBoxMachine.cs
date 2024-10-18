 using System;
 using System.Collections.Generic;
 using DG.Tweening;
using Dreamteck.Splines;
 using Unity.VisualScripting;
 using UnityEngine;

public class PutInBoxMachine : MachineController , ITriggerInteraction
{
    private GameObject _convertingItem;

    [SerializeField] private Transform lastBoxPosition;
    
    [SerializeField] private float jumpPower = .25f;
    // [SerializeField] private ParticleSystem ps; 
    [SerializeField] private SplineComputer splineComputer;

    [SerializeField] private ItemStatus outItemStatus;
    [SerializeField] private TapedItemStatus outTapedItemStatus;

    [SerializeField] private List<GameObject> newProducts;
    

    private Array _stackSystems;

    private void Awake()
    {
        _stackSystem = GetComponent<IStackSystem>();
        _stackSystems = GetComponents<IStackSystem>();
        splineComputer = GetComponentInChildren<SplineComputer>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
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
        _convertingItem.GetComponent<SplineFollower>().spline = splineComputer;
        _convertingItem.GetComponent<SplineFollower>().enabled = true; 
        
    }

    public void PathEnded(GameObject item)
    {
        item.GetComponent<SplineFollower>().enabled = false;
        var packBox = Instantiate(newProducts[0],lastBoxPosition.position, Quaternion.Euler(0,90,0), null);
        item.transform.DOLocalJump(lastBoxPosition.position, .5f, 1, .25f)
            .OnComplete(() =>
            {
                packBox.GetComponentInChildren<Animator>().SetTrigger("close");
                packBox.GetComponent<IItem>().SetStatus(outItemStatus,outTapedItemStatus);
                item.SetActive(false);
                packBox.transform.DOLocalRotate(_stackSystem.MaterialDropPositon().rotation.eulerAngles, .15f);
                packBox.transform.transform.DOLocalJump(_stackSystem.MaterialDropPositon().position, .5f, 1, .15f).OnComplete(() =>
                    {
                        _getMaterialFromMachine.singleMaterial.Add(packBox);
                        _stackSystem.DropPointHandle();
                        isMachineWorking = false;
                        anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
                    });
            }); 
    }

    public void OnTriggerBanting(GameObject item)
    {
        // Do Nothing for this class
    }
    
     
}