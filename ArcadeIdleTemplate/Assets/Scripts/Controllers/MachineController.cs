using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class MachineController : MonoBehaviour, IMachineController
{
    public List<GameObject> convertedMaterials;

    [SerializeField] protected Transform material_machine_enter_pos;

    [HideInInspector] public IStackSystem _stackSystem;

    protected bool isMachineWorking;

    [HideInInspector] public Animator anim;

    protected AddMaterialToMachine _addMaterialToMachine;

    protected GetMaterialFromMachine _getMaterialFromMachine;
 
    [SerializeField] protected float timePerProduce;

    protected Vector3 startPosOfDropPos;

    [SerializeField] protected float stackFinishZposIncreaser;

    protected float animStartValue;


    public virtual void StartWashing()
    {
    }

    public virtual void Press_Finished()
    {
    }

    public void MachineUpgradeUpdate(int currentLevel)
    {
        anim.SetFloat(TagManager.ANIM_SPEED_FLOAT, animStartValue + currentLevel);
    }



    // function 


}