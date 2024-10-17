using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Dreamteck.Splines;
using UnityEngine;

public class BoxTapingMachine : MachineController , ITriggerInteraction
{
    private GameObject _convertingItem;

    [SerializeField] private float jumpPower = .25f;

    // [SerializeField] private ParticleSystem ps; 
    [SerializeField] private SplineComputer splineComputer;

    private void Awake()
    {
        _stackSystem = GetComponent<IStackSystem>();
        splineComputer = GetComponentInChildren<SplineComputer>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        InvokeRepeating(nameof(MachineStartedWorking), 1f, 1f);
        // animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT); 
    }

    private void MachineStartedWorking()
    {
        if (convertedMaterials.Count <= 0) return;

        //      ps.Play();
        // anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        _convertingItem = convertedMaterials[^1];
        convertedMaterials.Remove(_convertingItem);
        _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
        _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f)
            .OnComplete(ItemBoxingProcess); 
    }

    private void ItemBoxingProcess()
    {
        _convertingItem.GetComponent<SplineFollower>().spline = splineComputer;
        _convertingItem.GetComponent<SplineFollower>().enabled = true;
    }
    
    
    public void OnTriggerBanting(GameObject item)
    {
        item.GetComponent<SplineFollower>().followSpeed = .55f;
       // var packBox = Instantiate(newProduct, lastBoxPosition.position, Quaternion.Euler(0, 90, 0), null);
    }

    public void PathEnded(GameObject item)
    {
        item.GetComponent<SplineFollower>().enabled = false;
        item.transform.DOLocalRotate(_stackSystem.MaterialDropPositon().rotation.eulerAngles, .15f);
        item.transform.transform.DOLocalJump(_stackSystem.MaterialDropPositon().position, .5f, 1, .15f).OnComplete(() =>
        {
            _getMaterialFromMachine.singleMaterial.Add(item);
            _stackSystem.DropPointHandle(); 
          //  anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
        });
    }

   
}