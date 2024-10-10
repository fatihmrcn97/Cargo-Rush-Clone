using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomMachine : MachineController
{
    GameObject _convertingItem;

    [SerializeField] private Vector3 lastRotation;

    [SerializeField] private int createCount=3;

    [SerializeField] private Transform toGoLastPostion;

    [SerializeField] private float timeForMove=0,jumpPower=.25f;

   // [SerializeField] private ParticleSystem ps;

    [SerializeField] private int machineWorkCount=1;
    
    private int _machineDefaulutWork;
    private void Awake()
    {
        _stackSystem = GetComponent<StackSystem>();
        anim = GetComponentInChildren<Animator>();
        _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
        _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>();
        startPosOfDropPos = _stackSystem.materialDropPos.localPosition;
        InvokeRepeating(nameof(MachineStartedWorking), 1f, 1f);
        animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT);
        _machineDefaulutWork = machineWorkCount;
    }
      
    private void MachineStartedWorking()
    {
        if (isMachineWorking || convertedMaterials.Count <= 0 || _getMaterialFromMachine.singleMaterial.Count >= _addMaterialToMachine.maxConvertedMaterial) return;
        isMachineWorking = true;

  //      ps.Play();
        anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);

        machineWorkCount = convertedMaterials.Count < 2 ? 1 : _machineDefaulutWork;

        for (int i = 0; i < machineWorkCount; i++)
        {
            _convertingItem = convertedMaterials[^1];
            convertedMaterials.Remove(_convertingItem);
            _addMaterialToMachine._stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
            _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f);
            Destroy(_convertingItem, 6.25f);
        }
    }
 
    public override void Press_Finished()
    {
        for (int j = 0; j < machineWorkCount; j++)
        {
            for (int i = 0; i < createCount; i++)
            {
                GameObject newBox = Instantiate(newProduct,
                    _convertingItem.transform.GetChild(i).transform.position, Quaternion.Euler(0,0,90), null);
                _convertingItem.SetActive(false);
                //   particle2.Play();
                newBox.transform.DOMove(toGoLastPostion.position, timeForMove).OnComplete(() =>
                {
                    newBox.transform.DOLocalRotate(lastRotation, .15f);
                    newBox.transform.transform.DOLocalJump(_stackSystem.materialDropPos.position, .5f, 1, .15f)
                        .OnComplete(()=>_getMaterialFromMachine.singleMaterial.Add(newBox));
                
                    _stackSystem.DropPointHandle();
                    isMachineWorking = false;
                    anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
                });
            }
   //         ps.Stop();
        }
    }


}