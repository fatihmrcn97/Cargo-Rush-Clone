using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PutInBoxMachine : MachineController
{
    private GameObject _convertingItem;

    [SerializeField] private float jumpPower = .25f;

    // [SerializeField] private ParticleSystem ps;

    [SerializeField] private int machineWorkCount = 1;

    private void Awake()
    {
        _stackSystem = GetComponent<IStackSystem>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        InvokeRepeating(nameof(MachineStartedWorking), 1f, .25f);
        // animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT); 
    }

    private void MachineStartedWorking()
    {
        if (isMachineWorking || convertedMaterials.Count <= 0 || _getMaterialFromMachine.singleMaterial.Count >=
            _addMaterialToMachine.maxConvertedMaterial) return;
        isMachineWorking = true;
        //      ps.Play();
        // anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
        _convertingItem = convertedMaterials[^1];
        convertedMaterials.Remove(_convertingItem);
        _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
        _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f);
        Destroy(_convertingItem, 6.25f);
    }
}