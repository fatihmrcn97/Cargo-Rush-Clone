using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CustomMachine : MachineController
{
  //   GameObject _convertingItem; 
  //
  //   [SerializeField] private int createCount=3;
  //
  //   [SerializeField] private Transform toGoLastPostion;
  //
  //   [SerializeField] private float timeForMove=0,jumpPower=.25f;
  //
  //  // [SerializeField] private ParticleSystem ps;
  //
  //   [SerializeField] private int machineWorkCount=1;
  //
  //   [SerializeField] private Transform middlePosition , lastBoxPosition;
  //   
  //   
  //   private int _machineDefaulutWork;
  //   private void Awake()
  //   {
  //       _stackSystem = GetComponent<IStackSystem>();
  //       anim = GetComponentInChildren<Animator>();
  //       _addMaterialToMachine = GetComponentInChildren<AddMaterialToMachine>();
  //       _getMaterialFromMachine = GetComponentInChildren<GetMaterialFromMachine>(); 
  //       InvokeRepeating(nameof(MachineStartedWorking), 1f, .25f);
  //       animStartValue = anim.GetFloat(TagManager.ANIM_SPEED_FLOAT);
  //       _machineDefaulutWork = machineWorkCount;
  //   }
  //     
  //   private void MachineStartedWorking()
  //   {
  //       if (isMachineWorking || convertedMaterials.Count <= 0 || _getMaterialFromMachine.singleMaterial.Count >= _addMaterialToMachine._maxConvertedMaterial) return;
  //       isMachineWorking = true;
  //
  // //      ps.Play();
  //       anim.SetBool(TagManager.WALKING_BOOL_ANIM, true);
  //
  //       machineWorkCount = convertedMaterials.Count < 2 ? 1 : _machineDefaulutWork;
  //
  //       for (int i = 0; i < machineWorkCount; i++)
  //       {
  //           _convertingItem = convertedMaterials[^1];
  //           convertedMaterials.Remove(_convertingItem);
  //           _addMaterialToMachine.stackSystem.SetTheStackPositonBack(convertedMaterials.Count);
  //           _convertingItem.transform.DOLocalJump(material_machine_enter_pos.position, jumpPower, 1, .15f);
  //           Destroy(_convertingItem, 6.25f);
  //       }
  //   }
  //
  //   public override void Press_Finished()
  //   {
  //       for (int j = 0; j < machineWorkCount; j++)
  //       {
  //           for (int i = 0; i < createCount; i++)
  //           {
  //               // GameObject newBox = Instantiate(newProduct,
  //               //     _convertingItem.transform.GetChild(i).transform.position, Quaternion.Euler(0,0,90), null);
  //               // _convertingItem.SetActive(false);
  //
  //               var packableItemBeforePack = _convertingItem; 
  //               //   particle2.Play();
  //               packableItemBeforePack.transform.DOMove(middlePosition.position, timeForMove).OnComplete(() =>
  //               {
  //                   var packBox = Instantiate(newProduct,lastBoxPosition.position, Quaternion.Euler(0,0,90), null);
  //                   packableItemBeforePack.transform.DOMove(toGoLastPostion.position, timeForMove).OnComplete(() =>
  //                   {
  //                       packableItemBeforePack.transform.DOLocalJump(lastBoxPosition.position, .5f, 1, .15f)
  //                           .OnComplete(() =>
  //                           {
  //                               packableItemBeforePack.SetActive(false);
  //                               packBox.transform.DOLocalRotate(_stackSystem.MaterialDropPositon().rotation.eulerAngles, .15f);
  //                               packBox.transform.transform.DOLocalJump(_stackSystem.MaterialDropPositon().position, .5f, 1, .15f).OnComplete(
  //                                       () =>
  //                                       {
  //                                           _getMaterialFromMachine.singleMaterial.Add(packBox);
  //                                           _stackSystem.DropPointHandle();
  //                                           isMachineWorking = false;
  //                                           anim.SetBool(TagManager.WALKING_BOOL_ANIM, false);
  //                                       });
  //                           });
  //                   });
  //               });
  //           }
  //            //         ps.Stop();
  //       }
  //   }


}